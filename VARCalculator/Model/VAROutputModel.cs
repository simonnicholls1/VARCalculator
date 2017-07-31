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
        public double VAR { get; set; }

        public VAROutputModel(double instrumentCount, DateTime startDate, DateTime endDate, DateTime runDate, double var)
        {
            // TODO: Complete member initialization
            this.InstrumentCount = instrumentCount;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.RunDate = runDate;
            this.VAR = var;
        }

    }
}
