using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BackeryApp.ViewModel;
using CheckBox = System.Windows.Controls.CheckBox;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for EditSupply.xaml
    /// </summary>
    public partial class EditSupply : Window
    {
        #region Variables

        private readonly SupplyVM _viewModel;

        #endregion
        #region Contructors

        public EditSupply()
        {
            InitializeComponent();
        }
        public EditSupply(SupplyVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            if (_viewModel.Supply.SupplyId == 0)
            {

                SaveButton.Content = "Crear...";
                Title = "Nuevo " + Title;
            }
            else
            {
                SaveButton.Content = "Salvar...";
                Title = "Editar " + Title;
                format_measurementList.ItemsSource = _viewModel.PosibleUnits();
            }
            

            InitImage();

            Loaded += InitControl;
            Closed += Cancel;
            
            SaveButton.Click += SaveSupply;
            cancelButton.Click += Cancel;
            frameImage.MouseUp += SetImage;

            nameText.GotFocus += CleanNameText;
            nameText.LostFocus += RestartNameText;
            format_amountText.GotFocus += AmountGotFocus;
            format_amountText.LostFocus += AmountLostFocus;
            format_costText.GotFocus += CostGotFocus;
            format_costText.LostFocus += CostLostFocus;
            detailsText.GotFocus += DetailsGotFocus;
            detailsText.LostFocus += DetailsLostFocus;

            if (_viewModel.IsTemplate)
            {
                format_measurementList.ItemsSource = new List<string> {"u"};
                InitIsTemplate();
            }
           
            
            Show();
        }

        #endregion
        #region Methods


        private void CleanNameText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_viewModel.Name) || _viewModel.Name == "Introduzca un nombre...")
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

        private void AmountGotFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Amount.Amount == 0)
                format_amountText.Text = "";
            format_amountText.Foreground = new SolidColorBrush(Color.FromRgb(33, 33, 33));
        }
        private void AmountLostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Amount.Amount == 0)
            {
                format_amountText.Text = "0";
                format_amountText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }

        private void CostGotFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Cost.AmountCUC == 0)
                format_costText.Text = "";
            format_costText.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
        }
        private void CostLostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Cost.AmountCUC == 0)
            {
                format_costText.Text = "0";
                format_costText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
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
                detailsText.Text = "Describa el insumo...";
                detailsText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }


        private void InitControl(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_viewModel.Name))
            {
                nameText.Text = "Introduzca un nombre...";
                nameText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
            if (Math.Abs(_viewModel.Amount.Amount) < 0.00000001)
            {
                format_amountText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
            if (Math.Abs(_viewModel.Cost.Amount) < 0.00000001)
            {
                format_costText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
            if (string.IsNullOrWhiteSpace(_viewModel.Description))
            {
                detailsText.Text = "Describa el insumo...";
                detailsText.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            }
        }
        private void SetImage(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                CheckFileExists = true,
                Title = "Seleccione una imagen",
                FileName = "Document",
                DefaultExt = ".JPGE",
                Filter = "(.JPG)|*.JPG|(.PNG)|*.PNG"
            };

            var result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                _viewModel.Image = filename;
                img.Source = new BitmapImage(new Uri(filename));
            }
        }
        private void InitImage()
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.Image))
            {
                if (!File.Exists(_viewModel.Image))
                {
                    MessageBox.Show(
                        "La ruta de la imagen asociada a esta ficha de costo a cambiado. Por favor vuelva a seleccionar una imagen.",
                        "Error mostrando imagen", MessageBoxButton.OK, MessageBoxImage.Error);
                    _viewModel.ResetImage();
                    return;
                }
                img.Source = new BitmapImage(new Uri(_viewModel.Image));
                frameImage.Background = new SolidColorBrush(Colors.White);
            }
        }
        private void InitIsTemplate()
        {
            var checkBox = new CheckBox()
            {
                IsChecked = true,
                IsEnabled = false,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(20, 0, 0, 0)
            };
            Grid.SetRow(checkBox, 3);
            Grid.SetColumnSpan(checkBox, 2);
           
            var textBlock = new TextBlock()
                               {
                                   Text = "Es una ficha de costo",
                                   Foreground = new SolidColorBrush(Color.FromRgb(55, 55, 55)),
                                   VerticalAlignment = VerticalAlignment.Center,
                                   HorizontalAlignment = HorizontalAlignment.Left,
                                   Margin = new Thickness(40, 0, 0, 0)
                               };
            Grid.SetRow(textBlock, 3);
            Grid.SetColumnSpan(textBlock, 2);
            content.Children.Add(checkBox);
            content.Children.Add(textBlock);
        }

        private void SaveSupply(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrWhiteSpace(_viewModel.Name))
            {
                MessageBox.Show("Debe asignar un nombre válido antes de crear un nuevo insumo.", "Error", MessageBoxButton.OK,
                               MessageBoxImage.Error);
                return;
            }
            if (Math.Abs(_viewModel.Amount.Amount) < 0.000001)
            {
                MessageBox.Show("Debe asignar una cantidad de formato válida antes de crear un nuevo insumo.", "Error", MessageBoxButton.OK,
                               MessageBoxImage.Error);
                return;
            }
            if (Math.Abs(_viewModel.Cost.AmountCUC) < 0.0000001 && !_viewModel.IsTemplate)
            {
                MessageBox.Show("Debe asignar un costo de formato válida antes de crear un nuevo insumo.", "Error", MessageBoxButton.OK,
                               MessageBoxImage.Error);
                return;
            }
            _viewModel.SaveSupply();
            Close();
        }
        private void Cancel(object sender, EventArgs e)
        {
            _viewModel.ResetToSupply();
            Close();
        }
        
      
        #endregion
    }
}
