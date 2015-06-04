using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using BackeryApp.ClassUtils;
using BackeryApp.ViewModel;

namespace BackeryApp
{
    public partial class ViewSupply : UserControl
    {
        #region Variables

        private readonly SupplyVM _viewModel;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        
        #endregion
        #region Constructor

        public ViewSupply()
        {
            InitializeComponent();
        }
        public ViewSupply(SupplyVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            
            DataContext = _viewModel;

            InitImage();
            Loaded += LoadFormatComparer;
            searchTempateText.GotFocus += SeachGotFocus;
            searchTempateText.LostFocus += SeachLostFocus;
            searchTempateText.TextChanged += FilterTemplates;
            formatAmountBox.SelectionChanged += RecalculateFormatAmount;
            formatCostBox.SelectionChanged += RecalculateFormatCost;
            goMainMenu.Click += GoMain;
            cancelButton.Click += ExitToIndex;
        }

        #endregion
        #region Methods

        private void InitImage()
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.Supply.Image))
            {
                if (!File.Exists(_viewModel.Supply.Image))
                {
                    MessageBox.Show(
                        "La ruta de la imagen asociada a esta ficha de costo a cambiado. Por favor vuelva a seleccionar una imagen.",
                        "Error mostrando imagen", MessageBoxButton.OK, MessageBoxImage.Error);
                    _viewModel.ResetImage();
                    return;
                }
                img.Source = new BitmapImage(new Uri(_viewModel.Supply.Image));
                frameImage.Background = new SolidColorBrush(Colors.White);
            }
        }
        private void SeachGotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTempateText.Text == "Filtrar Fichas de Costo...")
                searchTempateText.Text = "";
            searchTempateText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void SeachLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTempateText.Text))
                searchTempateText.Text = "Filtrar Fichas de Costo...";
            searchTempateText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        }
        private void LoadFormatComparer(object sender, RoutedEventArgs e)
        {
            var formatComparer = new ComparerList(_viewModel.Supply, _viewModel.RelatedSupplies, SelectFormatList, UnselectList)
            {
                Margin = new Thickness(0, 0, 0, 0),
                AddToolTip = AddToolTip,
                RemoveToolTip = RemoveToolTip,
            };
            comparerGrid.Children.Add(formatComparer);
        }
        private void SelectFormatList(object sender, RoutedEventArgs e)
        {
            var temp = (sender as ListBox).SelectedValue as SupplyComparer;
            if (temp == null)
                return;
            _viewModel.RecalculateTemplateCosts(temp.Supply);
        }
        private void UnselectList(object sender, RoutedEventArgs e)
        {
            var listBox = sender as ListBox;
            listBox.SelectedIndex = -1;
            _viewModel.ResetTemplateCosts();
        }
        private void GoMain(object sender, RoutedEventArgs e)
        {
            var home = this.Find("homeUserControl");
            home.Visibility = Visibility.Visible;
            this.AutoRemove();
        }
        private void ExitToIndex(object sender, RoutedEventArgs e)
        {
            var suppliesControl = (IndexSupplies)this.Find("suppliesUserControl");
            //((IndexSupplies)suppliesControl).FilterSupplies(sender, e);
            suppliesControl.Visibility = Visibility.Visible;
            this.AutoRemove();
        }
        private void RecalculateFormatAmount(object sender, RoutedEventArgs e)
        {
            _viewModel.ConvertFormatAmount(formatAmountBox.SelectedItem as string);
        }
        private void RecalculateFormatCost(object sender, RoutedEventArgs e)
        {
            _viewModel.ConvertFormatCost(formatCostBox.SelectedItem as string);
        }
        private void FilterTemplates(object sender, TextChangedEventArgs e)
        {
            templateSupplyList.ItemsSource = _viewModel.FilterTemplates(searchTempateText.Text);
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
        
        public void AddToolTip(string message)
        {
            var mousePosition = Mouse.GetPosition(this);
            var tip = new ToolTip(message)
            {
                Name = "ToolTip",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0),
                Opacity = 0
            };
            Grid.SetRowSpan(tip, 20);
            Grid.SetColumnSpan(tip, 10);
            mainBlock.Children.Add(tip);
            RegisterName(tip.Name, tip);

            var myDoubleAnimation = new DoubleAnimation { To = 1, Duration = new Duration(TimeSpan.FromSeconds(0.2)), BeginTime = TimeSpan.FromSeconds(0.5)};

            var myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);

            Storyboard.SetTargetName(myDoubleAnimation, tip.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(OpacityProperty));
            myStoryboard.Begin(this);
        }
        public void RemoveToolTip()
        {
            mainBlock.Children.RemoveAt(mainBlock.Children.Count - 1);
            UnregisterName("ToolTip");

        }

        #endregion
    }

}
