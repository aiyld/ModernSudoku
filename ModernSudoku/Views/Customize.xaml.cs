using ModernSudoku.ViewModels;
using System.Windows.Controls;

namespace ModernSudoku.Views
{
    /// <summary>
    /// Customize.xaml 的交互逻辑
    /// </summary>
    public partial class Customize : UserControl
    {
        public Customize()
        {
            InitializeComponent();
            this.DataContext = new CustomizeViewModel();
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            //this.gamePanel.Focus();

            base.OnKeyDown(e);
        }

    }
}
