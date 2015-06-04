using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BackeryApp.ClassUtils;
using BackeryApp.ViewModel;
using UserControl = System.Windows.Controls.UserControl;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for ViewProduction.xaml
    /// </summary>
    public partial class ViewProduction : UserControl
    {
        #region Variables

        private readonly ProductionVM _viewModel;

        #endregion
        #region Constructor

        public ViewProduction()
        {
            InitializeComponent();
        }
        public ViewProduction(ProductionVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;

            GoMainMenu.Click += GoMain;
            CancelButton.Click += ExitToIndex;
            CostList.SelectionChanged += ConvertCost;
            ProfitsList.SelectionChanged += ConvertProfit;
            DaysBox.LostFocus += RecalculateDays;
        }

        private void RecalculateDays(object sender, RoutedEventArgs e)
        {
            _viewModel.DaysChange();
        }

        private void ConvertProfit(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.Profits = ProfitsList.SelectedIndex == 0 ? _viewModel.Profits.ToCUC() : _viewModel.Profits.ToCUP();
        }

        private void ConvertCost(object sender, SelectionChangedEventArgs e)
        {
            if (CostList.SelectedIndex == 0)
            {
                _viewModel.Cost = _viewModel.Cost.ToCUC();
                _viewModel.SupplyAmounts.ForEach(sa => sa.Cost = sa.Cost.ToCUC());
            }
            else
            {
                _viewModel.Cost = _viewModel.Cost.ToCUP();
                _viewModel.SupplyAmounts.ForEach(sa => sa.Cost = sa.Cost.ToCUP());

            }
        }

        #endregion
        #region Methods

        private void GoMain(object sender, RoutedEventArgs e)
        {
            var home = this.Find("homeUserControl");
            home.Visibility = Visibility.Visible;
            this.AutoRemove();
        }
        private void ExitToIndex(object sender, RoutedEventArgs e)
        {
            var productionsControl = (IndexProductions)this.Find("productionsUserControl");
            productionsControl.Visibility = Visibility.Visible;
            _viewModel.Days = 1;
            _viewModel.ResetToProduction(true);
            this.AutoRemove();
        }

        #endregion
    }
}
