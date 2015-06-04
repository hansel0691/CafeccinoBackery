using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BackeryApp.ClassUtils;
using BackeryApp.ViewModel;
using SupplyStock;
using SupplyStock.Utils;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for EditProduction.xaml
    /// </summary>
    public partial class EditProduction : UserControl
    {
        #region Variables

        private readonly ProductionVM _viewModel;

        #endregion
        #region Constructors

        public EditProduction()
        {
            InitializeComponent();
        }
        public EditProduction(ProductionVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            Loaded += InitControl;
            nameText.GotFocus += CleanNameText;
            nameText.LostFocus += RestartNameText;
            SearchProductionText.GotFocus += SeachGotFocus;
            SearchProductionText.LostFocus += SeachLostFocus;
            SearchProductionText.TextChanged += Filter;
            detailsText.GotFocus += DetailsGotFocus;
            detailsText.LostFocus += DetailsLostFocus;
            SaveButton.Click += SaveProduction;
            CancelButton.Click += Cancel;
            removeFromSelected.Click += RemoveAll;
            selectedSuppliesListBox.SelectionChanged += SetSelectedCost;
            TemplatesListView.SelectionChanged += DescribeSupply;
            calculatedCostList.SelectionChanged += ConvertCost;
            calculateLocalCostList.SelectionChanged += ConvertSelectedCost;
        }

        private void ConvertSelectedCost(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedCost = calculateLocalCostList.SelectedIndex == 0 ? _viewModel.SelectedCost.ToCUC() : _viewModel.SelectedCost.ToCUP();
        }

        private void ConvertCost(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.Cost = calculatedCostList.SelectedIndex == 0 ? _viewModel.Cost.ToCUC() : _viewModel.Cost.ToCUP();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            GoBack();
            _viewModel.ResetToProduction(true);
        }

        private void RemoveAll(object sender, RoutedEventArgs e)
        {
            _viewModel.ClearUsedTemplates();
        }

        private void SaveProduction(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveProduction();
            GoBack();
        }

        private void Filter(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchPattern = SearchProductionText.Text == "Filtrar Dulces..." ? "" : SearchProductionText.Text;
            _viewModel.Filter();
        }

        #endregion
        #region Methods

        #region GUI Methods

        private void CleanNameText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_viewModel.Name))
                nameText.Text = "";
            nameText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void RestartNameText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_viewModel.Name))
            {
                nameText.Text = "Introduzca un nombre...";
                nameText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }

        private void SeachGotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchProductionText.Text == "Filtrar Dulces...")
                SearchProductionText.Text = "";
            SearchProductionText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void SeachLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchProductionText.Text))
                SearchProductionText.Text = "Filtrar Dulces...";
            SearchProductionText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        }

        private void DetailsGotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_viewModel.Description))
                detailsText.Text = "";
            detailsText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void DetailsLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_viewModel.Description))
            {
                detailsText.Text = "Describa la producción...";
                detailsText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }

        #endregion

        private void InitControl(object sender, RoutedEventArgs routedEventArgs)
        {
            DataContext = _viewModel;
            if (_viewModel.Production.ProductionId == 0)
            {
                headeTitle.Text = "Nueva " + headeTitle.Text;
                SaveButton.Content = "Crear...";
            }
            else
            {
                headeTitle.Text = "Editar " + headeTitle.Text;
                SaveButton.Content = "Editar...";
            }
            if (string.IsNullOrWhiteSpace(_viewModel.Name))
            {
                nameText.Text = "Introduzca un nombre...";
                nameText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
            if (string.IsNullOrWhiteSpace(_viewModel.Description))
            {
                detailsText.Text = "Describa la producción...";
                detailsText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }
        private void GoBack()
        {
            var templatesUserControl = (IndexProductions)this.Find("productionsUserControl");
            templatesUserControl.Visibility = Visibility.Visible;
            this.AutoRemove();
        }

        #endregion

        private void DescribeSupply(object sender, SelectionChangedEventArgs e)
        {
            if (TemplatesListView.SelectedIndex < 0)
                TemplateDescriptionText.Text = "Descripción: ";
            else
            {
                var description = ((CostTemplate)TemplatesListView.Items[TemplatesListView.SelectedIndex]).Description;
                TemplateDescriptionText.Text = "Descripción: " + (string.IsNullOrWhiteSpace(description) ? "(sin descripción)" : description);
            }
        }

        private void AddTemplateToProduction(object sender, MouseButtonEventArgs e)
        {
            var template = ((ListViewItem)sender).Content as CostTemplate;
            _viewModel.AddTemplate(template);
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
            _viewModel.RecalculateTotalCost();
            SetSelectedCost(sender, e);
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

        private void SetSelectedCost(object sender, EventArgs e)
        {
            var ta = selectedSuppliesListBox.SelectedItem as TemplateAmount;
            if (ta == null) return;
            _viewModel.SelectedCost = ta.Cost;
        }
    }
}
