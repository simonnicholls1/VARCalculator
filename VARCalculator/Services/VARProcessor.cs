using Deedle;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            //returnsDAO.LoadInstrumentPricesMemory();

        }

        public VAROutputModel ProcessVAR(double portfolioValue, double confidenceLevel, int noOfInstruments,  DateTime startDate, DateTime endDate, CancellationToken cancellationToken, IProgress<int> progress, IProgress<string> status)
        {
            //Equally weighted
            portfolioWeights = new Dictionary<string, double>();
            double weight = (1 /(double) noOfInstruments);
            for (int i = 0; i < noOfInstruments; i++)
            {
                portfolioWeights.Add(i.ToString(), weight);
            }

            //Load returns into memory if not already done
            if (returnsDAO.instrumentPricesHistory == null)
            {
                status.Report("Loading prices into memory please wait...");
                try
                {
                    returnsDAO.LoadInstrumentPricesMemory();
                }
                catch(DAOException ex)
                {
                    if(ex.GetErrorCode() == DAOException.FILE_NA)
                    {
                        throw new ServiceException(ServiceException.DATA_ACCESS_ERROR, "Could not find file for stock prices please check settings");
                    }
                    else
                    {
                        throw new ServiceException(ServiceException.UNKNOWN_ERROR, "Unkown error when loading prices please try again");
                    }
                }
                catch(Exception ex)
                {
                    throw new ServiceException(ServiceException.UNKNOWN_ERROR, "Unkown error when loading prices please try again");
                }
            }

            ConcurrentDictionary<string, InstrumentModel> instrumentDictionary = new ConcurrentDictionary<string, InstrumentModel>();

            //For each instrument get the returns for the selected dates then calculate a mean and portfolio value
            status.Report("Collecting returns and calculating means");
            Parallel.ForEach(portfolioWeights, instrument =>
            {
                //Check if request has been cancelled
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                try
                {

                    //Create a new stock object to store details on the stock, in future might want
                    //to look at other attributes like largets exposures, mean and var separately
                    InstrumentModel currentInstrument = new InstrumentModel();
                    currentInstrument.instrumentID = instrument.Key;
                    currentInstrument.portfolioWeight = instrument.Value;

                    //Get returns for specific instrument, should think about not storing this in 
                    //object if already have returns in memory
                    currentInstrument.instrumentReturnsArray = returnsDAO.getInstrumentReturns(instrument.Key, startDate, endDate);

                    //Calculate mean for VAR and store in stock object
                    currentInstrument.mean = varCalculator.calculateMean(currentInstrument.instrumentReturnsArray);
                    currentInstrument.volatility = varCalculator.calculateVol(currentInstrument.instrumentReturnsArray, currentInstrument.mean);

                    //Need how much of the stock we own to calculate VAR
                    double amountOwned = instrument.Value * portfolioValue;

                    instrumentDictionary.TryAdd(instrument.Key, currentInstrument);
                }
                catch(DAOException ex)
                {
                    throw new ServiceException(ServiceException.UNKNOWN_ERROR, "Error occurred when processing instrument returns and mean");
                }
                catch(ServiceException ex)
                {
                    throw new ServiceException(ServiceException.UNKNOWN_ERROR, "Error occurred when processing instrument returns and mean");
                }
                catch(Exception ex)
                {
                    throw new ServiceException(ServiceException.UNKNOWN_ERROR, "Error occurred when processing instrument returns and mean");
                }
            });

            progress.Report(10);

            //Check if it has been cancelled and if so set VAR to 0
            VAROutputModel VAROutput;
            if (cancellationToken.IsCancellationRequested)
            {
                VAROutput = new VAROutputModel(instrumentDictionary.Keys.Count, startDate, endDate, DateTime.Now, portfolioValue, confidenceLevel * 100, 0);
                progress.Report(100);
                status.Report("");
            }
            else
            {
                //Produce covariance matrix
                double portfolioVol = varCalculator.calculatePortfolioVol(instrumentDictionary, portfolioWeights, cancellationToken, progress, status);

                //Check again if task has been cancelled 
                if (cancellationToken.IsCancellationRequested)
                {
                    VAROutput = new VAROutputModel(instrumentDictionary.Keys.Count, startDate, endDate, DateTime.Now, portfolioValue, confidenceLevel * 100, 0);
                    progress.Report(100);
                    status.Report("");
                }
                else
                {

                    //Now work out total var of portfolio
                    status.Report("Calculating VAR");
                    double totalVAR = varCalculator.calculateVAR(portfolioValue, portfolioVol, confidenceLevel);
                    progress.Report(85);
                    VAROutput = new VAROutputModel(instrumentDictionary.Keys.Count, startDate, endDate, DateTime.Now, portfolioValue, confidenceLevel * 100, Math.Round(totalVAR, 2));
                    progress.Report(100);
                    status.Report("");
                }
            }
            return VAROutput;
        }
              
    }
}
