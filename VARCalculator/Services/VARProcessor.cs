using Deedle;
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

        ConcurrentDictionary<string, InstrumentModel> instrumentDictionary;
        Dictionary<string, double> portfolioWeights;

        public VARProcessor()
        {
            varCalculator = new VARCalculatorService();
            returnsDAO = new ReturnsDAO();
            returnsDAO.LoadInstrumentReturnsMemory();

            instrumentDictionary = new ConcurrentDictionary<string, InstrumentModel>();

            //TEST DATA
            portfolioWeights = new Dictionary<string, double>();
            double weight = (1 / 3000.00);
            for(int i =1; i < 3000; i++)
            {
                portfolioWeights.Add(i.ToString() , weight);
            }
        }

        public VAROutputModel ProcessVAR(double portfolioValue, double confidenceLevel, DateTime startDate, DateTime endDate)
        {
            

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
                currentInstrument.VAR = varCalculator.calculateVAR(instrument.Key, amountOwned, currentInstrument.mean, currentInstrument.volatility, confidenceLevel);

                //Add stock to concurrent dcitionary of instruments
                instrumentDictionary.TryAdd(instrument.Key, currentInstrument);                
            });

            //Now work out total var of portfolio
            double totalVAR = 0;

            foreach(string instrumentID in instrumentDictionary.Keys)
            {
                totalVAR += instrumentDictionary[instrumentID].VAR;
            }

            VAROutputModel VAROutput = new VAROutputModel(instrumentDictionary.Keys.Count, startDate, endDate, DateTime.Now, totalVAR);

            return VAROutput;
        }
              
    }
}
