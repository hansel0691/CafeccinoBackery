using System.Windows;
using System.Windows.Controls;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for SupplyContextMenu.xaml
    /// </summary>
    public partial class SupplyContextMenu : Grid
    {
        public SupplyContextMenu()
        {
            InitializeComponent();
            topPath.MouseUp += SelectTop;
            TopPathText.MouseUp += SelectTop;
            centerPath.MouseUp += SelectCenter;
            centerPathText.MouseUp += SelectCenter;
            bottomPath.MouseUp += SelectBottom;
            bottomPathText.MouseUp += SelectBottom;
        }

        #region Properties

        public uint? SelectedIndex { get; set; }

        #endregion
        #region Methods

        private void SelectTop(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 0;
        }
        private void SelectCenter(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 1;
        }
        private void SelectBottom(object sender, RoutedEventArgs e)
        {
            SelectedIndex = 2;
        }

        #endregion
    }
}
