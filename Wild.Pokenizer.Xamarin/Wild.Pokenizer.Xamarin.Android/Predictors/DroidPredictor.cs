using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading.Tasks;
using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using Java.Nio.FileNio;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;
using Xamarin.TensorFlow.Lite;
using File = System.IO.File;
using Object = Java.Lang.Object;
using Path = System.IO.Path;

namespace Wild.Pokenizer.Xamarin.Droid.Predictors
{
    public class DroidPredictor : IPredictor
    {
        public const string LabelsFileName = "Pokemon_Labels.txt";
        public const string ModelFileName = "Pokemon_Model.tflite";
        private const Environment.SpecialFolder ModelStorageFolder = Environment.SpecialFolder.Personal;

        private const int ImageHeight = 160;
        private const int ImageWidth = 160;
        private const int NumBytesPerChannel = 3;
        private const int BatchSize = 1;
        private const int PixelSize = 3;
        private readonly IAssetLoader _assetLoader;
        private IList<string> _labels;
        private byte[] _model;
        private MemoryMappedFile _modelFile;
        private MappedByteBuffer _modelBuffer;

        public string Name => typeof(TensorFlowLite).AssemblyQualifiedName;

        public string Version => TensorFlowLite.Version();

        public DroidPredictor(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public async Task<PredictionResult> PredictAsync(string input)
        {
            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from Android string."
                    });
        }

        public async Task<PredictionResult> PredictAsync(Stream stream)
        {
            //https://www.tensorflow.org/lite/models/image_classification/android
            //Code https://github.com/tensorflow/examples/blob/master/lite/examples/image_classification/android/app/src/main/java/org/tensorflow/lite/examples/classification/tflite/Classifier.java

            //var tf = new Tensor();
            _labels = _labels ?? (_labels = await LoadLabels(LabelsFileName));
            //_model = _model ?? (_model = await LoadModel(ModelFileName));
            //_modelFile = _modelFile ?? (_modelFile = await LoadModelFile(ModelFileName));
            _modelBuffer = _modelBuffer ?? (_modelBuffer = await LoadModelFileBuffer(ModelFileName));
            //var device = Android.App.Application.Context.Dev
            //Xamarin.T.TensorFlow.Lite.
            //var x = new TensorFlowInferenceInterface()
            //TODO: Look into GPU - GpuHelperDelegate g;

            //TODO: Pass from UI?
            var numThreads = 1;

            var tfOptions = new Interpreter.Options();
            tfOptions.SetNumThreads(numThreads);

            //TODO: Look into use of GPU delegate vs NNAPI
            // https://developer.android.com/ndk/guides/neuralnetworks
            
            //https://stackoverflow.com/questions/30716027/c-sharp-equivalent-of-java-memory-mapping-methods
            //ByteBuffer.FromArray()
            //var buff = new ByteBuffer()
            var interpreter = new Interpreter(_modelBuffer, tfOptions);

            //var imgData =  new int[ImageWidth * ImageHeight];
            //bitmap.GetPixels(argbPixelArray, 0, bitmap.Width, 0, 0, bitmap.Width, bitmap.Height);
            var bitmap = CreateBitmap(stream);
            var imgData = ConvertBitmapToByteBuffer(bitmap);
            //var normalizedPixelComponents = new float[argbPixelArray.Length * 3];

            var labelProbArray = new float[_labels.Count];

            //TODO: Reshape data and pass the right types in here
            interpreter.Run(imgData, labelProbArray);


            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from Android stream."
                    });
        }

        private Bitmap CreateBitmap(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
                return BitmapFactory.DecodeStream(stream);
        }

        private ByteBuffer ConvertBitmapToByteBuffer(Bitmap bitmap)
        {

            var imgData =
                ByteBuffer.AllocateDirect(
                    BatchSize
                    * ImageWidth
                    * ImageHeight
                    * PixelSize
                    * NumBytesPerChannel);
            imgData.Order(ByteOrder.NativeOrder());

            return imgData;
        }

        private async Task<IList<string>> LoadLabels(string fileName)
        {
            using (var stream = _assetLoader.GetStream(fileName))
            using (var sr = new StreamReader(stream))
            {
                var content = await sr.ReadToEndAsync();
                return content.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);
            }
        }

        private async Task<byte[]> LoadModel(string fileName)
        {
            var buffer = new byte[16 * 1024];
            using (var stream = _assetLoader.GetStream(fileName))
            {
                using (var ms = new MemoryStream())
                {
                    int read;
                    while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }

        //private async Task<MemoryMappedFile> LoadModelFile(string fileName)
        //{
        //    var mmf = MemoryMappedFile.CreateFromFile(fileName, FileMode.Read);
        //    using (MemoryMappedViewStream vs = mmf.CreateViewStream())
        //    {
        //        // perform stream operations
        //    }

        //    return mmf;
        //}

        private async Task<MappedByteBuffer> LoadModelFileBuffer(string fileName)
        {
            //Need to make a local copy of the file
            var filePath = await CopyAssetFileToInternalStorageAsync(fileName);

            //var mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open);
            //using (var vs = mmf.CreateViewStream())
            //{
            //    // perform stream operations
            //}

            //return mmf;

            var fileDescriptor = Application.Context.Assets.OpenFd(filePath);
            var inputStream = new FileInputStream(fileDescriptor.FileDescriptor);
            var fileChannel = inputStream.Channel;
            var startOffset = fileDescriptor.StartOffset;
            var declaredLength = fileDescriptor.DeclaredLength;
            return fileChannel.Map(FileChannel.MapMode.ReadOnly, startOffset, declaredLength);
        }


        private async Task<string> CopyAssetFileToInternalStorageAsync(string fileName)
        {
            var targetPath = Path.Combine(Environment.GetFolderPath(ModelStorageFolder), fileName);

            if (!File.Exists(targetPath))
            {
                try
                {
                    using (var assetStream = _assetLoader.GetStream(fileName))
                    using (var outputFileStream = new FileStream(targetPath, FileMode.OpenOrCreate))
                    {
                        var buffer = new byte[1024];

                        var b = buffer.Length;
                        int length;

                        while ((length = await assetStream.ReadAsync(buffer, 0, b)) > 0)
                        {
                            await outputFileStream.WriteAsync(buffer, 0, length);
                        }

                        outputFileStream.Flush();
                        assetStream.Close();
                        assetStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    //Handle exceptions
                }
            }

            return targetPath;
        }
    }
}
