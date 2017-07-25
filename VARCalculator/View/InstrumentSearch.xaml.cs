using FuzzyLogicSearch.ViewModel;
using System.Windows;
using MahApps.Metro.Controls;

namespace FuzzyLogicSearch
{
    /// <summary>
    /// Interaction logic for InstrumentSearch.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InstrumentSearchViewModel viewModel = new InstrumentSearchViewModel();
            //Get initial list of all instruments
            //viewModel.SelectedMatchAlgo = "Levenshtein Distance";

            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
