using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Runtime;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;
using Xamarin.TensorFlow.Lite;
using Environment = System.Environment;
using File = System.IO.File;
using Path = System.IO.Path;

namespace Wild.Pokenizer.Xamarin.Droid.Predictors
{
    public class DroidPredictor : IPredictor
    {
        public const string LabelsFileName = "Pokemon_Labels.txt";
        public const string ModelFileName = "Pokemon_Model.tflite";
        private const Environment.SpecialFolder ModelStorageFolder = Environment.SpecialFolder.Personal;

        private const bool NormalizeMobileNetInputs = false;

        private const int ImageHeight = 160;
        private const int ImageWidth = 160;
        private const int NumBytesPerChannel = 4;//3;
        private const int BatchSize = 1;
        private const int PixelSize = 3;
        private readonly IAssetLoader _assetLoader;
        private IList<string> _labels;
        //private byte[] _model;
        //private MemoryMappedFile _modelFile;
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

            //Kotlin example https://medium.com/@teresa.wu/tensorflow-image-recognition-on-android-with-kotlin-cee8d977ae9
            // - code at https://github.com/teresawu/random

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
            //tfOptions.SetUseNNAPI(true);
            tfOptions.SetAllowFp16PrecisionForFp32(true);

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

            //var arr = new float[1, _labels.Count];
            //var arr1 = new float[_labels.Count];
            //var labelProbArray = new float[_labels.Count];
            //var objectArray = new object[1];
            var labelProbArray = new float[1][] {
                new float[_labels.Count] 
                };

            //https://forums.xamarin.com/discussion/1930/creating-arrays-of-custom-java-objects-in-jni
            //var labelProbArray = Java.Lang.Object.FromArray<IJavaObject>();
            //var javaProbArray = Java.Lang.Object.FromArray(labelArray);
            //var javaObjectArray = Java.Lang.Object.FromArray(objectArray);
            var javaLabelProbArray = Java.Lang.Object.FromArray(labelProbArray);
            //var labelProbArray = Java.Lang.Object.To(labelArray);

            var javaLabelProbArray1 = Java.Lang.Object.FromArray(labelProbArray[0]);
            //var javaLabelProbArray1 = new 
            var javaProbArrayContainer = new Java.Lang.Object[] { javaLabelProbArray1 };

            var floatArrayArray = new Java.Lang.Object(JNIEnv.NewArray(labelProbArray), JniHandleOwnership.TransferLocalRef);

            //var array = JNIEnv.NewArray(arr);
            //Java.Lang.Object[] values = arr
            //    .Select(b => new Java.Lang.Object(b.ToLocalJniHandle(), JniHandleOwnership.TransferLocalRef))
            //    .ToArray();

            //var intPtrFloatArray = JNIEnv.NewArray<float>(labelProbArray);
            //var labelProbArrayContainer = new Java.Lang.Object[] { javaLabelProbArray };

            //values2[0] = labelProbArray;

            //Array<ByteArray>

            try
            {
                // Run the inference
                //Try RunForMultipleInputsOutputs
                //    ?? GetOutputTensor
                //  - see https://github.com/tensorflow/tensorflow/issues/25751
                // or https://devblogs.microsoft.com/xamarin/android-apps-tensorflow/
                interpreter.Run(imgData, floatArrayArray);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }

            //var outputArrayFirstDimension = javaProbArrayContainer[0];
            //var outputs = outputArrayFirstDimension.ToArray<float[][]>();

            //https://stackoverflow.com/questions/17305522/mono-for-android-binding-jagged-array

            //float[][] payload = (float[][])JNIEnv.GetArray(floatArrayArray, JniHandleOwnership.DoNotTransfer, typeof(float[]));
            //float[][] payload = JNIEnv.GetArray<float[]>(javaLabelProbArray);
            float[][] outputs = JNIEnv.GetArray<float[]>(floatArrayArray.Handle);
            for (int i = 0; i < outputs[0].Length; i++)
            {
                if (!float.IsNaN(outputs[0][i]))
                {
                }

                if (outputs[0][i] > 0)
                {
                }
            }


            /*
            using (var byteArrayArray = new Java.Lang.Object(JNIEnv.NewArray(data), JniHandleOwnership.TransferLocalRef))
            {
                Console.WriteLine("# jonp [[b: {0}", JNIEnv.GetClassNameFromInstance(byteArrayArray.Handle));
                byte[][] data2 = JNIEnv.GetArray<byte[]>(byteArrayArray.Handle);
                */

            //https://stackoverflow.com/questions/6594250/type-cast-from-java-lang-object-to-native-clr-type-in-monodroid
            var obj = javaLabelProbArray;
            var propertyInfo = obj.GetType().GetProperty("Instance");
            var result = propertyInfo == null
                ? null
                : propertyInfo.GetValue(obj, null) as float[][];

            //var outputs = javaLabelProbArray.ToArray<float[][]>();


            // Find the best classifications.
            var recognitions = new List<Recognition>(_labels.Count);
            for (var i = 0; i < _labels.Count; i++)
            {
                recognitions.Add(new Recognition
                (
                    i.ToString(),
                    _labels[i],
                    labelProbArray[0][i],
                    null
                    ));
            }
            // Sort high-to-low via confidence 
            //Array.Sort(results, (x, y) => y.Confidence.CompareTo(x.Confidence));

