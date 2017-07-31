using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VARCalculator.Model;
using VARCalculator.Services;

namespace VARCalculator.ViewModel
{
    public class VARCalculatorViewModel : ObservableObject
    {
        private VARProcessor VARService;
        
        private string varLevelLabel;
        private double selectedConfLevel;
        private string selectedConfLevelIndex;
        private ICommand varCalculateClick;
        private ICommand clearGridClick;
        private DateTime startDate;
        private DateTime endDate;
        private double portfolioValue;

        private ObservableCollection<ConfLevelModel> confLevelList = new ObservableCollection<ConfLevelModel>();
        private ObservableCollection<VAROutputModel> varOutputList = new ObservableCollection<VAROutputModel>();
        

        public VARCalculatorViewModel()
        {
            VARService = new VARProcessor();


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
            selectedConfLevelIndex = "0";
            selectedConfLevel = 99;            
        }

       
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

        public double PortfolioValue
        {
            get { return portfolioValue; }

            set
            {
                portfolioValue = value;
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
        private async void CalculateVAR()
        {
            //Async task to stop blocking of UI
            VAROutputModel VAROutput = await Task.Run(() => VARService.ProcessVAR(portfolioValue, selectedConfLevel, startDate, endDate));
            varOutputList.Add(VAROutput);
        }

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
        
  
    }
}
