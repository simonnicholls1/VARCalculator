using Deedle;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VARCalculator.Model;

namespace VARCalculator.Services
{

    public delegate void ProgressUpdate(int value);

    class VARCalculatorService
    {
        
        public Double calculateVAR(double portfolioValue, double volatility, double confidenceLevel)
        {
            //find z score and then find VAR
            MathNet.Numerics.Distributions.Normal result = new MathNet.Numerics.Distributions.Normal();
            double zScore = result.InverseCumulativeDistribution(confidenceLevel);
            double VAR = portfolioValue * zScore * volatility;

            return VAR;
        }

        public double calculatePortfolioVol(ConcurrentDictionary<string, InstrumentModel> instruments, Dictionary<string, double> portfolioWeights, CancellationToken cancellationToken, IProgress<int> progress, IProgress<string> status)
        {
            double[] portfolioWeightArray = new double[portfolioWeights.Count];
            portfolioWeights.Values.CopyTo(portfolioWeightArray, 0);

            Vector<double> portfolioWeightsVector = Vector<double>.Build.Dense(portfolioWeightArray);

            //Build covar matrix
            Matrix<double> coVarianceMatrix = createCoVarMatrix(instruments, portfolioWeights, cancellationToken, progress, status);

            //Calculate variance of portfolio
            Vector<double> portfolioVariance = portfolioWeightsVector.ToRowMatrix() * coVarianceMatrix * portfolioWeightsVector;

            double volatility = Math.Sqrt(portfolioVariance[0]);

            return volatility;

        }

        private Matrix<double> createCoVarMatrix(ConcurrentDictionary<string, InstrumentModel> instruments, Dictionary<string, double> portfolioWeights, CancellationToken cancellationToken, IProgress<int> progress, IProgress<string> status)
        {

            int noOfInstruments = instruments.Keys.Count();
            //Number of iterations of instruments to be processed for a one percent change in progress
            double onePercentProcess = noOfInstruments / 80.00;

            Matrix<double> coVARMatrix = Matrix<double>.Build.Dense(noOfInstruments, noOfInstruments);

            //Driven off portfolio weights as it has the keys in the correct order.
            //Concurrent dictionary of instruments may not be in correct order as threads
            //Will add whenever they are finished.
            int progressCounter = 0;
            int i = 0;
            int count = 0;
            var watchMain = System.Diagnostics.Stopwatch.StartNew();

            //For each looping over rows 
            foreach(InstrumentModel instrument in instruments.Values)
            {
                status.Report("Covar matrix inst " + (i +1).ToString() + " of " + noOfInstruments.ToString());  
                //For each parallel over each column for that row
                Parallel.ForEach(instruments.Keys, (instrumentNest, pls, index) =>
                {
                    int n = Convert.ToInt32(index);
                    //Matrix symetrical so don't need to fill if the row number is great than the column number
                    //as already should have been filled
                    if (i > n)
                    {

                    }
                    else
                    {
                        try
                        {
                            double coVar = CalculateCoVariance(instrument.instrumentReturnsArray, instrument.mean, instruments[instrumentNest].instrumentReturnsArray, instruments[instrumentNest].mean);
                            lock (coVARMatrix)
                            {
                                if (i == n)
                                {

                                    coVARMatrix[i, n] = coVar;
                                }
                                else
                                {
                                    coVARMatrix[i, n] = coVar;
                                    coVARMatrix[n, i] = coVar;
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            throw new ServiceException(ServiceException.UNKNOWN_ERROR, "Unknown error when calculating covariance");
                        }
                    }
                }
                );

                i++;
                count++;

                //Check if we need to add one percent to progress number
                if(count >= onePercentProcess)
                {
                    progressCounter++;
                    progress.Report(20 + progressCounter);
                    count = 0;
                }

                //check if user has cancelled if so then abort loop
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            };
            return coVARMatrix;
        }

        private static double CalculateCoVariance(double[] firstReturns, double firstMean, double[] secondReturns, double secondMean)
        {
            double covariance;
            ConcurrentBag<double> topSums = new ConcurrentBag<double>();
            int numberOfElements = 0;

            if (firstReturns.Count() != secondReturns.Count())
            {
                throw new ServiceException(ServiceException.DIMENSIONS_ERROR, "Dimensions do not match for covariance calculation");
            }
            else
            {
                numberOfElements = firstReturns.Count();
            }

            Parallel.ForEach(firstReturns, (instrument, pls, index) =>
            {   
                topSums.Add(((firstReturns[index] - firstMean) * (secondReturns[index] - secondMean)));
            });

            double topSum = topSums.Sum();
            covariance = topSum / numberOfElements;
            return covariance;
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

    }
}
