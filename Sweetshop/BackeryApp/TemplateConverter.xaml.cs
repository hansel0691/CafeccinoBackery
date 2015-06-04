using System.Collections.ObjectModel;
using System.ComponentModel;
using BackeryApp.ClassUtils;
using SupplyStock;
using SupplyStock.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for TemplateConverter.xaml
    /// </summary>
    public partial class TemplateConverter : UserControl
    {
        #region Contructor

        public TemplateConverter()
        {
            InitializeComponent();            
        }
        public TemplateConverter(IEnumerable<CostTemplate> templates)
        {
            InitializeComponent();
            Templates = new ObservableCollection<CostTemplate>(templates);
            templateslist.ItemsSource = Templates.Where(t => t.FinishedTemplate);
            @from.DataContext = From;
            to.DataContext = To;

            searchSuplyText.GotFocus += SeachGotFocus;
            searchSuplyText.LostFocus += SeachLostFocus;
            searchSuplyText.TextChanged += FilterTemplates;
            changeFrom.Click += ChangeFrom;
            changeTo.Click += ChangeTo;
            cancelButton.Click += ExitToMain;
            convertBtn.Click += ConvertTo;
            fromCostBox.SelectionChanged += ReclaculateFromCost;
            fromProfitBox.SelectionChanged += ReclaculateFromProfit;
            toCostBox.SelectionChanged += ReclaculateToCost;
            toProfitBox.SelectionChanged +=  ReclaculateToProfit;
            format_amountText.TextChanged += RecalculateFromData;
            goMainMenu.Click += GoMain;
        }


        #endregion
        #region Properties

        public ObservableCollection<CostTemplate> Templates { get; set; }
        public ComparerObject From { get; set; }
        public ComparerObject To { get; set; }

        #endregion
        #region Methods

        private void RecalculateFromData(object sender, RoutedEventArgs e)
        {
            if (From == null)
                return;
            From.Amount = int.Parse(string.IsNullOrWhiteSpace(format_amountText.Text) ? "0" : format_amountText.Text);
        }
        private void SeachGotFocus(object sender, RoutedEventArgs e)
        {
            if (searchSuplyText.Text == "Filtrar Dulces...")
                searchSuplyText.Text = "";
            searchSuplyText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void SeachLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchSuplyText.Text))
                searchSuplyText.Text = "Filtrar Dulces...";
            searchSuplyText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        }
        private void FilterTemplates(object sender, RoutedEventArgs e)
        {
            templateslist.ItemsSource = searchSuplyText.Text != "Filtrar Dulces..." ? Templates.Where(s => s.Name.ToLower().Contains(searchSuplyText.Text.ToLower()) && s.FinishedTemplate) : Templates;
        }
        private void ChangeFrom(object sender, RoutedEventArgs e)
        {
            From = new ComparerObject(templateslist.SelectedItem as CostTemplate);
            from.DataContext = From;
        }
        private void ChangeTo(object sender, RoutedEventArgs e)
        {
            To = new ComparerObject(templateslist.SelectedItem as CostTemplate);
            to.DataContext = To;
        }
        private void ConvertTo(object sender, RoutedEventArgs e)
        {
            if (From == null)
            {
                MessageBox.Show("Debe seleccionar un elemento inicial para la comparación.");
                return;
            }
            if (To == null)
            {
                MessageBox.Show("Debe seleccionar un elemento final para la comparación.");
                return;
            }
            if (From != null && From.Profit.Amount <= 0)
            {
                MessageBox.Show(String.Format("Las ganancias de {0} no puede ser menor o igual a 0.",From.Name));
                return;
            }
            if (To != null && To.Profit.Amount < 0)
            {
                MessageBox.Show(String.Format("Las ganancias de {0} no puede ser menor o igual a 0.", To.Name));
                return;
            }
            To.Amount = (int)Math.Ceiling(From.Profit.AmountCUC * To.Template.ProducedUnits / To.Template.Profit.AmountCUC);
        }
        private void ExitToMain(object sender, RoutedEventArgs e)
        {
            var templatesUserControl = (StartUpControl)this.Find("homeUserControl");
            templatesUserControl.Visibility = Visibility.Visible;
            this.AutoRemove();
        }
        private void ReclaculateToProfit(object sender, RoutedEventArgs e)
        {
            To.Profit = Recalculate(To.Profit, ((ComboBox)sender).SelectedItem as string);
            toProfitsText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();

        }
        private void ReclaculateToCost(object sender, RoutedEventArgs e)
        {
            To.Cost = Recalculate(To.Cost, ((ComboBox)sender).SelectedItem as string);
            toProductionCostText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
        private void ReclaculateFromProfit(object sender, RoutedEventArgs e)
        {
            From.Profit = Recalculate(From.Profit, ((ComboBox)sender).SelectedItem as string);
            fromProfitsText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
        private void ReclaculateFromCost(object sender, RoutedEventArgs e)
        {
            From.Cost = Recalculate(From.Cost, ((ComboBox)sender).SelectedItem as string);
            fromProductionCostText.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
        private void GoMain(object sender, RoutedEventArgs e)
        {
            var home = this.Find("homeUserControl");
            home.Visibility = Visibility.Visible;
            this.AutoRemove();
        }



        private Currency Recalculate(Currency amount, string selection)
        {
            if (amount == null) return null;
            return selection == "CUC"
                                ? new Currency(amount.Amount, amount.Unit).ToCUC()
                                : new Currency(amount.Amount, amount.Unit).ToCUP();
        }

        #endregion
    }

    public class ComparerObject : INotifyPropertyChanged
    {
        #region Variables

        private int _units;

        #endregion
        #region Contructor

        public ComparerObject(CostTemplate template)
        {
            Template = template;
            _units = 0;
            Profit = new Currency();
            Cost = new Currency();
        }

        #endregion
        #region Properties

        public CostTemplate Template { get; set; }
        public string Name { get { return Template.ToString(); } }
        public int Amount
        {
            get { return _units; } 
            set
            {
                if (value == _units) return;
                _units = value;
                Cost = new Currency(_units * Template.CostPerUnit.Amount, Template.CostPerUnit.Unit);
                Profit = new Currency((Template.Profit.Amount / Template.ProducedUnits) * _units, Template.Profit.Unit);
                OnPropertyChange("Amount");
                OnPropertyChange("Cost");
                OnPropertyChange("Profit");
            }
        }
        public Currency Cost { get; set; }
        public Currency Profit { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Methods

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName ));
        }
        
        #endregion
    }
}
