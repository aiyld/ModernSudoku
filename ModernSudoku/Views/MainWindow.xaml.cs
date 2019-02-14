using CommonControls.Common;
using ModernSudoku.Datas;
using ModernSudoku.Model;
using ModernSudoku.ViewModels;
using ModernSudoku.Views;
using System.Windows;
using XmlOperation.LiDongYang;

namespace ModernSudoku
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : LayoutControl
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Initialize();
            this.DataContext = new MainViewModel();
        }

        private void Initialize()
        {
            try
            {
                AppSetting.Settings = XmlSerialization.Deserialize<AppSetting>(AppSetting.GetPath());

                if (ModernSudoku.InternalService!=null && ModernSudoku.InternalService.HasUser())
                {
                    AppSetting.Settings.SudokuUser = new SudokuUser()
                    {
                        Name = ModernSudoku.InternalService.CurrentUser.Name,
                        Account = ModernSudoku.InternalService.CurrentUser.Account.ToString()
                    };
                }
            }
            catch
            {
                Utility.ShowMessage("Failed to retrieve settings.", MessageKind.Error);
            }

            Utility.Frame = this.frameMain;
            Utility.ProgressGrid = this.progressRing;
            Utility.MessageGrid = this.messageTile;
            Utility.MessageBody = this.messageBody;
            Utility.MessageAction = this.buttonAction;
            Utility.Frame.Navigate(new CenterWindow());

            this.frameMain.Navigated += (s, e) =>
            {
                if (this.frameMain.NavigationService.CanGoBack)
                {
                    this.frameMain.NavigationService.RemoveBackEntry();
                }
            };
        }

        public override bool Closing()
        {
            AppSetting.Save();

            return base.Closing();
        }

    }
}
