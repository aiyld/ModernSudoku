using CommonControls.Common;
using ModernSudoku.ViewModels;
using System.Windows.Controls;

namespace ModernSudoku.Views
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : LayoutControl
    {
        public About()
        {
            InitializeComponent();
            this.DataContext = new AboutViewModel();
        }
    }
}
