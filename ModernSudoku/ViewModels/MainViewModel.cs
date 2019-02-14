using ModernSudoku.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModernSudoku.ViewModels
{
    public class MainViewModel
    {
        private static bool IsAddedExit;

        public MainViewModel()
        {
            if (!MainViewModel.IsAddedExit)
            {
                Application.Current.Exit += Current_Exit;
                MainViewModel.IsAddedExit = true;
            }
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            AppSetting.Save();
        }
    }
}
