using System;
using System.ComponentModel;
namespace VARCalculator.Model
{
    public class VAROutputModel
    {
        public double InstrumentCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RunDate { get; set; }
        public double PortfolioValue { get; set; }
        public double ConfLevel { get; set; }
        public double VAR { get; set; }

        public VAROutputModel(double instrumentCount, DateTime startDate, DateTime endDate, DateTime runDate, double portfolioValue, double confLevel, double var)
        {
            
            this.InstrumentCount = instrumentCount;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.RunDate = runDate;
            this.PortfolioValue = portfolioValue;
            this.ConfLevel = confLevel;
            this.VAR = var;
        }

    }
}
