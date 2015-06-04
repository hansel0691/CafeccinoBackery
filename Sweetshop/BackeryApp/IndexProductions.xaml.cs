using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BackeryApp.ClassUtils;
using BackeryApp.ViewModel;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for IndexProductions.xaml
    /// </summary>
    public partial class IndexProductions : UserControl
    {
        #region Variables

        private readonly IndexProductionsVM _viewModel;

        #endregion
        #region Constructor

        public IndexProductions()
        {
            InitializeComponent();
        }
        public IndexProductions(IndexProductionsVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            SearchProductionText.GotFocus += SeachGotFocus;
            SearchProductionText.LostFocus += SeachLostFocus;
            SearchProductionText.TextChanged += Filter;
            DelProductionButton.Click += RemoveProduction;
            CancelButton.Click += GoHome;
            NewProductionButton.Click += NewProduction;
            EditProductionButton.Click += EditProduction;
        }

        private void Filter(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchPattern = SearchProductionText.Text == "Filtrar Producciones..."
                ? ""
                : SearchProductionText.Text;
            _viewModel.FilterProductions();
        }
        private void SeachGotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchProductionText.Text == "Filtrar Producciones...")
                SearchProductionText.Text = "";
            SearchProductionText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void SeachLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchProductionText.Text))
                SearchProductionText.Text = "Filtrar Producciones...";
            SearchProductionText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        }

        #endregion
        #region Methods

        private void RemoveProduction(object sender, RoutedEventArgs e)
        {
            var prod = listView.SelectedItem as ProductionVM;
            _viewModel.RemoveProduction(prod);
        }
        private void GoHome(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var home = (StartUpControl)this.Find("homeUserControl");
            home.Visibility = Visibility.Visible;
        }
        private void NewProduction(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            InitEditProduction();
        }

        private void EditProduction(object sender, RoutedEventArgs e)
        {
            var prod = listView.SelectedItem as ProductionVM;
            if (prod == null)
            {
                MessageBox.Show("Es necesario tener seleccionada una producción antes de editar.", "Error editando",
                                MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            Visibility = Visibility.Hidden;
            InitEditProduction(prod);
        }
        private void InitEditProduction(ProductionVM production = null)
        {
            var editProductionUserControl = new EditProduction(production ?? new ProductionVM(_viewModel.Context, null))
            {
                Name = "editProductionUserControl",
                Width = double.NaN,
            };
            Grid.SetRowSpan(editProductionUserControl, 2);
            ((Grid)Parent).Children.Add(editProductionUserControl);
        }

        #endregion

        private void ViewProduction(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var productionVM = ((ListViewItem)sender).Content as ProductionVM;

            var viewProduction = new ViewProduction(productionVM) { Name = "ViewProduction" };
            Grid.SetRowSpan(viewProduction, 2);
            ((Grid)Parent).Children.Add(viewProduction);
        }
    }
}
