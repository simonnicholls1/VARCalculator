using Deedle;
using FuzzyLogicSearch.DataAccess;
using FuzzyLogicSearch.Model;
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

        public double calculateMean(double[] returns)
        {
            double meanReturn;

            meanReturn = returns.Average();
            return meanReturn;

        }

        public double calculateVol(double[] returns, double? mean)
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

            double sumOfDeviation;
            foreach(double returnValue in returns)
            {
                sumOfDeviation += returnValue * returnValue;
            }
            
            double sumOfDeviationAverage;

            sumOfDeviationAverage = sumOfDeviation/(returns.Count() -1);

            volatility = Math.Sqrt(sumOfDeviationAverage - (meanReturns * meanReturns));

            return volatility;
        }


        public Double calculateMeanOld(Frame<int, string> timeSeriesReturns, string returnsKey, string dateKey)
        {
            Frame<DateTime, String> frameDate = timeSeriesReturns.IndexRows<DateTime>(dateKey).SortRowsByKey();
            Series<DateTime, Double> instOpen = frameDate.GetColumn<double>(returnsKey);
            Double mean = instOpen.Mean();
            return mean;
        }

        public Double calculateVolOld(Frame<int, string> timeSeriesReturns, string returnsKey, string dateKey)
        {
            Frame<DateTime, String> frameDate = timeSeriesReturns.IndexRows<DateTime>(dateKey).SortRowsByKey();
            Series<DateTime, Double> instOpen = frameDate.GetColumn<double>(returnsKey);
            Double vol = instOpen.StdDev();
            return vol;
        }
    }
}
