using Deedle;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VARCalculator.Model;

namespace VARCalculator.Services
{
    class VARCalculatorService
    {
        
        public Double calculateVAR(double portfolioValue, double volatility, double confidenceLevel)
        {
            MathNet.Numerics.Distributions.Normal result = new MathNet.Numerics.Distributions.Normal();
            double zScore = result.InverseCumulativeDistribution(confidenceLevel);
            double VAR = portfolioValue * zScore * volatility;

            return VAR;
        }

        public double calculatePortfolioVol(ConcurrentDictionary<string, InstrumentModel> instruments, Dictionary<string, double> portfolioWeights)
        {
            double[] portfolioWeightArray = new double[portfolioWeights.Count];
            portfolioWeights.Values.CopyTo(portfolioWeightArray, 0);

            Vector<double> portfolioWeightsVector = Vector<double>.Build.Dense(portfolioWeightArray);
            Matrix<double> coVarianceMatrix = createCoVarMatrix(instruments, portfolioWeights);

            Vector<double> portfolioVariance = portfolioWeightsVector.ToRowMatrix() * coVarianceMatrix * portfolioWeightsVector;

            double volatility = Math.Sqrt(portfolioVariance[0]);

            return volatility;

        }

        private Matrix<double> createCoVarMatrix(ConcurrentDictionary<string, InstrumentModel> instruments, Dictionary<string, double> portfolioWeights)
        {

            int noOfInstruments = instruments.Keys.Count();
            Matrix<double> coVARMatrix = Matrix<double>.Build.Dense(noOfInstruments, noOfInstruments);

            //Driven off portfolio weights as it has the keys in the correct order.
            //Concurrent dictionary of instruments may not be in correct order as threads
            //Will add whenever they are finished.

            int i = 0;
            //Parallel.ForEach(instruments.Keys, instrumentID =>
            foreach(string instrumentID in portfolioWeights.Keys)
            {
                int n = 0;
                //Parallel.ForEach(instruments.Keys, instrumentIDNest =>
                foreach (string instrumentIDNest in portfolioWeights.Keys)
                {
                    InstrumentModel instrumentOne = instruments[instrumentID];
                    InstrumentModel instrumentTwo = instruments[instrumentIDNest];
                    coVARMatrix[i, n] = CalculateCoVariance(instrumentOne.instrumentReturns, instrumentOne.mean, instrumentTwo.instrumentReturns, instrumentTwo.mean);
                    n++;
                }
                //);

                i++;
            }
            
            //);

            return coVARMatrix;
        }

        private double CalculateCoVariance(Series<DateTime, double> firstReturns, double firstMean,  Series<DateTime, double> secondReturns, double secondMean)
        {
            double covariance;
            ConcurrentBag<double> topSums = new ConcurrentBag<double>();
            int numberOfElements = 0;

            if (firstReturns.KeyCount != secondReturns.KeyCount)
            {
                //TODO: Throw error
            }
            else
            {
                numberOfElements = firstReturns.KeyCount;
            }

            Parallel.ForEach(firstReturns.Keys, key => {
                topSums.Add(((firstReturns.Get(key) - firstMean) * (secondReturns.Get(key) - secondMean)));
            });

            double topSum = topSums.Sum();
            covariance = topSum / numberOfElements;
            return covariance;
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
