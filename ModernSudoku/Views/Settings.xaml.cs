using CommonControls.Common;
using ModernSudoku.ViewModels;
using System.Windows.Controls;

namespace ModernSudoku.Views
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : LayoutControl
    {
        public Settings()
        {
            InitializeComponent();
            this.DataContext = new SettingsViewModel(this);
        }
    }
}
