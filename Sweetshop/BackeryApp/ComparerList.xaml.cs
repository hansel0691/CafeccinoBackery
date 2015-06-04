using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SupplyStock;
using SupplyStock.Utils;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for ComparerList.xaml
    /// </summary>
    public partial class ComparerList : UserControl
    {
        #region Variables

        private Measurement _definedAmount;
        private IEnumerable<Supply> _relatedSupplies;
        private Supply _supply;

        #endregion
        #region Constructors

        public ComparerList()
        {
            InitializeComponent();
        }
        public ComparerList(Supply supply, IEnumerable<Supply> relatedSupplies, SelectionChangedEventHandler selectionList, RoutedEventHandler unselectList)
        {
            InitializeComponent();
            _supply = supply;
            _relatedSupplies = relatedSupplies;
                _definedAmount = new Measurement(1, supply.FormatAmount.Unit);
            definedAmountPanel.DataContext = _definedAmount;

            SelectFormatList = selectionList;
            UnselectList = unselectList;

            Loaded += LoadFormatComparer;
            inputAmountText.LostFocus += UpdateSupplyComparers;
            inputAmountBox.SelectionChanged += UpdateSupplyComparers;
            
        }

        
        #endregion
        #region Properties

        public Action<string> AddToolTip { get; set; }
        public Action RemoveToolTip { get; set; }
        public SelectionChangedEventHandler SelectFormatList { get; set; }
        public RoutedEventHandler UnselectList { get; set; }

        #endregion
        #region Methods

        private void LoadFormatComparer(object sender, RoutedEventArgs e)
        {
            var rowCount = 1;
            var virtualMax = _relatedSupplies.Any()
                                 ? Math.Max(_supply.CostPerUnit().AmountCUC,
                                            _relatedSupplies.Max(s => s.CostPerUnit().AmountCUC))
                                 : _supply.CostPerUnit().AmountCUC;
            var temp = new SupplyComparer(_supply, virtualMax, _supply.CostPerUnit(), _definedAmount, ShowToolTip, HideToolTip)
                           {
                               Name = "SelectedSupplyComparer",
                               Height = 60,
                               Width = double.NaN,
                               Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                               Margin = new Thickness(10, 10, 10, 0)
                           };
            RegisterName(temp.Name, temp);
            mainComparer.Children.Add(temp);
            Grid.SetRow(temp, rowCount++);

            if (_relatedSupplies.Any())
            {
                var tempText = new TextBlock { Text = "Insumos Relacionados...", FontSize = 13, FontStyle = FontStyles.Italic, Foreground = new SolidColorBrush(Color.FromRgb(79, 159, 207)), Height = 26, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(10, 15, 0, -5) };
                mainComparer.Children.Add(tempText);
                mainComparer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                Grid.SetRow(tempText, rowCount++);

                var listComparer = new ListBox() { Name = "listComparer", BorderBrush = new SolidColorBrush(Colors.Transparent), SelectionMode = SelectionMode.Single, Margin = new Thickness(0, 0, 0, 0) };
                listComparer.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                listComparer.LostFocus += UnselectList;
                listComparer.SelectionChanged += SelectFormatList;

                RegisterName(listComparer.Name, listComparer);
                mainComparer.RowDefinitions.Add(new RowDefinition());
                Grid.SetRow(listComparer, rowCount);

                foreach (var comp in _relatedSupplies.Select(supply =>
                    new SupplyComparer(supply, virtualMax, _supply.CostPerUnit(), _definedAmount, ShowToolTip, HideToolTip) 
                                    { 
                                        Height = 60, 
                                        Width = double.NaN, 
                                        Background = new SolidColorBrush(Colors.Transparent) 
                                    }))
                    listComparer.Items.Add(comp);
                mainComparer.Children.Add(listComparer);
            }
        }
        private void ShowToolTip(object sender, MouseEventArgs e)
        {
            var anonimusObj = ((dynamic) (sender as Rectangle).DataContext);
            var unitCost = anonimusObj.UnitCost;
            var supplyFormatAmount = anonimusObj.FormatAmount;

            var relatedUnits = new List<string>(supplyFormatAmount.RelatedUnits);
            var scale = new List<double>(supplyFormatAmount.GetScale());
            var output = new StringBuilder("Costos por unidades: \n");
            output.Append("\t" + relatedUnits.First() + " ... " + unitCost + "\n");

            for (int i = 0; i < relatedUnits.Count() - 1; i++)
            {
                var unit = relatedUnits[i + 1];
                unitCost *= (1 / scale[i]);
                output.Append("\t" + unit + " ... " + unitCost + "\n");
            }
            output.Append("\nDescripción: " + (string.IsNullOrWhiteSpace(_supply.Description) ? "(sin descripción)" : _supply.Description));
            AddToolTip(output.ToString());
        }
        private void HideToolTip(object sender, MouseEventArgs e)
        {
            RemoveToolTip();
        }
        private void UpdateSupplyComparers(object sender, RoutedEventArgs e)
        {
            var suppComp = FindName("SelectedSupplyComparer") as SupplyComparer;
            if (suppComp != null)
                suppComp.RecalculateCostPerUnit(sender, e);
            var listComp = FindName("listComparer") as ListBox;
            if (listComp != null)
                foreach (SupplyComparer comparer in listComp.Items)
                    comparer.RecalculateCostPerUnit(sender, e);
        }


        #endregion
    }
}
