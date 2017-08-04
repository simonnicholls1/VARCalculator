using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VARCalculator.Model;
using VARCalculator.Services;

namespace VARCalculator.ViewModel
{
    public class VARCalculatorViewModel : ObservableObject, INotifyPropertyChanged
    {
        private VARProcessor VARService;
        
        private double selectedConfLevel;
        private string selectedConfLevelIndex;
        private int selectedNoInstruments;
        private ICommand varCalculateClick;
        private ICommand clearGridClick;
        private ICommand cancelVARClick;
        private DateTime startDate;
        private DateTime endDate;
        private double portfolioValue;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private int progressNumber;
        private string calculateLabel;
        private bool isRunning;
        private string selectedNoInstrumentsIndex;

        private ObservableCollection<ConfLevelModel> confLevelList = new ObservableCollection<ConfLevelModel>();
        private ObservableCollection<VAROutputModel> varOutputList = new ObservableCollection<VAROutputModel>();
        private ObservableCollection<NoInstrumentsModel> noInstrumentsList = new ObservableCollection<NoInstrumentsModel>();
        
        public VARCalculatorViewModel()
        {
            VARService = new VARProcessor();

            //Set portfolio value
            portfolioValue = 1000000;

            //Set start and end dates
            startDate = new DateTime(2012, 1, 1);
            endDate = new DateTime(2017, 1, 1);
          
            //Add confidence levels to select from
            confLevelList.Add(new ConfLevelModel(90));
            confLevelList.Add(new ConfLevelModel(91));
            confLevelList.Add(new ConfLevelModel(92));
            confLevelList.Add(new ConfLevelModel(93));
            confLevelList.Add(new ConfLevelModel(94));
            confLevelList.Add(new ConfLevelModel(95));
            confLevelList.Add(new ConfLevelModel(96));
            confLevelList.Add(new ConfLevelModel(97));
            confLevelList.Add(new ConfLevelModel(98));
            confLevelList.Add(new ConfLevelModel(99));
            selectedConfLevelIndex = "9";
            selectedConfLevel = 99;
            
            
            //Add to number of instruments list
            noInstrumentsList.Add(new NoInstrumentsModel(2));
            noInstrumentsList.Add(new NoInstrumentsModel(10));
            noInstrumentsList.Add(new NoInstrumentsModel(50));
            noInstrumentsList.Add(new NoInstrumentsModel(100));
            noInstrumentsList.Add(new NoInstrumentsModel(200));
            noInstrumentsList.Add(new NoInstrumentsModel(300));
            noInstrumentsList.Add(new NoInstrumentsModel(400));
            noInstrumentsList.Add(new NoInstrumentsModel(500));
            noInstrumentsList.Add(new NoInstrumentsModel(1000));
            noInstrumentsList.Add(new NoInstrumentsModel(2000));
            noInstrumentsList.Add(new NoInstrumentsModel(3000));
            selectedNoInstrumentsIndex = "2";
            selectedNoInstruments = 50;
            calculateLabel = "Calculate";
            
        }


        //For progress bar
        public int ProgressNumber
        {
            get { return progressNumber; }
            set { progressNumber = value;
            NotifyPropertyChanged("ProgressNumber");
            }
        }


        //For progress status text
        public string CurrentStatus
        {
            get { return currentStatus; }
            set { currentStatus = value;
            NotifyPropertyChanged("CurrentStatus");
            }
        }

       //Number of instruments to run VAR for
        public string SelectedNoInstruments
        {
            get { return selectedNoInstrumentsIndex; }

            set
            {
                selectedNoInstruments = noInstrumentsList[Convert.ToInt32(value)].NoInstruments;
            }
        }

        public IEnumerable<NoInstrumentsModel> NoInstrumentsList
        {
            get { return noInstrumentsList; }
        }

        //Confidence levels
        public string SelectedConfLevel
        {
            get { return selectedConfLevelIndex; }

            set
            {
                selectedConfLevel = confLevelList[Convert.ToInt32(value)].ConfLevel;
            }
        }

        public IEnumerable<ConfLevelModel> ConfLevelList
        {
            get { return confLevelList; }
        }


        //Output for VAR table 
        public IEnumerable<VAROutputModel> VAROutputList
        {
            get { return varOutputList; }
        }


        public DateTime StartDate
        {
            get { return startDate; }

            set
            {
                startDate = value;
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }

            set
            {
                endDate = value;
            }
        }

        public string PortfolioValue
        {
            get { return portfolioValue.ToString(); }

            set
            {
                portfolioValue = Convert.ToDouble(value);
            }
        }

        public string CalculateLabel
        {
            get { return calculateLabel; }

            set
            {
                calculateLabel = value;
                NotifyPropertyChanged("CalculateLabel");
                
            }
        }

        public ICommand VARCalculateClick
        {
            get
            {
                return varCalculateClick ?? (varCalculateClick = new CommandHandler(() => CalculateVAR(), true));
            }
        }
       

        //Main function for running VAR
        private async Task CalculateVAR()
        {
            if(calculateLabel == "Cancel")
                {
                    //Setting cancel
                    CancelVARRun();
                    isRunning = false;
                    CalculateLabel = "Calculate";
                
                }
                else
                {
                    //run 
                    //Async task to stop blocking of UI
                    isRunning = true;
                    CalculateLabel = "Cancel";
                    var token = tokenSource.Token;

                     //Progress number for progress bar
                    Progress<int> progressHandler = new Progress<int>(value => { ProgressNumber = value; });
                    IProgress<int> progress = progressHandler as IProgress<int>;

                    //Status text
                    Progress<string> statusHandler = new Progress<string>(value => { CurrentStatus = value; });
                    IProgress<string> status = statusHandler as IProgress<string>;

                    try
                    {
                        //Run VAR calculation
                        VAROutputModel VAROutput = await Task.Run(() => VARService.ProcessVAR(portfolioValue, (selectedConfLevel / 100), selectedNoInstruments, startDate, endDate, token, progress, status));
                        tokenSource.Dispose();
                        tokenSource = new CancellationTokenSource();
                        varOutputList.Add(VAROutput);
                        isRunning = false;
                        CalculateLabel = "Calculate";
                    }
                    catch(Exception ex)
                    {
                        MessageBoxResult result = MessageBox.Show("ERROR: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                   
                }
        }


        //Clear results grid
        public ICommand ClearGridClick
        {
            get
            {
                return clearGridClick ?? (clearGridClick = new CommandHandler(() => ClearGrid(), true));
            }
        }


        private void ClearGrid()
        {
            varOutputList.Clear();
        }

        public ICommand CancelVARClick
        {
            get
            {
                return cancelVARClick ?? (cancelVARClick = new CommandHandler(() => CancelVARRun(), true));
            }
        }


        private void CancelVARRun()
        {
            tokenSource.Cancel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string currentStatus;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        
  
    }
}
