using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BackeryApp.ClassUtils;
using BackeryApp.ViewModel;
using SupplyStock.Utils;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for ViewCostTemplate.xaml
    /// </summary>
    public partial class ViewCostTemplate : UserControl
    {
        #region Variables

        private readonly CostTemplateVM _viewModel;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        #endregion
        #region Contructor

        public ViewCostTemplate()
        {
            InitializeComponent();
        }
        public ViewCostTemplate(CostTemplateVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;


            if (!_viewModel.Template.FinishedTemplate)
            {
                sellingPriceGrid.Visibility = Visibility.Hidden;
                sellingPriceGrid.Width = 0;
                sellingPriceGrid.Height = 0;
                sellingPriceGrid.Margin = new Thickness(0);
                profitsGrid.Visibility = Visibility.Hidden;
                profitsGrid.Width = 0;
                profitsGrid.Height = 0;
                profitsGrid.Margin = new Thickness(0);
            }

            InitImage();
            goMainMenu.Click += GoMain;
            cancelButton.Click += Exit;
            format_costList.SelectionChanged += RecalculateCost;
            costPerUnit.SelectionChanged += RecalculateCostPerUnit;
            sellingPrice.SelectionChanged += RecalculateSellingPrice;
            profitBox.SelectionChanged += RecalculateProfits;
        }

        #endregion
        #region Methods

        private void InitImage()
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.Template.Image))
            {
                if (!File.Exists(_viewModel.Template.Image))
                {
                    MessageBox.Show(
                        "La ruta de la imagen asociada a esta ficha de costo a cambiado. Por favor vuelva a seleccionar una imagen.",
                        "Error mostrando imagen", MessageBoxButton.OK, MessageBoxImage.Error);
                    _viewModel.ResetImage();
                    return;
                }
                img.Source = new BitmapImage(new Uri(_viewModel.Template.Image));
                frameImage.Background = new SolidColorBrush(Colors.White);
            }
        }
        private void RecalculateCost(object sender, RoutedEventArgs e)
        {
            _viewModel.ConvertCost(format_costList.SelectedItem as string);
        }
        private void RecalculateCostPerUnit(object sender, RoutedEventArgs e)
        {
            _viewModel.ConvertCostPerUnit(costPerUnit.SelectedItem as string);
        }
        private void RecalculateSellingPrice(object sender, RoutedEventArgs e)
        {
            _viewModel.ConvertSellingPrice(sellingPrice.SelectedItem as string);
        }
        private void RecalculateProfits(object sender, RoutedEventArgs e)
        {
            _viewModel.ConvertProfits(profitBox.SelectedItem as string);
        }

        private void GoMain(object sender, RoutedEventArgs e)
        {
            var home = this.Find("homeUserControl");
            home.Visibility = Visibility.Visible;
            this.AutoRemove();
        }
        private void Recalculate(TextBlock control, Currency amount, string selection)
        {
            control.Text = selection == "CUC"
                                        ? amount.ToCUC().Amount.SmartString()
                                        : amount.ToCUP().Amount.SmartString();
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            var index = this.Find("templatesUserControl");
            index.Visibility = Visibility.Visible;
            var gridChildren = ((Grid)Parent).Children;
            gridChildren.RemoveAt(gridChildren.Count - 1);
        }

        private void SortData(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;
            if (headerClicked == null) return;

            if (headerClicked.Role == GridViewColumnHeaderRole.Padding) return;

            direction = headerClicked != _lastHeaderClicked
                            ? ListSortDirection.Ascending
                            : _lastDirection == ListSortDirection.Ascending
                                  ? ListSortDirection.Descending
                                  : ListSortDirection.Ascending;

            var header = headerClicked.Column.Header as string;
            _viewModel.Sort(header, direction);
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;

        }

        #endregion
    }
}
