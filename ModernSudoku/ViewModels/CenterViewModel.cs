using CommonControls.Common;
using CommonUtilities.MVVM;
using ModernSudoku.Datas;
using ModernSudoku.Model;
using ModernSudoku.Views;
using System;

namespace ModernSudoku.ViewModels
{
    public class CenterViewModel : MBaseViewModel
    {
        #region Properties

        /// <summary>
        /// New game command
        /// </summary>
        public UserCommand NewGameCommand { get; set; }

        /// <summary>
        /// Customize command
        /// </summary>
        public UserCommand CustomizeCommand { get; set; }

        /// <summary>
        /// Settings command.
        /// </summary>
        public UserCommand SettingsCommand { get; set; }

        /// <summary>
        /// About command.
        /// </summary>
        public UserCommand AboutCommand { get; set; }

        /// <summary>
        /// Performance Command.
        /// </summary>
        public UserCommand PerformanceCommand { get; set; }

        #endregion

        public CenterViewModel(object window)
            : base(window)
        {
            this.NewGameCommand = new UserCommand(this.ExecuteNewGameCommand);
            this.CustomizeCommand = new UserCommand(this.ExecuteCustomizeCommand);
            this.SettingsCommand = new UserCommand(this.ExecuteSettingsCommand);
            this.AboutCommand = new UserCommand(this.ExecuteAboutCommand);
            this.PerformanceCommand = new UserCommand(this.ExecutePerformanceCommand);
        }

        #region Methods

        private void ExecuteNewGameCommand()
        {
            this.GoToGame<GameWindow>();
        }

        private void ExecuteCustomizeCommand()
        {
            this.GoToGame<Customize>();
        }

        private void ExecuteSettingsCommand()
        {
            Utility.Frame.Navigate(new Settings());
        }

        private void ExecuteAboutCommand()
        {
            Utility.Frame.Navigate(new About());
        }

        private void ExecutePerformanceCommand()
        {
            Utility.Frame.Navigate(new Performance());
        }

        private void GoToGame<T>()
        {
            if (!AppSetting.Settings.HasUser())
            {
                if (ModernSudoku.InternalService != null && ModernSudoku.InternalService.HasUser())
                {
                    AppSetting.Settings.SudokuUser = new SudokuUser()
                    {
                        Name = ModernSudoku.InternalService.CurrentUser.Name,
                        Account = ModernSudoku.InternalService.CurrentUser.Account.ToString()
                    };

                    Utility.Frame.Navigate(Activator.CreateInstance<T>());
                }
                else
                {
                    this.GetWindow<LayoutControl>().Dialog.Show((s) =>
                    {
                        AppSetting.Settings.SudokuUser = new SudokuUser() { Name = s };
                        Utility.Frame.Navigate(Activator.CreateInstance<T>());
                    }, Constants.InputUserName);
                }
            }
            else
            {
                Utility.Frame.Navigate(Activator.CreateInstance<T>());
            }
        }

        #endregion
    }
}
