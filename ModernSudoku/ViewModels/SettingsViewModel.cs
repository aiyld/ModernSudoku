using CommonControls.Common;
using CommonUtilities.MVVM;
using ModernSudoku.Datas;
using ModernSudoku.Model;
using System;
using XmlOperation.LiDongYang;
using YldExtensions;

namespace ModernSudoku.ViewModels
{
    public class SettingsViewModel : MBaseViewModel
    {
        private AppSetting appSetting;

        /// <summary>
        /// Input name command.
        /// </summary>
        public UserCommand InputNameCommand { get; set; }

        /// <summary>
        /// Sudoku original count.
        /// </summary>
        public AppSetting AppSetting
        {
            get { return this.appSetting; }
            set { this.SetProperty(ref this.appSetting, value); }
        }

        public SettingsViewModel(object window)
            : base(window)
        {
            this.AppSetting = Model.AppSetting.Settings;
            this.InputNameCommand = new UserCommand(this.ExecuteInputNameCommand);
        }

        private void ExecuteInputNameCommand()
        {
            this.GetWindow<LayoutControl>().Dialog.Show((s) =>
            {
                if (!this.appSetting.HasUser())
                {
                    this.appSetting.SudokuUser = new SudokuUser();
                }

                this.appSetting.SudokuUser = new SudokuUser() { Name = s };
                this.RaisePropertyChanged("AppSetting");
                AppSetting.Save();
            }, Constants.InputUserName);
        }
    }
}
