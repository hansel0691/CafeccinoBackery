using System.Windows;
using SupplyStock.Utils;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        #region Variables

        private readonly SupplyStock.Utils.Options _options;

        #endregion
        #region Constructor

        public Options()
        {
            InitializeComponent();
            _options = (SupplyStock.Utils.Options)Settings.Options.Clone();
            currencyRatio.DataContext = _options;

            okBtn.Click += SaveChanges;
            cancelBtn.Click += CloseWindow;
        }

        #endregion
        #region Methods

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            Settings.Options.Copy(_options);
            _options.SaveOptions();
            Close();
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
    }
}
