using VARCalculator.ViewModel;
using System.Windows;
using MahApps.Metro.Controls;

namespace VARCalculator
{
    /// <summary>
    /// Interaction logic for InstrumentSearch.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        VARCalculatorViewModel viewModel;

        public MainWindow()
        {
            viewModel = new VARCalculatorViewModel();

            this.DataContext = viewModel;
            InitializeComponent();
        }

    }
}
