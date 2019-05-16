using System;
using System.Collections.Generic;
using System.IO;

using Android.App;
using Android.Content.Res;
using Android.Graphics;

namespace Wild.Pokenizer.Xamarin.Droid.Predictors
{
    // From https://github.com/lobrien/TensorFlow.Xamarin.Android/blob/master/TensorFlowXamarin/Hello_TF/InceptionClassifier.cs

    public struct Recognition
    {
        public string Label;
        public float Confidence;

        public override string ToString()
        {
            return string.Format($"[Label: {Label}, Confidence: {Confidence}]");
        }
    }

    //Dummy class to make this compile
    public class TensorFlowInferenceInterface
    {
        private AssetManager _assets;
        private string _v;

        public TensorFlowInferenceInterface(AssetManager assets, string v)
        {
            this._assets = assets;
            this._v = v;
        }

        public TfGraphOperation GraphOperation(string outputName)
        {
            throw new NotImplementedException();
        }

        public void Feed(string inputName, float[] normalizedPixelComponents, int i, int inputSize, int inputSize1)
        {
            throw new NotImplementedException();
        }

        public void Feed(string inputName, float[] normalizedPixelComponents, int i, int inputSize, int inputSize1, int i1)
        {
            throw new NotImplementedException();
        }

        public void Run(string[] strings)
        {
            throw new NotImplementedException();
        }

        public void Fetch(string outputName, float[] outputs)
        {
            throw new NotImplementedException();
        }
    }

    public class TfGraphOperation
    {
        public TfOutput Output(int i)
        {
            throw new NotImplementedException();
        }
    }

    public class TfOutput
    {
        public TfShape Shape()
        {
            throw new NotImplementedException();
        }
    }

    public class TfShape
    {
        public long Size(int i)
        {
            throw new NotImplementedException();
        }
    }

    public class InceptionClassifier : Activity
    {
        const int INPUT_SIZE = 224;
        const int IMAGE_MEAN = 117;
        const float IMAGE_STD = 1;
        const string INPUT_NAME = "input";
        const string OUTPUT_NAME = "output";

        const string MODEL_FILE = "file:///android_asset/tensorflow_inception_graph.pb";
        const string LABEL_FILE = "imagenet_comp_graph_label_strings.txt";

        readonly List<string> _labels = new List<string>();
        readonly TensorFlowInferenceInterface _inferenceInterface;

        long OutputSize()
        {
            return _inferenceInterface.GraphOperation(OUTPUT_NAME).Output(0).Shape().Size(1);
        }

        public InceptionClassifier(AssetManager assetManager)
        {
            _labels.AddRange(ReadLabelsIntoMemory(assetManager, LABEL_FILE));
            _inferenceInterface = new TensorFlowInferenceInterface(assetManager, MODEL_FILE);
            var operation = _inferenceInterface.GraphOperation(OUTPUT_NAME);
            var numClasses = operation.Output(0).Shape().Size(1);
            //TODO: Confirm in Android, because my results are 1001 labels, 1008 classes
            Console.WriteLine($"Read {_labels.Count} labels, output layer size is {numClasses}");
            var bmp = BitmapCreate(assetManager, "husky.png");
            var results = Recognize(bmp);
        }

        static Bitmap BitmapCreate(AssetManager assetManager, string assetName)
        {
            using (var str = assetManager.Open(assetName))
            {
                return BitmapFactory.DecodeStream(str);
            }
        }

        IEnumerable<String> ReadLabelsIntoMemory(AssetManager assets, string fileName)
        {
            using (StreamReader sr = new StreamReader(assets.Open(fileName)))
            {
                var content = sr.ReadToEnd();
                return content.Split(new[] { '\n' });
            }
        }

        public IEnumerable<Recognition> Recognize(Bitmap bitmap)
        {
            var argbPixelArray = new int[INPUT_SIZE * INPUT_SIZE];
            bitmap.GetPixels(argbPixelArray, 0, bitmap.Width, 0, 0, bitmap.Width, bitmap.Height);

            var normalizedPixelComponents = new float[argbPixelArray.Length * 3];
            for (int i = 0; i < argbPixelArray.Length; i++)
            {
                var argb = argbPixelArray[i];
                normalizedPixelComponents[i * 3 + 0] = (((argb >> 16) & 0xFF) - IMAGE_MEAN) / IMAGE_STD;
                normalizedPixelComponents[i * 3 + 1] = (((argb >> 8) & 0xFF) - IMAGE_MEAN) / IMAGE_STD;
                normalizedPixelComponents[i * 3 + 2] = (((argb) & 0xFF) - IMAGE_MEAN) / IMAGE_STD;
            }

            // Copy the input data into TF
            _inferenceInterface.Feed(INPUT_NAME, normalizedPixelComponents, 1, INPUT_SIZE, INPUT_SIZE, 3);

            // Run the inference
            _inferenceInterface.Run(new[] { OUTPUT_NAME });

            // Grab the output data 
            var outputs = new float[OutputSize()];
            _inferenceInterface.Fetch(OUTPUT_NAME, outputs);
            var results = new Recognition[_labels.Count];
            for (int i = 0; i < _labels.Count; i++)
            {
                results[i] = new Recognition { Confidence = outputs[i], Label = _labels[i] };
            }
            // Sort high-to-low via confidence 
            Array.Sort(results, (x, y) => y.Confidence.CompareTo(x.Confidence));
            Console.WriteLine(results[0]);
            return results;
        }
    }
}