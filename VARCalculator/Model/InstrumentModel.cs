using Deedle;
using System.ComponentModel;


namespace FuzzyLogicSearch.Model
{
    public class InstrumentModel
    {
        public string instrumentID { get; set; }
        public Frame<int, string> instrumentReturns { get; set; }
        public double portfolioWeight { get; set; }
        public double mean { get; set; }
        public double volatility { get; set; }
        public double VAR { get; set; }

        public InstrumentModel(string instrumentID)
        {
            this.instrumentID = instrumentID;
        }

        public InstrumentModel(string instrumentID, double portfolioWeight)
        {
            this.instrumentID = instrumentID;
            this.portfolioWeight = portfolioWeight;
        }

    }
}
