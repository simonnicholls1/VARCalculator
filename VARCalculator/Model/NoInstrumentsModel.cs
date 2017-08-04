using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VARCalculator.Model
{
    public class NoInstrumentsModel
    {
        public int NoInstruments { get; set; }

        public NoInstrumentsModel(int noInstruments)
        {
            this.NoInstruments = noInstruments;
        }
    }
}