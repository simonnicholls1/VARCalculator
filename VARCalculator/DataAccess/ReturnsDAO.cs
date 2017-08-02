using Deedle;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using VARCalculator.Model;

namespace VARCalculator.DataAccess
{
    class ReturnsDAO
    {
        Frame<DateTime, string> instrumentPricesHistory;

        public ReturnsDAO()
        {
           

        }


        public void LoadInstrumentPricesMemory()
        {
            
            string fileName = ConfigurationManager.AppSettings["InstrumentDataFileName"].ToString();
            string fullFilePath = Environment.CurrentDirectory + "/" + fileName;
            try
            {
                Frame<int, string> instrumentPrices = Frame.ReadCsv(fullFilePath);
                instrumentPricesHistory = instrumentPrices.IndexRows<DateTime>("Date").SortRowsByKey();
            
            }
            catch (Exception ex)
            {
                throw new DAOException(DAOException.UNKNOWN_ERROR, "Unknown error occurred when opening file", ex);
            }
          
        }

        public Series<DateTime, double> getInstrumentReturns(string instrumentID, DateTime startDate, DateTime endDate)
        {
            Series<DateTime, double> instrumentPricesColumn = instrumentPricesHistory.GetColumn<double>(instrumentID);
            Series<DateTime, double> instrumentPrices = instrumentPricesColumn.Between(startDate, endDate);
            Series<DateTime, double> instrumentReturns = instrumentPrices.Diff(1) / instrumentPrices;

            return instrumentReturns.DropMissing();
        }
    }
}
