using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BackeryApp.ClassUtils;
using BackeryApp.ViewModel;
using DataAccess;
using SupplyStock;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for IndexTemplates.xaml
    /// </summary>
    public partial class IndexTemplates : UserControl
    {
        #region Variables

        private ObservableCollection<CostTemplate> _templates { get; set; }
        private readonly MContext _context;
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        #endregion
        #region Contructors

        public IndexTemplates()
        {
            InitializeComponent();
            //this.Templates = new ObservableCollection<CostTemplate>();
            
            //listView.ItemsSource = this.Templates;
            //cancelButton.Click += GoHome;
        }
        public IndexTemplates(MContext context)
        {
            InitializeComponent();
            _context = context;
            _templates = new ObservableCollection<CostTemplate>(_context.LocalTemplates);
            InitIndexTemplates();
            listView.ItemsSource = _templates;
            
            cancelButton.Click += GoHome;
            newTemplateButton.Click += NewCostTemplate;
            editButton.Click += EditTemplate;
            delTemplateButton.Click += RemoveTemplate;
            searchTempateText.GotFocus += SeachGotFocus;
            searchTempateText.LostFocus += SeachLostFocus;
            searchTempateText.TextChanged += FilterTemplates;
            selectedFilterBox.SelectionChanged += FilterTemplates;
            goSuppliesIndex.Click += GoSupplies;
            //this.IsVisibleChanged += ReloadView;
        }

        #endregion
        #region Methods

        #region GUI Methods

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

        #endregion

        private void InitIndexTemplates()
        {
            _context.LocalTemplates.CollectionChanged += (sender, args) =>
                                                                  {
                                                                      var text = searchTempateText.Text != "Filtrar Fichas de Costo..." ? searchTempateText.Text : "";
                                                                      if (args.NewItems != null)
                                                                      {
                                                                          foreach (var newItem in args.NewItems)
                                                                              if (PassTheFilter((CostTemplate)newItem, text, selectedFilterBox.SelectedIndex))
                                                                                  _templates.Add((CostTemplate)newItem);
                                                                      }
                                                                      if (args.OldItems != null)
                                                                      {
                                                                          FilterTemplates(sender, new RoutedEventArgs());
                                                                      }
                                                                  };
        }
        private void FilterTemplates(object sender, RoutedEventArgs e)
        {
            _templates.Clear();
            var text = searchTempateText.Text != "Filtrar Fichas de Costo..." ? searchTempateText.Text : "";
            foreach (var template in _context.LocalTemplates.Where(template => PassTheFilter(template, text, selectedFilterBox.SelectedIndex)))
                _templates.Add(template);
        }
        private bool PassTheFilter(CostTemplate template, string text, int option)
        {
            return template.Name.ToLower().Contains(text.ToLower()) &&
                   ((option == 0) || 
                    (option == 1 && !template.FinishedTemplate) ||
                    (option == 2 && template.FinishedTemplate));
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var home = (StartUpControl)this.Find("homeUserControl");
            home.Visibility = Visibility.Visible;
        }
        private void GoSupplies(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var supplies = this.Find("suppliesUserControl");
            //((IndexSupplies)supplies).FilterSupplies(sender, e);
            supplies.Visibility = Visibility.Visible;
        }
        private void InitEditTemplates(CostTemplate template = null)
        {
            var editTemplateUserControl = new EditCostTemplate(_context, template)
            {
                Name = "editTemplateUserControl",
                Width = double.NaN,
            };
            Grid.SetRowSpan(editTemplateUserControl, 2);
            ((Grid)Parent).Children.Add(editTemplateUserControl);
            //editTemplateUserControl.IsVisibleChanged += RefilterTemplates;
        }
        private void NewCostTemplate(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            InitEditTemplates();
        }
        private void EditTemplate(object sender, RoutedEventArgs e)
        {
            var temp = listView.SelectedItem as CostTemplate;
            if (temp == null)
            {
                MessageBox.Show("Es necesario tener seleccionada una ficha de costo antes de editar.", "Error editando",
                                MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            Visibility = Visibility.Hidden;
            InitEditTemplates(temp);
        }
        private void RemoveTemplate(object sender, RoutedEventArgs e)
        {
            var temp = listView.SelectedItem as CostTemplate;
            if (temp == null)
            {
                MessageBox.Show("Es necesario tener seleccionada una ficha de costo antes de eliminar.", "Error borrando",
                                MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            var affecedTemplates =  _context.RemoveTemplate(temp);
            if (affecedTemplates.Count > 0)
            {
                var message = affecedTemplates.Aggregate("Las siguientes fichas de costo serán modificadas tras la eliminación de este insumo. Desea continuar con la eliminación?\n\n", (current, affecedTemplate) => current + ("\t" + affecedTemplate + "\n"));

                var answer = MessageBox.Show(message, "Eliminación de insumos", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (answer == MessageBoxResult.OK)
                    _context.RemoveTemplate(temp, true);
            }
            //this.FilterTemplates(sender, e);
        }
        private void ViewCostTemplate(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var template = ((ListViewItem) sender).Content as CostTemplate;
            var viewTemplate = new ViewCostTemplate(new CostTemplateVM(_context, template)){Name = "ViewCostTemplate"};
            //Grid.SetRow(viewTemplate, 1);
            Grid.SetRowSpan(viewTemplate, 2);
            ((Grid) Parent).Children.Add(viewTemplate);
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
            Sort(header, direction);
            _lastHeaderClicked = headerClicked;
            _lastDirection = direction;

        }
        private void Sort(string header, ListSortDirection direction)
        {
            Comparison<CostTemplate> comparer = null;

            switch (header)
            {
                case "Nombre":
                    comparer = (x, y) => x.Name.CompareTo(y.Name);
                    break;
                case "Descripción":
                    comparer = (x, y) => x.Description == null ? 1 : y.Description == null ? -1 : x.Description.CompareTo(y.Description);
                    break;
                case "Unidades Producidas":
                    comparer = (x, y) => x.ProducedUnits.CompareTo(y.ProducedUnits);
                    break;
                case "Costo Total":
                    comparer = (x, y) => x.Cost.ToCUC().CompareTo(y.Cost.ToCUC());
                    break;
                case "Ganancia Neta":
                    comparer = (x, y) => double.IsNaN(x.Profit.Amount) ? 1 : double.IsNaN(y.Profit.Amount) ? -1 : x.Profit.ToCUC().CompareTo(y.Profit.ToCUC());
                    break;
                case "% de Ganancia":
                    comparer = (x, y) => String.Compare(x.Percentage, y.Percentage, StringComparison.Ordinal);
                    break;
            }

            if (comparer == null) return;
            var items = new List<CostTemplate>(_templates);
            _templates.Clear();
            items.Sort(comparer);
            if (direction == ListSortDirection.Ascending)
                items.Reverse();
            items.ForEach(s => _templates.Add(s));

        }

        #endregion
    }
}
