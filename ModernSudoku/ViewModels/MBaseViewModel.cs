using CommonUtilities.MVVM;
using ModernSudoku.Model;
using ModernSudoku.Views;

namespace ModernSudoku.ViewModels
{
    public class MBaseViewModel : BaseViewModel
    {
        private Sudoku sudoku;

        /// <summary>
        /// Sudoku operations.
        /// </summary>
        public Sudoku Sudoku
        {
            get
            {
                if (this.sudoku == null)
                {
                    this.sudoku = new Sudoku();
                }

                return this.sudoku;
            }
            set { this.sudoku = value; }
        }

        /// <summary>
        /// Back command
        /// </summary>
        public UserCommand BackCommand { get; set; }

        public MBaseViewModel()
        {
            this.Initialize();
        }

        public MBaseViewModel(object window)
            : base(window)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.BackCommand = new UserCommand(ExecuteBackCommand);
        }

        /// <summary>
        /// Execute back command
        /// </summary>
        protected virtual void ExecuteBackCommand()
        {
            Utility.CloseProgress();
            Utility.Frame.Navigate(new CenterWindow());
        }
    }
}
