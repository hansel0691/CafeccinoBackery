using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using BackeryApp.ViewModel;
using DataAccess;
using System.IO;                      //for : input - output
//For : OpenFileDialog / SaveFileDialog
//For : BitmapImage etc etc
using SupplyStock.Utils;
using Path = System.IO.Path;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables

        private MContext _context;

        #endregion
        #region Contructor

        public MainWindow()
        {
            InitializeComponent();
            Closed += DisposeResources;

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MContext>());
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");

            _context = new MContext();
            
            var statrUpControl = new StartUpControl(_context) { Name = "homeUserControl", Width = double.NaN };
            Grid.SetRow(statrUpControl, 0);
            Grid.SetRowSpan(statrUpControl, 2);
            container.Children.Add(statrUpControl);

            InitSettings();
            InitIndexTemplates();
            InitIndexSupplies();
        }

        #endregion
        #region Methods

        private void InitSettings()
        {
            SupplyStock.Utils.Options.Path = "app_config.stg";
            SupplyStock.Utils.Options options = null;
            if (!File.Exists(SupplyStock.Utils.Options.Path))
            {
                ShowErrorMessage();
                options = new SupplyStock.Utils.Options();
                options.SaveOptions();
            }
            else if (Path.GetExtension(SupplyStock.Utils.Options.Path) != ".stg")
            {
                ShowErrorMessage();
                options = new SupplyStock.Utils.Options();
                options.SaveOptions();
            }
            if (options == null)
            {
                var mySerializer = new XmlSerializer(typeof(SupplyStock.Utils.Options));
                using (var myFileStream = new FileStream(SupplyStock.Utils.Options.Path, FileMode.Open))
                    options = (SupplyStock.Utils.Options)mySerializer.Deserialize(myFileStream);
            }
            Settings.Options = options;
        }

        private static void ShowErrorMessage()
        {
            MessageBox.Show(
                    "Ha ocurrido un error al cargar el archivo de configuración. Será aplicada la configuración por defecto de su aplicación",
                    "Archivo de Configuración Dañado", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void InitIndexTemplates()
        {
            var templatesUserControl = new IndexTemplates(_context)
                                           {
                                               Name = "templatesUserControl",
                                               Width = double.NaN,
                                               Visibility = Visibility.Hidden
                                           };
            Grid.SetRowSpan(templatesUserControl, 2);
            container.Children.Add(templatesUserControl);
        }

        private void InitIndexSupplies()
        {
            var suppliesUserControl = new IndexSupplies(new IndexSuppliesVM(_context))
                                          {
                                              Name = "suppliesUserControl",
                                              Width = double.NaN,
                                              Visibility = Visibility.Hidden
                                          };
            Grid.SetRowSpan(suppliesUserControl, 2);
            container.Children.Add(suppliesUserControl);
        }

        private void DisposeResources(object sender, EventArgs e)
        {
            _context = null;
        }

        #endregion
    }
}
