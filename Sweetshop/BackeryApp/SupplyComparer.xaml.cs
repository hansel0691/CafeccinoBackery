using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using BackeryApp.ViewModel;
using SupplyStock;
using SupplyStock.Utils;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for SupplyComparer.xaml
    /// </summary>
    public partial class SupplyComparer : UserControl
    {
        #region Variables

        //private Supply _supply;
        private SupplyComparerVM _viewModel;

        #endregion
        #region Constructor

        public SupplyComparer()
        {
            InitializeComponent();
        }
        public SupplyComparer(Supply supply, double maxCust, Currency selectedCostPerUnit, Measurement definedAmount, MouseEventHandler mouseEnter, MouseEventHandler mouseLeave)
        {
            InitializeComponent();
            _viewModel = new SupplyComparerVM(supply.ToString(), supply.CostPerUnit(), maxCust, selectedCostPerUnit, definedAmount);
            Supply = supply;
            DataContext = _viewModel;

            LoadController(mouseEnter, mouseLeave);
        }


        #endregion
        #region Properties

        public Supply Supply { get; set; }

        #endregion
        #region Methods

        private void LoadController(MouseEventHandler mouseEnter, MouseEventHandler mouseLeave)
        {
            if (Supply.CostPerUnit().Amount == 0)
            {
                middleGrid.Children.Clear();
                CostPerBasicUnitPanel.Width = 0;
                var TextBlock = new TextBlock() { Text = "Este insumo no tiene definida una cantidad o el costo es 0 y se hace imposible su comparación.", FontSize = 13, Foreground = new SolidColorBrush(Color.FromRgb(53, 153, 255)), VerticalAlignment = VerticalAlignment.Center };
                Grid.SetColumn(TextBlock, 1);
                Grid.SetColumnSpan(TextBlock, 2);
                contentGrid.Children.Add(TextBlock);
                return;
            }

            if (_viewModel.MaxCostPerUnitCUC == 0)
                _viewModel.MaxCostPerUnitCUC = _viewModel.UnitCost.AmountCUC;

            var columnDefinition = new ColumnDefinition { MinWidth = 1, Width = new GridLength(_viewModel.Proportion, GridUnitType.Star), Name = "Slider" };
            RegisterName(columnDefinition.Name, columnDefinition);
            middleGrid.ColumnDefinitions.Add(columnDefinition);
            middleGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1 - _viewModel.Proportion + 0.4, GridUnitType.Star) });
            Grid.SetColumn(barRect, 0);

            differenceInfo.Foreground =
                new SolidColorBrush(_viewModel.Difference.Amount >= 0
                                        ? Color.FromRgb(128, 163, 5)
                                        : Color.FromRgb(204, 51, 0));
            differenceInfo.DataContext = _viewModel.Difference;
            Grid.SetColumn(differenceInfo, 1);

            var fill = new Rectangle() { Width = 2000, Fill = new SolidColorBrush(Colors.Transparent), DataContext = new { UnitCost = _viewModel.UnitCost, FormatAmount = Supply.FormatAmount } };
            Grid.SetColumnSpan(fill, 2);
            middleGrid.Children.Add(fill);
            fill.MouseEnter += mouseEnter;
            fill.MouseLeave += mouseLeave;

            unitCostBox.SelectionChanged += RecalculateCostPerUnit;
        }
        public void RecalculateCostPerUnit(object sender, EventArgs e)
        {
            Currency amount;
            Currency difference;
            if (unitCostBox.SelectedItem as string == "CUP")
            {
                amount = _viewModel.UnitCost.ToCUP();
                difference = _viewModel.Difference.ToCUP();
            }
            else
            {
                amount = _viewModel.UnitCost.ToCUC();
                difference = _viewModel.Difference.ToCUC();
            }
            _viewModel.UnitCost.Amount = amount.Amount;
            _viewModel.UnitCost.Unit = amount.Unit;
            _viewModel.Cost = amount * _viewModel.DefinedAmount.AmountInUnit;

            differenceInfo.Text = string.Format("({0}{1} {2})", Math.Sign(difference.Amount) >= 0 ? "+" : "-",
                                                Math.Abs(difference.Amount).SmartString(), difference.Unit);
            unitCostText.Text = _viewModel.Cost.Amount.SmartString();
        }

        #endregion
    }

   
}
