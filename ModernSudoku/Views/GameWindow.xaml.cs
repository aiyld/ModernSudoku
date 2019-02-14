using ModernSudoku.ViewModels;
using System.Windows.Controls;

namespace ModernSudoku.Views
{
    /// <summary>
    /// GameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GameWindow : UserControl
    {
        public GameWindow()
        {
            InitializeComponent();
            this.DataContext = new GameViewModel();
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            //this.gamePanel.Focus();

            base.OnKeyDown(e);
        }
    }
}
