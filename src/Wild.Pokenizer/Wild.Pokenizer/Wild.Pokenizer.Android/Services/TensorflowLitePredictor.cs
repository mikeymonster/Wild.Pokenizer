using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;
using Xamarin.TensorFlow.Lite;
using Environment = System.Environment;

namespace Wild.Pokenizer.Droid.Services
{
    public class TensorflowLitePredictor : IPredictor
    {
        public const string LabelsFileName = "labels.txt";
        public const string ModelFileName = "model.tflite";

        public const int FloatSize = 4;
        public const int PixelSize = 3;
        private const Environment.SpecialFolder ModelStorageFolder = Environment.SpecialFolder.Personal;

        private readonly IAssetLoader _assetLoader;

        public string Name => typeof(TensorFlowLite).AssemblyQualifiedName;

        public string Version => TensorFlowLite.RuntimeVersion();

        public event EventHandler<PredictionCompletedEventArgs> PredictionCompleted;

        public TensorflowLitePredictor(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public async Task<IEnumerable<Prediction>> PredictAsync(Stream input)
        {
            var memoryStream = new MemoryStream();
            await input.CopyToAsync(memoryStream);
            return await ClassifyAsync(memoryStream.ToArray());
        }

        public async Task<IEnumerable<Prediction>> ClassifyAsync(byte[] bytes)
        {
            var mappedByteBuffer = GetModelAsMappedByteBuffer();

            //var interpreter = new Xamarin.TensorFlow.Lite.Interpreter(mappedByteBuffer);

            System.Console.WriteLine($"Running Tensorflow interpreter");
            System.Console.WriteLine($"Tensorflow runtime version {TensorFlowLite.RuntimeVersion()}");
            System.Console.WriteLine($"Tensorflow schema version {TensorFlowLite.SchemaVersion()}");
            
            var interpreterOptions = new Interpreter.Options();
            //TODO: Pass from UI or settings?
            var numThreads = 1;
            interpreterOptions.SetNumThreads(numThreads);

            //TODO: Look into use of GPU delegate vs NNAPI
            // https://developer.android.com/ndk/guides/neuralnetworks
            interpreterOptions.SetAllowFp16PrecisionForFp32(true);
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.OMr1)
            {
                interpreterOptions.SetUseNNAPI(true);
            }

            //var interpreter = new Interpreter(mappedByteBuffer);
            var interpreter = new Interpreter(mappedByteBuffer, interpreterOptions);

            var tensor = interpreter.GetInputTensor(0);

            var shape = tensor.Shape();

            var width = shape[1];
            var height = shape[2];

            var labels = await LoadLabelsAsync(LabelsFileName);
            var byteBuffer = GetPhotoAsByteBuffer(bytes, width, height);

            //var outputLocations = new float[1][] { new float[labels.Count] };
            var outputLocations = new[] { new float[labels.Count] };

            var outputs = Java.Lang.Object.FromArray(outputLocations);

            interpreter.Run(byteBuffer, outputs);

            var classificationResult = outputs.ToArray<float[]>();

            var result = new List<Prediction>();

            for (var i = 0; i < labels.Count; i++)
            {
                var label = labels[i];
                result.Add(new Prediction(label, classificationResult[0][i]));
            }

            //TODO: Consider using this event or MediatR to return results to view model
            //https://blog.duijzer.com/posts/mvvmcross_with_mediatr/
            PredictionCompleted?.Invoke(this, new PredictionCompletedEventArgs(result));

            return result;
        }

        private MappedByteBuffer GetModelAsMappedByteBuffer()
        {
            var assetDescriptor = Application.Context.Assets.OpenFd(ModelFileName);
            var inputStream = new FileInputStream(assetDescriptor.FileDescriptor);

            return inputStream.Channel.Map(
                FileChannel.MapMode.ReadOnly,
                assetDescriptor.StartOffset,
                assetDescriptor.DeclaredLength);
        }

        private ByteBuffer GetPhotoAsByteBuffer(byte[] bytes, int width, int height)
        {
            var modelInputSize = FloatSize * height * width * PixelSize;

            var bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            var resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);

            var byteBuffer = ByteBuffer.AllocateDirect(modelInputSize);
            byteBuffer.Order(ByteOrder.NativeOrder());

            var pixels = new int[width * height];
            resizedBitmap.GetPixels(pixels, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

            var pixel = 0;

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var pixelVal = pixels[pixel++];

                    byteBuffer.PutFloat(pixelVal >> 16 & 0xFF);
                    byteBuffer.PutFloat(pixelVal >> 8 & 0xFF);
                    byteBuffer.PutFloat(pixelVal & 0xFF);
                }
            }

            bitmap.Recycle();

            return byteBuffer;
        }

        private async Task<IList<string>> LoadLabelsAsync(string fileName)
        {
            await using var stream = _assetLoader.GetStream(fileName);
            using var sr = new StreamReader(stream);
            
            var labels = (await sr.ReadToEndAsync())
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            return labels;
        }

        /*
         * Methods below here are from the old version of the classifier
         */

        //private async Task<MappedByteBuffer> LoadModelFileBuffer(string fileName)
        // ReSharper disable once UnusedMember.Local
        private MappedByteBuffer LoadModelFileBuffer(string fileName)
        {
            //Need to make a local copy of the file
            //var filePath = await CopyAssetFileToInternalStorageAsync(fileName);

            //var mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open);
            //using (var vs = mmf.CreateViewStream())
            //{
            //    // perform stream operations
            //}

            //return mmf;

            //File descriptor fails to find file - possibly only used for Asset?

            //Here we open the copied file instead of the asset stream
            //If using a file directly, the file can be passed into the interpreter
            //var inputStream = new FileInputStream(filePath);

            var fileDescriptor = Application.Context.Assets.OpenFd(fileName);
            var inputStream = new FileInputStream(fileDescriptor.FileDescriptor);
            var fileChannel = inputStream.Channel;
            var startOffset = fileDescriptor.StartOffset;
            var declaredLength = fileDescriptor.DeclaredLength;
            return fileChannel.Map(FileChannel.MapMode.ReadOnly, startOffset, declaredLength);
        }

        // ReSharper disable once UnusedMember.Local
        private async Task<string> CopyAssetFileToInternalStorageAsync(string fileName)
        {
            var targetPath = System.IO.Path.Combine(Environment.GetFolderPath(ModelStorageFolder), fileName);

            if (!System.IO.File.Exists(targetPath))
            {
                try
                {
                    await using var assetStream = _assetLoader.GetStream(fileName);
                    await using var outputFileStream = new FileStream(targetPath, FileMode.OpenOrCreate);

                    var buffer = new byte[1024];

                    var b = buffer.Length;
                    int length;

                    while ((length = await assetStream.ReadAsync(buffer, 0, b)) > 0)
                    {
                        await outputFileStream.WriteAsync(buffer, 0, length);
                    }

                    outputFileStream.Flush();
                }
#pragma warning disable 168
                catch (Exception ex)
#pragma warning restore 168
                {
                    //Handle exceptions
                }
            }

            return targetPath;
        }
    }
}