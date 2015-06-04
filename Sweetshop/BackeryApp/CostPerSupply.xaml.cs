using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BackeryApp.ClassUtils;
using DataAccess;
using SupplyStock;
using SupplyStock.Utils;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for CostPerSupply.xaml
    /// </summary>
    public partial class CostPerSupply : UserControl
    {
        #region Variables

        private readonly MContext _context;
        private readonly CostTemplate _costTemplate;

        #endregion
        #region Contructor

        public CostPerSupply()
        {
            InitializeComponent();
        }
        public CostPerSupply(CostTemplate template, MContext context)
        {
            InitializeComponent();
            _context = context;
            _costTemplate = (CostTemplate)template.Clone();
            usedSuppliesList.ItemsSource = template.SupplyAmounts.Select(sa => sa.Supply);
            DataContext = _costTemplate;

            format_costList.SelectionChanged += RecalculateCost;
            sellingPrice.SelectionChanged += RecalculateSellingPrice;
            costPerUnit.SelectionChanged += RecalculateCostPerUnit;
            profitBox.SelectionChanged += RecalculateProfits;
            cancelButton.Click += GoBack;
            goMainMenu.Click += GoMain;
            usedSuppliesList.SelectionChanged += LoadComparative;
        }

        
        #endregion
        #region Methods

        private void RecalculateCost(object sender, RoutedEventArgs e)
        {
            var selection = format_costList.SelectedItem as string;
            Recalculate(templateCostText, _costTemplate.Cost, selection);
        }
        private void RecalculateCostPerUnit(object sender, RoutedEventArgs e)
        {
            var selection = costPerUnit.SelectedItem as string;
            Recalculate(costPerUnitText, _costTemplate.CostPerUnit, selection);
        }
        private void RecalculateSellingPrice(object sender, RoutedEventArgs e)
        {
            var selection = sellingPrice.SelectedItem as string;
            Recalculate(sellingPriceText, _costTemplate.SellingPrice, selection);
        }
        private void RecalculateProfits(object sender, RoutedEventArgs e)
        {
            var selection = profitBox.SelectedItem as string;
            Recalculate(profitText, _costTemplate.Profit, selection);
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            var index = this.Find("ViewCostTemplate");
            index.Visibility = Visibility.Visible;
            this.AutoRemove();
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
        private void LoadComparative(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UnregisterName("listComparer");
            }
            catch 
            {
            }
            var selectedSupply = usedSuppliesList.SelectedItem as Supply;
            if (selectedSupply == null) return;
            comparerGrid.Children.Clear();
            LoadComparer(selectedSupply);
        }
        private void LoadComparer(Supply supply)
        {
            var relatedSupplies = _context.LocalSupplies.Where(s => s.Name == supply.Name);
            double virtualMax = 0;
            if (relatedSupplies.Count() > 0)
                virtualMax = relatedSupplies.Max(s => s.CostPerUnit().AmountCUC);
            var tempText = new TextBlock { Text = "Insumos Relacionados...", FontSize = 13, FontStyle = FontStyles.Italic, Foreground = new SolidColorBrush(Color.FromRgb(79, 159, 207)), Height = 26, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(10, 10, 10, -10) };
            comparerGrid.RowDefinitions.Add(new RowDefinition(){Height = new GridLength(25)});
            comparerGrid.Children.Add(tempText);
            var listComparer = new ListBox() { Name = "listComparer", BorderBrush = new SolidColorBrush(Colors.Transparent), SelectionMode = SelectionMode.Single, Margin = new Thickness(0, 0, 0, 0) };
            listComparer.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            RegisterName(listComparer.Name, listComparer);
            
            comparerGrid.RowDefinitions.Add(new RowDefinition());
            Grid.SetRow(listComparer, 1);
            listComparer.SelectionChanged += SelectFormatList;

            comparerGrid.Children.Add(listComparer);
        }

        private void SelectFormatList(object sender, SelectionChangedEventArgs e)
        {
            var usedSupply = usedSuppliesList.SelectedItem as Supply;
            var listBox = FindName("listComparer") as ListBox;
            var supplyComparer = listBox.SelectedItem as SupplyComparer;
            var newSupply =  supplyComparer.Supply;
            var suppAmount = _costTemplate.SupplyAmounts.Find(sa => sa.Supply.ToString() == usedSupply.ToString());
            if (newSupply == null || usedSupply == null) return;

            _costTemplate.SupplyAmounts.Add(new SupplyAmount(newSupply, suppAmount.StartAmount, suppAmount.EndAmount, suppAmount.NonDefaultMeasuremnt));
            _costTemplate.SupplyAmounts.Remove(suppAmount);
            //this._costTemplate.ItemChange("Cost");
        }

        #endregion
    }
}
