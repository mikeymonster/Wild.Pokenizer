using System.Drawing;
using System.Text;

namespace Wild.Pokenizer.Core.Models
{
    /** An immutable result returned by a Classifier describing what was recognized. */
    public class Recognition
    {
        /// <summary>
        /// A unique identifier for what has been recognized.
        /// Specific to the class, not the instance of the object.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Display name for the recognition.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A sortable score for how good the recognition is relative to others.
        /// Higher should be better.
        /// </summary>
        public float Confidence { get; set; }

        /// <summary>
        /// Optional location within the source image for the location of the recognized object.
        /// </summary>
        public RectangleF? Location { get; set; }

        public Recognition()
        { }

        public Recognition(string id, string title, float confidence, RectangleF? location)
        {
            Id = id;
            Title = title;
            Confidence = confidence;
            Location = location;
        }

        public override string ToString()
        {
            var resultString = new StringBuilder();
            if (Id != null)
            {
                resultString.Append($"[{Id}]");
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                if (resultString.Length > 0)
                    resultString.Append(" ");
                resultString.Append(Title);
            }

            if (resultString.Length > 0)
                resultString.Append(" ");
            resultString.Append($"confidence ({Confidence * 100.0f:0.##}");

            if (Location != null && Location != RectangleF.Empty)
            {
                if (resultString.Length > 0)
                    resultString.Append(" ");
                resultString.Append($"  {Location}");
            }

            return resultString.ToString().Trim();
        }
    }
}
