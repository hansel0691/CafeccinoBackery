using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.IO;                      //for : input - output
using BackeryApp.ClassUtils;
using BackeryApp.ViewModel;
using Microsoft.Win32;                //For : OpenFileDialog / SaveFileDialog
//For : BitmapImage etc etc
using DataAccess;
using SupplyStock;
using SupplyStock.Utils;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for EditCostTemplate.xaml
    /// </summary>
    public partial class EditCostTemplate : UserControl
    {
        #region Variables

        private ObservableCollection<SupplyAmount> _selectedSupplies;
        private SupplyAmount _selectedSupplyAmount;
        private MContext _context;
        private CostTemplate _backUpTemplate;
        private bool _menuOpen;
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;


        #endregion
        #region Contructors

        public EditCostTemplate()
        {
            InitializeComponent();
        }
        public EditCostTemplate(MContext context, CostTemplate template = null)
        {
            InitializeComponent();

            Loaded += InitControl;
            nameText.GotFocus += CleanNameText;
            nameText.LostFocus += RestartNameText;
            produced_amountText.GotFocus += ProducedGotFocus;
            produced_amountText.LostFocus += ProducedLostFocus;
            sellingText.GotFocus += SellingGotFocus;
            sellingText.LostFocus += SellingLostFocus;
            searchSuplyText.GotFocus += SeachGotFocus;
            searchSuplyText.LostFocus += SeachLostFocus;
            detailsText.GotFocus += DetailsGotFocus;
            detailsText.LostFocus += DetailsLostFocus;
            searchSuplyText.TextChanged += FilterSupplies;
            selectedFilterBox.SelectionChanged += FilterSupplies;
            suppliesListView.SelectionChanged += DescribeSupply;
            cancelButton.Click += GoIndex;
            SaveButton.Click += SaveCostTemplate;
            newSupplyButton.Click += NewSupply;
            calculatedCostList.SelectionChanged += RecalculateUnit;
            removeFromSelected.Click += RemoveAll;
            selectedSuppliesListBox.SelectionChanged += RecalculateLocalCost;
            calculateLocalCostList.SelectionChanged += RecalculateLocalUnit;
            isFinishedCheckBox.Checked += IncreaseIsFinishedWidth;
            isFinishedCheckBox.Unchecked += DecreaseIsFinishedWidth;
            isFinishedLabel.MouseUp += CheckIsFinished;
            frameImage.MouseUp += SetImage;

            _selectedSupplies = template == null || template.SupplyAmounts == null
                                         ? new ObservableCollection<SupplyAmount>()
                                         : new ObservableCollection<SupplyAmount>(template.SupplyAmounts);
            if (template == null)
            {
                headeTitle.Text = "Nueva " + headeTitle.Text;
                SaveButton.Content = "Crear...";
                CostTemplate = new CostTemplate();
                pricePanel.Opacity = 0;
            }
            else
            {
                headeTitle.Text = "Editar " + headeTitle.Text;
                SaveButton.Content = "Salvar...";
                CostTemplate = (CostTemplate)template.Clone();
                _backUpTemplate = template;
                pricePanel.Opacity = template.FinishedTemplate ? 1 : 0;
            }

            _context = context;
            Supplies = new ObservableCollection<Supply>(CleanSupplies(context.LocalSupplies));
            
            DataContext = CostTemplate;
            suppliesListView.ItemsSource = Supplies;
            selectedSuppliesListBox.DataContext = _selectedSupplies;
            suppliesCount.DataContext = _selectedSupplies;
            InitImage();
        }

        #endregion
        #region Properties

        public CostTemplate CostTemplate { get; set; }
        public ObservableCollection<Supply> Supplies { get; set; }
        public ObservableCollection<SupplyAmount> SupplyAmounts { get; set; }

        #endregion
        #region Methods

        #region GUI Methods

        private void CheckIsFinished(object sender, RoutedEventArgs e)
        {
            isFinishedCheckBox.IsChecked = !isFinishedCheckBox.IsChecked;
        }
        
        private void DecreaseIsFinishedWidth(object sender, RoutedEventArgs e)
        {
            var myDoubleAnimation = new DoubleAnimation { To = 0, Duration = new Duration(TimeSpan.FromSeconds(0.5)) };

            var myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);

            Storyboard.SetTargetName(myDoubleAnimation, pricePanel.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(OpacityProperty));
            myStoryboard.Begin(this);
            CostTemplate.SellingPrice.Amount = 0;

            CostTemplate.ProducedUnits = CostTemplate.ProducedUnits == 1 ? 0 : CostTemplate.ProducedUnits;
            produced_amountText.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            pricePanel.IsEnabled = false;

        }
        private void IncreaseIsFinishedWidth(object sender, RoutedEventArgs e)
        {
            pricePanel.IsEnabled = true;
            var myDoubleAnimation = new DoubleAnimation { To = 1, Duration = new Duration(TimeSpan.FromSeconds(0.5)) };

            var myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);

            Storyboard.SetTargetName(myDoubleAnimation, pricePanel.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(OpacityProperty));
            myStoryboard.Begin(this);
            CostTemplate.ProducedUnits = CostTemplate.ProducedUnits == 0 ? 1 : CostTemplate.ProducedUnits;
            produced_amountText.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }
       
        private void CleanNameText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CostTemplate.Name))
                nameText.Text = "";
            nameText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void RestartNameText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CostTemplate.Name))
            {
                nameText.Text = "Introduzca un nombre...";
                nameText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }

        private void ProducedGotFocus(object sender, RoutedEventArgs e)
        {
            if (CostTemplate.ProducedUnits == 0)
                produced_amountText.Text = "";
            produced_amountText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));

        }
        private void ProducedLostFocus(object sender, RoutedEventArgs e)
        {
            //problema aqui cuando pones un string te da un error de validacion.
            if (string.IsNullOrWhiteSpace(produced_amountText.Text) || CostTemplate.ProducedUnits == 0)
            {
                produced_amountText.Text = "0";
                produced_amountText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }

        private void SellingGotFocus(object sender, RoutedEventArgs e)
        {
            if (CostTemplate.SellingPrice == null || CostTemplate.SellingPrice.AmountCUC == 0)
                sellingText.Text = "";
            sellingText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void SellingLostFocus(object sender, RoutedEventArgs e)
        {
            if (CostTemplate.SellingPrice == null || string.IsNullOrWhiteSpace(produced_amountText.Text) || CostTemplate.SellingPrice.AmountCUC == 0)
            {
                sellingText.Text = "0";
                sellingText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }

        private void SeachGotFocus(object sender, RoutedEventArgs e)
        {
            if (searchSuplyText.Text == "Filtrar Insumos...")
                searchSuplyText.Text = "";
            searchSuplyText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void SeachLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchSuplyText.Text))
                searchSuplyText.Text = "Filtrar Insumos...";
            searchSuplyText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        }

        private void DetailsGotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CostTemplate.Description))
                detailsText.Text = "";
            detailsText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void DetailsLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CostTemplate.Description))
            {
                detailsText.Text = "Describa el producto...";
                detailsText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }

        #endregion

        private void InitImage()
        {
            if (!string.IsNullOrWhiteSpace(CostTemplate.Image))
            {
                if (!File.Exists(CostTemplate.Image))
                {
                    MessageBox.Show(
                        "La ruta de la imagen asociada a esta ficha de costo a cambiado. Por favor vuelva a seleccionar una imagen.",
                        "Error mostrando imagen", MessageBoxButton.OK, MessageBoxImage.Error);
                    CostTemplate.Image = null;
                    _context.SaveChanges();
                    return;
                }
                img.Source = new BitmapImage(new Uri(CostTemplate.Image));
                frameImage.Background = new SolidColorBrush(Colors.White);
            }
        }
        private IEnumerable<Supply> CleanSupplies(IEnumerable<Supply> supplies)
        {
            if (CostTemplate.UnderlyingSupply == null) CostTemplate.UnderlyingSupply = new Supply();
            foreach (var supply in supplies)
            {
                if (supply.ToString() != CostTemplate.ToString() && supply.FormatAmount.Amount != 0 && (supply.Template == null || !supply.Template.FinishedTemplate))
                    yield return supply;
            }


            _context.Supplies.Local.CollectionChanged += (sender, args) =>
                                                                  {
                                                                      if (args.NewItems != null)
                                                                          foreach (var newItem in args.NewItems)
                                                                              if (PassTheFilter((Supply)newItem, searchSuplyText.Text != "Filtrar Insumos..." ? searchSuplyText.Text : "", selectedFilterBox.SelectedIndex))
                                                                                Supplies.Add((Supply)newItem);
                                                                  };
        }
        private void InitControl(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CostTemplate.Name))
            {
                nameText.Text = "Introduzca un nombre...";
                nameText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
            if (CostTemplate.ProducedUnits == 0)
            {
                produced_amountText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
            if (CostTemplate.SellingPrice.Amount == 0)
            {
                sellingText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
            if (string.IsNullOrWhiteSpace(CostTemplate.Description))
            {
                detailsText.Text = "Describa el producto...";
                detailsText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }
        

        private void FilterSupplies(object sender, RoutedEventArgs e)
        {
            Supplies.Clear();
            var text = searchSuplyText.Text != "Filtrar Insumos..." ? searchSuplyText.Text : "";
            foreach (var supply in _context.LocalSupplies)
            {
                if (PassTheFilter(supply, text, selectedFilterBox.SelectedIndex))
                    Supplies.Add(supply);
            }
        }
        private bool PassTheFilter(Supply supply, string text, int option)
        {
            return supply.Name.ToLower().Contains(text.ToLower()) && supply.FormatAmount.Amount != 0 &&
                   ((option == 0 && (supply.Template == null || !supply.Template.FinishedTemplate)) ||
                    (option == 1 && !supply.IsTemplate) ||
                    (option == 2 && supply.IsTemplate && !supply.Template.FinishedTemplate));
        }

        private void DescribeSupply(object sender, RoutedEventArgs e)
        {
            if (suppliesListView.SelectedIndex < 0)
                supplyDescriptionText.Text = "Descripción: ";
            else
            {
                var description = ((Supply)suppliesListView.Items[suppliesListView.SelectedIndex]).Description;
                supplyDescriptionText.Text = "Descripción: " + (string.IsNullOrWhiteSpace(description) ? "(sin descripción)" : description);
            }
        }

        private void SuppliesDoubleClick(object sender, RoutedEventArgs e)
        {
            var supply = ((ListViewItem)sender).Content as Supply;
            if (_selectedSupplies.Any(sa => sa.Supply == supply)) return;
            _selectedSupplies.Add(new SupplyAmount(supply, new Measurement(0, supply.FormatAmount.Unit), new Measurement(0, supply.FormatAmount.Unit), false));
            CalculateTotalCost(sender, e);
        }

        private void RemoveAll(object sender, RoutedEventArgs e)
        {
            _selectedSupplies.Clear();
            CalculateTotalCost(sender, e);
            supplyCost.Text = "0";
        }

        private void SelectedSupplyContextMenu(object sender, RoutedEventArgs e)
        {
            if (_menuOpen) return;
            _menuOpen = true;
            var position = Mouse.GetPosition(this);
            var contextMenu = new SupplyContextMenu() { Name = "contextMenu", Height = 120, Width = 130, Margin = new Thickness(position.X, position.Y, 0, 0), VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left };
            contextMenu.LostFocus += DeleteContextMenu;
            contextMenu.MouseUp += SelectOption;
            ((Grid)Content).Children.Add(contextMenu);
            _selectedSupplyAmount = ((ListBoxItem)sender).Content as SupplyAmount;
        }

        public void DeleteContextMenu(object sender, RoutedEventArgs e)
        {
            var gridChildren = ((Grid)Content).Children;
            gridChildren.RemoveAt(gridChildren.Count - 1);
        }

        private void SelectOption(object sender, RoutedEventArgs e)
        {
            var gridChildren = ((Grid)Content).Children;
            var contextMenu = gridChildren[gridChildren.Count - 1];
            var selectedOption = ((SupplyContextMenu)contextMenu).SelectedIndex;
            switch (selectedOption)
            {
                case 0:
                    _selectedSupplies.Remove(_selectedSupplyAmount);
                    CalculateTotalCost(sender, e);
                    supplyCost.Text = "0";
                    break;
                case 1:
                    _selectedSupplyAmount.NonDefaultMeasuremnt = !_selectedSupplyAmount.NonDefaultMeasuremnt;
                    break;
                default:
                    _selectedSupplyAmount = null;
                    break;
            }
            DeleteContextMenu(sender, e);
            _menuOpen = false;
        }

        private void GoIndex(object sender, RoutedEventArgs e)
        {
            var templatesUserControl = (IndexTemplates)this.Find("templatesUserControl");
            templatesUserControl.Visibility = Visibility.Visible;
            this.AutoRemove();
        }
        private void SaveCostTemplate(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(CostTemplate.Name))
            {
                MessageBox.Show("Debe asignar un nombre a la ficha antes de crearla.", "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            if (produced_amountText.Text == "0")
            {
                var dependencies = CheckDependencies(CostTemplate);
                if (dependencies.Count > 0)
                {
                    var text =
                        "Este insumo ya ha sido asignado a las siguientes fichas de costo por lo que no puede tener su cantidad de unidades producidas igual a 0.\nFichas de costo relacionadas:";
                    dependencies.ForEach(d => text += "\n\t" + "-" + d);
                    MessageBox.Show(text, "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                    return;   
                }
            }

            if (CostTemplate.CostTemplateId == 0)
            {
                CostTemplate.UnderlyingSupply.IsTemplate = true;
                _context.AddTemplate(CostTemplate);
                _context.AddSupplyToTemplate(CostTemplate, _selectedSupplies.ToList());
            }
            else
            {
                if (_backUpTemplate.SupplyAmounts != null)
                    _backUpTemplate.SupplyAmounts.Where(sa => !_selectedSupplies.Contains(sa)).Select(sa => sa.SupplyAmountId).ToList().ForEach(_context.RemoveSupplyAmount);
                //aqui si se crea una ficha de costo sin unidades producidas o insumos no me deja agregar insumos luego a esta ficha de costo tiene que ver con el this._backUpTemplate.SupplyAmounts != null 
                _context.AddSupplyToTemplate(CostTemplate, _selectedSupplies.Where(sa => _backUpTemplate.SupplyAmounts != null && !_backUpTemplate.SupplyAmounts.Contains(sa)).ToList());
                _backUpTemplate.Copy(CostTemplate);
                _context.UpdateTemplate(_backUpTemplate);
            }
            if (_backUpTemplate != null)
            {
                _backUpTemplate.ItemChange("Profit");
                _backUpTemplate.ItemChange("Percentage");
            }
            GoIndex(sender, e);
        }
        private List<string> CheckDependencies(CostTemplate template)
        {
            var dependencies = new List<string>();
            if (template.CostTemplateId == 0) return dependencies;
            dependencies.AddRange(from temp in _context.LocalTemplates where temp.SupplyAmounts != null && temp.SupplyAmounts.Any(sa => sa.Supply.SupplyId == template.CostTemplateId) select temp.Name);
            return dependencies;
        } 

        private void NewSupply(object sender, RoutedEventArgs e)
        {
            var blurEffect = new BlurEffect { Radius = 1, KernelType = KernelType.Box };
            Effect = blurEffect;

            var newSupply = new EditSupply(new SupplyVM(_context))
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            newSupply.Closed += CloseNewSupply;
            IsEnabled = false;
        }
        private void CloseNewSupply(object sender, EventArgs e)
        {
            Effect = null;
            IsEnabled = true;
        }

        private void CalculateTotalCost(object sender, EventArgs e)
        {
            var value = _selectedSupplies.TotalCost();
            suppliesCost.Text = (string) calculatedCostList.SelectedValue == "CUC"
                                    ? value.SmartString()
                                    : new Currency(value, CurrencyUnit.CUC).ToCUP().Amount.SmartString();
        }
        private void ResetText(object sender, EventArgs e)
        {
            var textbox = sender as TextBox;
            double value;
            if (string.IsNullOrWhiteSpace(textbox.Text) || !double.TryParse(textbox.Text, out value))
            {
                textbox.Text = "0";
                textbox.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
            
            CalculateTotalCost(sender, e);
        }
        private void CleanAmountText(object sender, EventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox.Text == "0")
            {
                textbox.Text = "";
            }
            textbox.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void RecalculateLocalCost(object sender, EventArgs e)
        {
            var selected = selectedSuppliesListBox.SelectedItem as SupplyAmount;
            if (selected == null) return;
            calculateLocalCostList.SelectedValue = selected.Cost.Unit.ToString();
            supplyCost.Text = selected.Cost.Amount.SmartString();
        }
        private void RecalculateUnit(object sender, EventArgs e)
        {
            var unit = (string)calculatedCostList.SelectedValue;
            var value = double.Parse(suppliesCost.Text);
            suppliesCost.Text = unit == "CUP"
                                    ? new Currency(value, CurrencyUnit.CUC).ToCUP().Amount.SmartString()
                                    : new Currency(value, CurrencyUnit.CUP).ToCUC().Amount.SmartString();
        }
        private void RecalculateLocalUnit(object sender, EventArgs e)
        {
            var unit = (string)calculateLocalCostList.SelectedValue;
            if (unit == null) return;
            var value = double.Parse(supplyCost.Text);
            supplyCost.Text = unit == "CUP"
                                    ? new Currency(value, CurrencyUnit.CUC).ToCUP().Amount.SmartString()
                                    : new Currency(value, CurrencyUnit.CUP).ToCUC().Amount.SmartString();
        }
        private void SetImage(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            // Configure open file dialog box
            dlg.CheckFileExists = true;
            dlg.Title = "Seleccione una imagen";
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".JPGE"; // Default file extension
            dlg.Filter = "(.JPG)|*.JPG|(.PNG)|*.PNG"; // Filter files by extension

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                CostTemplate.Image = filename;
                img.Source = new BitmapImage(new Uri(filename));
            }
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
            Comparison<Supply> comparer = null;

            switch (header)
            {
                case "Nombre":
                    comparer = (x, y) => x.ToString().CompareTo(y.ToString());
                    break;
                case "Cantidad del Formato":
                    comparer = (x, y) => x.FormatAmount.AmountInUnit.CompareTo(y.FormatAmount.AmountInUnit);
                    break;
                case "Costo del Formato":
                    comparer = (x, y) => x.FormatCost.ToCUC().CompareTo(y.FormatCost.ToCUC());
                    break;
            }

            if (comparer == null) return;
            var items = new List<Supply>(Supplies);
            Supplies.Clear();
            items.Sort(comparer);
            if (direction == ListSortDirection.Descending)
                items.Reverse();
            items.ForEach(s => Supplies.Add(s));

        }


        #endregion
    }
}
