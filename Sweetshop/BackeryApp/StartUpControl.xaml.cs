using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using BackeryApp.ClassUtils;
using BackeryApp.ViewModel;
using DataAccess;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for StartUpControl.xaml
    /// </summary>
    public partial class StartUpControl : UserControl
    {
        #region Variables

        private readonly MContext _context;

        #endregion
        #region Contructors

        public StartUpControl(MContext context)
        {
            InitializeComponent();

            _context = context;
            costTemplateBtn.Click += GoTemplates;
            suppliesBtn.Click += GoSuplies;
            convertionBtn.Click += NewTemplateConverter;
            ProductionBtn.Click += GoProductons;
            optionsButton.Click += ShowOptions;
        }

        #endregion
        #region Methods


        private void ShowOptions(object sender, RoutedEventArgs e)
        {
            var blurEffect = new BlurEffect {Radius = 1, KernelType = KernelType.Box};
            Effect = blurEffect;

            var optiosWindow = new Options()
                                {
                                    VerticalAlignment = VerticalAlignment.Center,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                                };
            optiosWindow.Show();
            optiosWindow.Closed += CloseNewWindow;
            IsEnabled = false;
        }
        private void CloseNewWindow(object sender, EventArgs e)
        {
            Effect = null;
            IsEnabled = true;
        }
        private void GoTemplates(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var templateControl = (IndexTemplates) this.Find("templatesUserControl");
            templateControl.Visibility = Visibility.Visible;
        }
        private void GoSuplies(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var suppliesControl = (IndexSupplies)this.Find("suppliesUserControl");
            //((IndexSupplies)suppliesControl).FilterSupplies(sender, e);
            suppliesControl.Visibility = Visibility.Visible;
        }
        private void GoProductons(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var productionsUserControl = new IndexProductions(new IndexProductionsVM(_context))
            {
                Name = "productionsUserControl",
                Width = double.NaN,
            };
            Grid.SetRow(productionsUserControl, 0);
            Grid.SetRowSpan(productionsUserControl, 2);
            var gridChild = ((Grid)Parent).Children;
            gridChild.Add(productionsUserControl);
        }
        private void NewTemplateConverter(object sender, RoutedEventArgs e)
        {
            var templateConverter = new TemplateConverter(_context.CostTemplates) { Width = double.NaN };
            Grid.SetRow(templateConverter, 0);
            Grid.SetRowSpan(templateConverter, 2);
            var gridChild = ((Grid)Parent).Children;
            Visibility = Visibility.Hidden;
            gridChild.Add(templateConverter);
        }
        

        #endregion
    }
}
