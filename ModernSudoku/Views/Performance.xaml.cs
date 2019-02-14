// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using CommonControls.Common;
using ModernSudoku.ViewModels;

namespace ModernSudoku.Views
{
    /// <summary>
    /// Performance.xaml 的交互逻辑
    /// </summary>
    public partial class Performance : LayoutControl
    {
        public Performance()
        {
            InitializeComponent();
            this.DataContext = new PerformanceViewModel();
        }
    }
}
