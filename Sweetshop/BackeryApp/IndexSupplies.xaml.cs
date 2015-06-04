  using System;
  using System.ComponentModel;
  using System.Windows;
using System.Windows.Controls;
  using System.Windows.Media;
using System.Windows.Media.Effects;
  using BackeryApp.ClassUtils;
  using BackeryApp.ViewModel;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for IndexSupplies.xaml
    /// </summary>
    public partial class IndexSupplies : UserControl
    {
        #region Variables

        private readonly IndexSuppliesVM _viewModel;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        #endregion
        #region Contructors

        public IndexSupplies()
        {
            InitializeComponent();
        }
        public IndexSupplies(IndexSuppliesVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            newSupplyButton.Click += NewSuply;
            editSupplyButton.Click += EditSupply;
            delSupplyButton.Click += RemoveSupply;
            cancelButton.Click += GoHome;

            searchTempateText.GotFocus += SeachGotFocus;
            searchTempateText.LostFocus += SeachLostFocus;
            searchTempateText.TextChanged += FilterSupplies;
            selectedFilterBox.SelectionChanged += FilterSupplies;
            goTemplatesIndex.Click += GoTemplates;
        }


        #endregion
        #region Methods

        private void SeachGotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTempateText.Text == "Filtrar Insumo...")
                searchTempateText.Text = "";
            searchTempateText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void SeachLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTempateText.Text))
                searchTempateText.Text = "Filtrar Insumo...";
            searchTempateText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        }
        private void GoHome(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var home = (StartUpControl) this.Find("homeUserControl");
            home.Visibility = Visibility.Visible;
        }
        
        private void NewSuply(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            InitEditSupply(new SupplyVM(_viewModel.Context));
        }
        private void EditSupply(object sender, RoutedEventArgs e)
        {
            var supp = listView.SelectedItem as SupplyVM;
            if (supp == null)
            {
                MessageBox.Show("Es necesario tener seleccionada un insumo antes de editar.", "Error editando",
                               MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            IsEnabled = false;
            InitEditSupply(supp);
        }
        private void InitEditSupply(SupplyVM supply)
        {
            var blurEffect = new BlurEffect { Radius = 1, KernelType = KernelType.Box };
            Effect = blurEffect;

            var newSupply = new EditSupply(supply)
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            newSupply.Closed += CloseNewSupply;
            IsEnabled = false;
        }
        private void RemoveSupply(object sender, RoutedEventArgs e)
        {
            var supp = listView.SelectedItem as SupplyVM;
            _viewModel.RemoveSupply(supp);
        }
        private void CloseNewSupply(object sender, EventArgs e)
        {
            Effect = null;
            IsEnabled = true;
        }
        private void ViewSupply(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var supplyVM = ((ListViewItem)sender).Content as SupplyVM;

            var viewSupply = new ViewSupply(supplyVM) { Name = "ViewSupply" };
            Grid.SetRowSpan(viewSupply, 2);
            ((Grid)Parent).Children.Add(viewSupply);
        }
        
        //cambiar para que se borre o creee uno nuevo cada vez que se acceda a el. Quitar de todos los metodos que oculten la ventana.
        private void GoTemplates(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var templates = this.Find("templatesUserControl");
            templates.Visibility = Visibility.Visible;
        }


        private void FilterSupplies(object sender, EventArgs e)
        {
            _viewModel.SearchPattern = searchTempateText.Text;
            _viewModel.FilterSupplies();
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
