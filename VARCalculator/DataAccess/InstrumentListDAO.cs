using FuzzyLogicSearch.Interfaces;
using FuzzyLogicSearch.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;

namespace FuzzyLogicSearch.DataAccess
{
    class InstrumentListDAO : IReportDAO<InstrumentModel>
    {
        
        private OdbcConnection connection;

        public InstrumentListDAO()
        {
            try
            {
                string filePath = Path.Combine(Environment.CurrentDirectory,ConfigurationManager.AppSettings["InstrumentDataPath"].ToString());
                connection = new OdbcConnection("Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + filePath + ";Extensions=asc,csv,tab,txt");
            }
            catch (OdbcException ex)
            {
                throw new DAOException(DAOException.FILE_NA, "Could not open file, check it exists or is not already in use", ex);
            }
            catch (Exception ex)
            {
                throw new DAOException(DAOException.UNKNOWN_ERROR, "Could not open file, unknown error please check stack trace", ex);
            }

        }

        public List<InstrumentModel> GetData()
        {
            List<InstrumentModel> instrumentData = new List<InstrumentModel>();
            OdbcDataReader dataReader = null;
            OdbcCommand command = null;

            //Make file name from date string
            string fileName = ConfigurationManager.AppSettings["InstrumentDataFileName"].ToString();

            try
            {
                command = new OdbcCommand("SELECT * FROM [" + fileName + "]", connection);
                connection.Open();
                dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);

                while (dataReader.Read())
                {
                    InstrumentModel instrument = new InstrumentModel();

                    if (!(dataReader[0].ToString() == string.Empty))
                    {

                        instrument.Identifier = dataReader[0].ToString();
                        instrument.InstrumentName = dataReader[1].ToString().Replace(" Prices, Dividends, Splits and Trading Volume", "");

                        instrumentData.Add(instrument);
                    }
                    else
                    {
                        instrumentData.Add(instrument);
                    }
                }
            }
            catch (OdbcException ex)
            {
                throw new DAOException(DAOException.FILE_NA, "Could not open file, check it exists or is not already in use", ex);
            }
            catch (FormatException ex)
            {
                throw new DAOException(DAOException.FILE_PARSE_ERROR, "Could not parse the data correctly please check output and try again", ex);
            }
            catch  (InvalidCastException ex)
            {
                throw new DAOException(DAOException.FILE_PARSE_ERROR, "Error when casting data please check output and try again", ex);
            }
            catch (Exception ex)
            {
                throw new DAOException(DAOException.UNKNOWN_ERROR, "Unknown error occurred when opening file", ex);
            }
            finally
            {
                if(dataReader != null)
                {
                    dataReader.Close();
                }
                if(command != null)
                {
                    command.Dispose();
                }
               
                connection.Close();
            }

            return instrumentData;
        }

        public List<InstrumentModel> GetFilteredData(string filter)
        {
            List<InstrumentModel> instrumentData = new List<InstrumentModel>();
            OdbcDataReader dataReader = null;
            OdbcCommand command = null;

            //Make file name from date string
            string fileName = ConfigurationManager.AppSettings["InstrumentDataFileName"].ToString();

            try
            {
                command = new OdbcCommand("SELECT identifier, instrument_name FROM " + fileName + " WHERE instrument_name like " + filter + "%", connection);
                connection.Open();
                dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);

                while (dataReader.Read())
                {
                    InstrumentModel instrument = new InstrumentModel();

                    if (!(dataReader[0].ToString() == string.Empty))
                    {

                        instrument.Identifier = dataReader[0].ToString();
                        instrument.InstrumentName = dataReader[1].ToString();

                        instrumentData.Add(instrument);
                    }
                    else
                    {
                        instrumentData.Add(instrument);
                    }
                }
            }
            catch (OdbcException ex)
            {
                throw new DAOException(DAOException.FILE_NA, "Could not open file, check it exists or is not already in use", ex);
            }
            catch (FormatException ex)
            {
                throw new DAOException(DAOException.FILE_PARSE_ERROR, "Could not parse the data correctly please check output and try again", ex);
            }
            catch (InvalidCastException ex)
            {
                throw new DAOException(DAOException.FILE_PARSE_ERROR, "Error when casting data please check output and try again", ex);
            }
            catch (Exception ex)
            {
                throw new DAOException(DAOException.UNKNOWN_ERROR, "Unknown error occurred when opening file", ex);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                if (command != null)
                {
                    command.Dispose();
                }

                connection.Close();
            }

            return instrumentData;
        }
    }
}
