using Deedle;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VARCalculator.DataAccess;
using VARCalculator.Model;


namespace VARCalculator.Services
{
    public class VARProcessor
    {
        //Services
        ReturnsDAO returnsDAO;
        VARCalculatorService varCalculator;
        Dictionary<string, double> portfolioWeights;

        public VARProcessor()
        {
            varCalculator = new VARCalculatorService();
            returnsDAO = new ReturnsDAO();
            returnsDAO.LoadInstrumentPricesMemory();

            //TEST WEIGHT DATA
            double noOfStocks = 3000;
            portfolioWeights = new Dictionary<string, double>();
            double weight = (1 / noOfStocks);
            for (int i = 0; i < noOfStocks; i++)
            {
                portfolioWeights.Add(i.ToString() , weight);
            }
        }

        public VAROutputModel ProcessVAR(double portfolioValue, double confidenceLevel, DateTime startDate, DateTime endDate)
        {

            ConcurrentDictionary<string, InstrumentModel> instrumentDictionary = new ConcurrentDictionary<string, InstrumentModel>();

            Parallel.ForEach(portfolioWeights, instrument =>
            {

                //Create a new stock object to store details on the stock, in future might want
                //to look at other attributes like largets exposures, mean and var separately
                InstrumentModel currentInstrument = new InstrumentModel(instrument.Key, instrument.Value);

                //Get returns for specific instrument, should think about not storing this in 
                //object if already have returns in memory
                currentInstrument.instrumentReturns = returnsDAO.getInstrumentReturns(instrument.Key, startDate, endDate);

                //Calculate mean and vol for VAR and store in stock object
                //TODO convert dataframe to array of returns
                currentInstrument.mean = varCalculator.calculateMean(currentInstrument.instrumentReturns);
                currentInstrument.volatility = varCalculator.calculateVol(currentInstrument.instrumentReturns);

                //Need how much of the stock we own to calculate VAR
                double amountOwned = instrument.Value * portfolioValue;

                //Now calculate VAR
                //currentInstrument.VAR = varCalculator.calculateVAR(instrument.Key, amountOwned, currentInstrument.mean, currentInstrument.volatility, confidenceLevel);

                //Add stock to concurrent dcitionary of instruments
                instrumentDictionary.TryAdd(instrument.Key, currentInstrument);                
            });

            //Produce covariance matrix
            double portfolioVol = varCalculator.calculatePortfolioVol(instrumentDictionary, portfolioWeights);

            //Now work out total var of portfolio
            double totalVAR = varCalculator.calculateVAR(portfolioValue, portfolioVol, confidenceLevel);

            VAROutputModel VAROutput = new VAROutputModel(instrumentDictionary.Keys.Count, startDate, endDate, DateTime.Now, portfolioValue, confidenceLevel * 100, Math.Round(totalVAR, 2));

            return VAROutput;
        }
              
    }
}
