using Deedle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VARCalculator.Services
{
    class VARCalculatorService
    {
        
        public Double calculateVAR(string instrumentID, double amountOwned, double mean, double volatility, double confidenceLevel)
        {
            MathNet.Numerics.Distributions.Normal result = new MathNet.Numerics.Distributions.Normal(mean, volatility);
            double alpha = result.InverseCumulativeDistribution(1 - confidenceLevel);
            double VAR = amountOwned - (amountOwned * (alpha));

            return VAR;
        }

        public double calculateMeanOld(double[] returns)
        {
            double meanReturn;

            meanReturn = returns.Average();
            return meanReturn;

        }

        public double calculateVolOld(double[] returns, double? mean)
        {
            double volatility;
            double meanReturns;

            if(mean == null)
            {
                meanReturns = returns.Average();
            }
            else
            {
               meanReturns = mean.GetValueOrDefault();
            }

            double sumOfDeviation = 0;
            foreach(double returnValue in returns)
            {
                sumOfDeviation += returnValue * returnValue;
            }
            
            double sumOfDeviationAverage;

            sumOfDeviationAverage = sumOfDeviation/(returns.Count() -1);

            volatility = Math.Sqrt(sumOfDeviationAverage - (meanReturns * meanReturns));

            return volatility;
        }


        public Double calculateMean(Series<DateTime, double> timeSeriesReturns)
        {

            Double mean = timeSeriesReturns.Mean();
            return mean;
        }

        public Double calculateVol(Series<DateTime, double> timeSeriesReturns)
        {
            Double vol = timeSeriesReturns.StdDev();
            return vol;
        }
    }
}
