
using System;

namespace Wild.Pokenizer.Core.Models
{
    public class PredictionResult
    {
        public bool Success { get; set; }

        public string Answer { get; set; }

        public DateTime PredictionTime { get; set; }
    }
}
