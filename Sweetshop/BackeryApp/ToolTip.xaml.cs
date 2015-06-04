using System.Windows.Controls;

namespace BackeryApp
{
    /// <summary>
    /// Interaction logic for ToolTip.xaml
    /// </summary>
    public partial class ToolTip : Grid
    {
        #region Constructors

        public ToolTip()
        {
            InitializeComponent();
        }
        public ToolTip(string text)
        {
            InitializeComponent();
            infoTextBlock.Text = text;
        }

        #endregion
    }
}
