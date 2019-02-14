// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using CommonControls.Windows.Timers;
using CommonUtilities.MVVM;
using ModernSudoku.Datas;
using ModernSudoku.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ModernSudoku.ViewModels
{
    public class CustomizeViewModel : MBaseViewModel
    {
        private const int OriginalCount = 26;
        private const int NewGameSpan = 500;

        private BrickNode[,] riddle;
        private string numberReferences;
        private int remainderCount;
        private bool loadFinished;
        private bool isOKEnable;
        private DispatcherTimer timer;
        private ObservableCollection<int> brickSource;
        private bool isAnswerEnable;
        private CalculagraphState timerState;
        private DateTime startTime;
        private DateTime endTime;
        private int callHelpTimes;

        /// <summary>
        /// Riddle.
        /// </summary>
        public BrickNode[,] Riddle
        {
            get { return this.riddle; }
            set
            {
                this.SetProperty(ref this.riddle, value);
                this.RemainderCount = Model.Sudoku.RemainderCount(this.Riddle);
            }
        }

        /// <summary>
        /// Number references.
        /// </summary>
        public string NumberReferences
        {
            get { return this.numberReferences; }
            set { this.SetProperty(ref this.numberReferences, value); }
        }

        /// <summary>
        /// Item source
        /// </summary>
        public ObservableCollection<int> BrickSource
        {
            get { return this.brickSource; }
            set { this.SetProperty(ref this.brickSource, value); }
        }

        /// <summary>
        /// Remainder Count.
        /// </summary>
        public int RemainderCount
        {
            get { return this.remainderCount; }
            set { this.SetProperty(ref this.remainderCount, value); }
        }

        /// <summary>
        /// Indicate if load finished.
        /// </summary>
        public bool LoadFinished
        {
            get { return this.loadFinished; }
            set { this.SetProperty(ref this.loadFinished, value); }
        }

        /// <summary>
        /// Indicate if ok button enable.
        /// </summary>
        public bool IsOKEnable
        {
            get { return this.isOKEnable; }
            set { this.SetProperty(ref this.isOKEnable, value); }
        }

        /// <summary>
        /// Indicate if answer button enable.
        /// </summary>
        public bool IsAnswerEnable
        {
            get { return this.isAnswerEnable; }
            set { this.SetProperty(ref this.isAnswerEnable, value); }
        }

        /// <summary>
        /// Timer state.
        /// </summary>
        public CalculagraphState TimerState
        {
            get { return this.timerState; }
            set { this.SetProperty(ref this.timerState, value); }
        }


        /// <summary>
        /// Number change command
        /// </summary>
        public UserCommand NumChangeCommand { get; set; }

        /// <summary>
        /// New game command
        /// </summary>
        public UserCommand NewGameCommand { get; set; }

        /// <summary>
        /// OK command
        /// </summary>
        public UserCommand OKCommand { get; set; }

        /// <summary>
        /// Answer command.
        /// </summary>
        public UserCommand AnswerCommand { get; set; }

        public CustomizeViewModel()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.NumChangeCommand = new UserCommand(this.ExecuteNumChangeCommand);
            this.NewGameCommand = new UserCommand(this.ExecuteNewGameCommand);
            this.AnswerCommand = new UserCommand(this.ExecuteAnswerCommand);
            this.OKCommand = new UserCommand(this.ExecuteOKCommand);
            this.IsOKEnable = true;
            this.isAnswerEnable = false;
            this.startTime = DateTime.Now;
            this.endTime = DateTime.MinValue;

            if (AppSetting.Settings.HasCustomizeSudoku())
            {
                this.Riddle = AppSetting.Settings.CustomizeSudoku;
            }
            else
            {
                this.Riddle = BrickNode.CreateNoneSudoku();
            }

            this.LoadFinished = true;

            timer = new DispatcherTimer();
            timer.Tick += (s, e) =>
            {

            };
            timer.Interval = TimeSpan.FromMilliseconds(NewGameSpan);
        }

        private void ExecuteOKCommand()
        {
            this.Riddle = BrickNode.TransformOriginal(this.Riddle);
            this.IsAnswerEnable = true;
            this.TimerState = CalculagraphState.Start;
            this.startTime = DateTime.Now;
        }

        private void ExecuteNumChangeCommand()
        {
            this.RemainderCount = Model.Sudoku.RemainderCount(this.Riddle);

            if (this.RemainderCount == 0)
            {
                Utility.ShowMessage("Congratulations! You have completed this sudoku.",
                    MessageKind.General,
                    "Start New",
                    this.StartNewGame);

                this.TimerState = CalculagraphState.Stop;

                this.endTime = DateTime.Now;
                PerformanceOperation.SaveCustomise(
                    new PerformanceModel()
                    {
                        CallHelpTimes = this.callHelpTimes,
                        EndTime = this.endTime,
                        StartTime = this.startTime,
                        GameMode = GameMode.Customize,
                        SudokuResult = this.Riddle,
                        Player = AppSetting.Settings.SudokuUser,
                    });
            }
        }

        private void ExecuteNewGameCommand()
        {
            if (this.Sudoku.IsCaculating||this.Riddle.Changed())
            {
                Utility.ShowMessage("Are you sure to customize new sudoku?",
                    MessageKind.General,
                    "Start New",
                    () =>
                    {
                        this.Sudoku.Stop = true;
                        Utility.CloseProgress();
                        this.StartNewGame();
                    });
            }
            else
            {
                this.StartNewGame();
            }
        }

        private async void ExecuteAnswerCommand()
        {
            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.TimerState = CalculagraphState.Stop;
                    this.IsAnswerEnable = false;
                    this.IsOKEnable = false;
                    this.LoadFinished = false;
                    this.Sudoku.Stop = false;
                    Utility.OpenProgress();
                });

                this.Sudoku.Calculate(this.Riddle);

                this.Dispatcher.Invoke(() =>
                {
                    if (this.Sudoku.GotAnswer)
                    {
                        this.Riddle = this.Sudoku.Answer;
                    }
                    else if (this.Sudoku.Stop)
                    {
                        Utility.ShowMessage("Sudoku caculating stopped.", MessageKind.Warning);
                    }
                    else
                    {
                        Utility.ShowMessage("Sudoku has no answer.", MessageKind.Warning);
                    }

                    this.LoadFinished = true;
                    Utility.CloseProgress();
                });
            });
        }

        private void StartNewGame()
        {
            //Create new and empty sudoku.
            this.Riddle = BrickNode.CreateNoneSudoku();
            this.IsOKEnable = true;
            this.IsAnswerEnable = false;
            this.TimerState = CalculagraphState.Stop;
            this.TimerState = CalculagraphState.Reset;
            this.startTime = DateTime.Now;
            this.endTime = DateTime.MinValue;
        }

        protected override void ExecuteBackCommand()
        {
            AppSetting.Settings.CustomizeSudoku = this.Riddle.Changed() && !this.Riddle.Resolved() ?
                this.Riddle : null;

            base.ExecuteBackCommand();
        }
    }
}
