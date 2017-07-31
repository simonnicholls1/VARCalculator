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
        Frame<DateTime, string> instrumentReturnsHistory;

        public ReturnsDAO()
        {
           

        }


        public void LoadInstrumentReturnsMemory()
        {
            
            string fileName = ConfigurationManager.AppSettings["InstrumentDataFileName"].ToString();
            string fullFilePath = Environment.CurrentDirectory + "/" + fileName;
            try
            {
                Frame<int, string> instrumentReturns = Frame.ReadCsv(fullFilePath);
                instrumentReturnsHistory = instrumentReturns.IndexRows<DateTime>("Date").SortRowsByKey();
            }
            catch (Exception ex)
            {
                throw new DAOException(DAOException.UNKNOWN_ERROR, "Unknown error occurred when opening file", ex);
            }
          
        }

        public Series<DateTime, double> getInstrumentReturns(string instrumentID, DateTime startDate, DateTime endDate)
        {
            Series<DateTime, double> instrumentReturnsColumn = instrumentReturnsHistory.GetColumn<double>("1");
            var instrumentReturns = instrumentReturnsColumn.Between(startDate, endDate);
            return instrumentReturns;
        }
    }
}
