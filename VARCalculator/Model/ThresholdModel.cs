using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogicSearch.Model
{
    public class ThresholdModel
    {
        public double Threshold { get; set; }

        public ThresholdModel(double threshold)
        {
            this.Threshold = threshold;
        }
    }
}
