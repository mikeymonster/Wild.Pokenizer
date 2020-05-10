
using System.Drawing;

namespace Wild.Pokenizer.Core.Models
{
    public class Prediction
    {
        public string Label { get; set; }
        public float Probability { get; set; }

        public RectangleF? Location { get; set; }

        public Prediction() 
        { }

        public Prediction(string label, float probability)
        {
            Label = label;
            Probability = probability;
        }
    }
}
