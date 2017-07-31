using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VARCalculator.Model
{
    public class ConfLevelModel
    {
        public double ConfLevel { get; set; }

        public ConfLevelModel(double confLevel)
        {
            this.ConfLevel = confLevel;
        }
    }
}