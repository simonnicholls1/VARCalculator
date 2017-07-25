using FuzzyLogicSearch.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzyLogicSearch.Services;
using System.Windows;

namespace FuzzyLogicSearch.ViewModel
{
    public class InstrumentSearchViewModel : ObservableObject
    {
        private InstrumentSearchService instrumentService;
        private string instrumentSearch;
        private string selectedMatchAlgo;
        private string selectedMatchAlgoIndex;
        private double selectedThreshold;
        private string selectedThresholdIndex;
        
        private ObservableCollection<InstrumentSearchModel> instrumentList = new ObservableCollection<InstrumentSearchModel>();
        private ObservableCollection<MatchAlgoModel> matchAlgoList = new ObservableCollection<MatchAlgoModel>();
        private ObservableCollection<ThresholdModel> thresholdList = new ObservableCollection<ThresholdModel>();
        private List<InstrumentSearchModel> currentInstrumentList = new List<InstrumentSearchModel>();
        private BackgroundWorker instrumentWorker;

        public InstrumentSearchViewModel()
        {
            instrumentService = new InstrumentSearchService();

            //Add matching algo's to select from
            matchAlgoList.Add(new MatchAlgoModel("Dice Coefficient"));
            matchAlgoList.Add(new MatchAlgoModel("Levenshtein Distance"));
            selectedMatchAlgoIndex = "0";
            selectedMatchAlgo = "Dice Coefficient";

            //Add thresholds to select from
            thresholdList.Add(new ThresholdModel(0));
            thresholdList.Add(new ThresholdModel(10));
            thresholdList.Add(new ThresholdModel(20));
            thresholdList.Add(new ThresholdModel(30));
            thresholdList.Add(new ThresholdModel(40));
            thresholdList.Add(new ThresholdModel(50));
            thresholdList.Add(new ThresholdModel(60));
            thresholdList.Add(new ThresholdModel(70));
            thresholdList.Add(new ThresholdModel(80));
            selectedThresholdIndex = "5";
            selectedThreshold = 50;


            InitializeBackgroundWorker();
            
        }

        public string InstrumentSearch 
        {
            get { return instrumentSearch; } 
             set 
             {
                 instrumentSearch = value;
                 RaisePropertyChangedEvent("InstrumentSearch");

                 if (instrumentSearch == "")
                 {
                     //UpdateInstrumentList();
                     instrumentList.Clear();
                 }
                 else
                 {
                     if (instrumentWorker.IsBusy != true)
                     {
                         // Go get the instruments
                         instrumentWorker.RunWorkerAsync(instrumentSearch);
                     }
                     else
                     {
                         instrumentWorker.CancelAsync();
                         instrumentWorker.Dispose();
                         //Spawn new background worker
                         InitializeBackgroundWorker();
                         instrumentWorker.RunWorkerAsync(instrumentSearch);
                     }
                 }
             } 
         }

        public string SelectedThreshold
        {
            get { return selectedThresholdIndex; }

            set
            {
                selectedThreshold = thresholdList[Convert.ToInt32(value)].Threshold;
            }
        }

        public IEnumerable<ThresholdModel> ThresholdList
        {
            get { return thresholdList; }
        }

        public string SelectedMatchAlgo
        {
            get { return selectedMatchAlgoIndex; }

            set
            {
                selectedMatchAlgo = matchAlgoList[Convert.ToInt32(value)].MatchAlgo;
            }
        }

        public IEnumerable<MatchAlgoModel> MatchAlgoList
        {
            get { return matchAlgoList;  }
        }

        public IEnumerable<InstrumentSearchModel> InstrumentList
        {
            get { return instrumentList; }
        }

        void UpdateInstrumentListFiltered()
        {  
            
            instrumentList.Clear();
            foreach (InstrumentSearchModel instrument in currentInstrumentList)
            {
                instrumentList.Add(instrument);
            }
        }

        void getFilteredInstruments(string filter)
        {
            currentInstrumentList = instrumentService.getFilteredInstruments(filter, currentInstrumentList, selectedMatchAlgo, selectedThreshold);
        }

        public void UpdateInstrumentList()
        {
            currentInstrumentList = instrumentService.getAllInstruments();
            
            instrumentList.Clear();
            foreach (InstrumentSearchModel instrument in currentInstrumentList)
            {
                instrumentList.Add(instrument);
            }
        }

        //Initialisations and Call backs for multi-thread
        private void InitializeBackgroundWorker()
        {
            instrumentWorker = new BackgroundWorker();
            instrumentWorker.WorkerSupportsCancellation = true;
            instrumentWorker.DoWork += new DoWorkEventHandler(instrumentWorker_DoWork);
            instrumentWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(instrumentWorker_RunWorkerCompleted);
        }

        private void instrumentWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                getFilteredInstruments((string)e.Argument);
            }
            catch (ServiceException ex)
            {
                MessageBoxResult result = MessageBox.Show("Error Occurred: " + ex.Message, "Confirmation", MessageBoxButton.OK);
            }
        }

        private void instrumentWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                UpdateInstrumentListFiltered();
            }
        }

    }
}