            var orderedRecognitions = recognitions.OrderByDescending(x => x.Confidence).ToList();
            foreach (var recognition in orderedRecognitions)
            {
                System.Console.WriteLine($"Result {recognition.Title} with confidence {recognition.Confidence}");
            }
            /*
            var pq =
                new PriorityQueue<Recognition>(
                    3,
                    new Comparator<Recognition>() {
                        @Override
                        public int compare(Recognition lhs, Recognition rhs)
                        {
                        // Intentionally reversed to put high confidence at the head of the queue.
                        return Float.compare(rhs.getConfidence(), lhs.getConfidence());
                    }
            });
            for (int i = 0; i < labels.size(); ++i) {
                pq.add(
                    new Recognition(
                        "" + i,
                        labels.size() > i? labels.get(i) : "unknown",
                        getNormalizedProbability(i),
                        null));
            }
            final ArrayList<Recognition> recognitions = new ArrayList<Recognition>();
            int recognitionsSize = Math.min(pq.size(), MAX_RESULTS);
            for (int i = 0; i<recognitionsSize; ++i) {
                recognitions.add(pq.poll());
            }
            */

            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = $"{orderedRecognitions.First().Title} with confidence {orderedRecognitions.First().Confidence:0.##}"
                    });
        }

        private Bitmap CreateBitmap(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var bitmap = BitmapFactory.DecodeStream(stream);

            var resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, ImageWidth, ImageHeight, false);
            return resizedBitmap;
        }

        private ByteBuffer AllocateByteBuffer()
        {
            var bufferSize = BatchSize
                             * ImageWidth
                             * ImageHeight
                             * PixelSize
                             * NumBytesPerChannel
                             * (NormalizeMobileNetInputs ? 4 : 1);

            var imgData = ByteBuffer.AllocateDirect(bufferSize);
            imgData.Order(ByteOrder.NativeOrder());
            return imgData;
        }

        private ByteBuffer ConvertBitmapToByteBuffer(Bitmap bitmap)
        {
            //Original sample allocates imgData in constructor
            var imgData = AllocateByteBuffer();

            //Shouldn't need this, unless it was already allocated
            //if (imgData == null)
            //{
            //    return;
            //}
            imgData.Rewind();
            var intValues = new int[ImageWidth * ImageHeight];
            //var argbPixelArray = new int[bitmap.Width * bitmap.Height];

            bitmap.GetPixels(intValues, 0, bitmap.Width, 0, 0, bitmap.Width, bitmap.Height);
            // Convert the image to floating point.
            var pixel = 0;

            try
            {
                //var startTime = SystemClock.UptimeMillis();
                for (var i = 0; i < ImageWidth; ++i)
                {
                    for (var j = 0; j < ImageHeight; ++j)
                    {
                        //System.Console.WriteLine($"{i},{j} pixel={pixel}");
                        //if (pixel == 19200)
                        //{
                        //    //Debugging - crashes here
                        //}

                        if (!imgData.HasRemaining)
                        {
                            //Debug - is about to crash
                        }
                        var val = intValues[pixel++];
                        AddPixelValue(val, imgData, NormalizeMobileNetInputs);
                        //System.Console.WriteLine($"Success pixel={pixel}");
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }

            //var endTime = SystemClock.UptimeMillis();
            //LOGGER.v("Timecost to put values into ByteBuffer: " + (endTime - startTime));

            return imgData;
        }

        /** MobileNet requires additional normalization of the used input. */
        private const float IMAGE_MEAN = 127.5f;
        private const float IMAGE_STD = 127.5f;

        //NOTE: This is virtual in sample, and overridden for different classifiers
        protected void AddPixelValue(int pixelValue, ByteBuffer imgData, bool normalizeInputs = false)
        {
            if (normalizeInputs)
            {
                imgData.PutFloat((((pixelValue >> 16) & 0xFF) - IMAGE_MEAN) / IMAGE_STD);
                imgData.PutFloat((((pixelValue >> 8) & 0xFF) - IMAGE_MEAN) / IMAGE_STD);
                imgData.PutFloat(((pixelValue & 0xFF) - IMAGE_MEAN) / IMAGE_STD);
            }
            else
            {
                imgData.Put((sbyte)((pixelValue >> 16) & 0xFF));
                imgData.Put((sbyte)((pixelValue >> 8) & 0xFF));
                imgData.Put((sbyte)(pixelValue & 0xFF));
            }
        }

        private async Task<IList<string>> LoadLabels(string fileName)
        {
            using (var stream = _assetLoader.GetStream(fileName))
            using (var sr = new StreamReader(stream))
            {
                var content = await sr.ReadToEndAsync();
                var labels = content.Split(
                    new[] { "\r\n" },
                    StringSplitOptions.RemoveEmptyEntries);

                //for (var i = 0; i < labels.Length; i++)
                //{
                //    labels[i] = labels[i].TrimEnd('\r');
                //}

                return labels;
            }
        }

        //private async Task<byte[]> LoadModel(string fileName)
        //{
        //    var buffer = new byte[16 * 1024];
        //    using (var stream = _assetLoader.GetStream(fileName))
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            int read;
        //            while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        //            {
        //                ms.Write(buffer, 0, read);
        //            }
        //            return ms.ToArray();
        //        }
        //    }
        //}

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
