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
    public class GameViewModel : MBaseViewModel
    {
        private const int NewGameSpan = 500;

        private BrickNode[,] riddle;
        private string numberReferences;
        private int remainderCount;
        private bool loadFinished;
        private bool isNewGameEnable;
        private DispatcherTimer timer;
        private int OriginalCount;
        private ObservableCollection<int> brickSource;
        private CalculagraphState timerState;
        private bool isAnswerEnable;
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
        /// Timer state.
        /// </summary>
        public CalculagraphState TimerState
        {
            get { return this.timerState; }
            set { this.SetProperty(ref this.timerState, value); }
        }

        /// <summary>
        /// Indicate if new game button enable.
        /// </summary>
        public bool IsNewGameEnable
        {
            get { return this.isNewGameEnable; }
            set { this.SetProperty(ref this.isNewGameEnable, value); }
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
        /// Number change command
        /// </summary>
        public UserCommand NumChangeCommand { get; set; }

        /// <summary>
        /// New game command
        /// </summary>
        public UserCommand NewGameCommand { get; set; }

        /// <summary>
        /// Answer command.
        /// </summary>
        public UserCommand AnswerCommand { get; set; }

        public GameViewModel()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.OriginalCount = AppSetting.Settings.OriginalCount;

            this.NumChangeCommand = new UserCommand(this.ExecuteNumChangeCommand);
            this.NewGameCommand = new UserCommand(this.ExecuteNewGameCommand);
            this.AnswerCommand = new UserCommand(this.ExecuteAnswerCommand);
            this.IsNewGameEnable = true;
            this.IsAnswerEnable = false;
            this.startTime = DateTime.Now;
            this.endTime = DateTime.MinValue;

            if (AppSetting.Settings.HasAdventureSudoku())
            {
                this.LoadFinished = true;
                this.Riddle = AppSetting.Settings.AdventureSudoku;
                this.TimerState = CalculagraphState.Start;
                this.IsAnswerEnable = true;
            }
            else
            {
                this.CreateRandomSudokuTask().RunSynchronously();
            }
            timer = new DispatcherTimer();
            timer.Tick += (s, e) =>
            {
                if (this.LoadFinished)
                {
                    this.IsNewGameEnable = true;
                    this.IsAnswerEnable = false;
                    timer.Stop();
                    this.CreateRandomSudokuTask().RunSynchronously();
                }
            };
            timer.Interval = TimeSpan.FromMilliseconds(NewGameSpan);
        }

        private void ExecuteNumChangeCommand()
        {
            this.RemainderCount = Model.Sudoku.RemainderCount(this.Riddle);

            if (this.RemainderCount == 0)
            {
                this.TimerState = CalculagraphState.Stop;

                Utility.ShowMessage("Congratulations! You have completed this sudoku.",
                    MessageKind.General,
                    "Start New",
                    this.ExecuteNewGameCommand);

                this.endTime = DateTime.Now;
                this.LoadFinished = false;
                PerformanceOperation.SaveGeneral(
                    new PerformanceModel()
                    {
                        CallHelpTimes = this.callHelpTimes,
                        EndTime = this.endTime,
                        StartTime = this.startTime,
                        GameMode = GameMode.General,
                        SudokuResult = this.Riddle,
                        Player = AppSetting.Settings.SudokuUser,
                    });
            }
        }

        private void ExecuteNewGameCommand()
        {
            this.IsAnswerEnable = false;
            this.IsNewGameEnable = false;
            this.Sudoku.Stop = true;
            this.TimerState = CalculagraphState.Reset;

            timer.Start();
        }

        private void ExecuteAnswerCommand()
        {
            this.TimerState = CalculagraphState.Stop;

            if (this.Sudoku.Answer == null || !this.Riddle.Resolved())
            {
                this.Sudoku.Calculate(this.Riddle);
            }

            this.Riddle = this.Sudoku.Answer;
        }

        /// <summary>
        /// Random sudoku.
        /// </summary>
        /// <returns></returns>
        private Task<BrickNode[,]> RandomSudokuTask()
        {
            return this.Sudoku.RandomSudoku(this.OriginalCount);
        }

        /// <summary>
        /// Create random sudoku.
        /// </summary>
        private Task CreateRandomSudokuTask()
        {
            return new Task(async () =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    Utility.ShowMessage("Sudoku is initializing, wait for a moment please...", MessageKind.Warning);
                    this.LoadFinished = false;
                    this.Sudoku.Stop = false;
                    Utility.OpenProgress();
                });

                var result = await this.RandomSudokuTask();

                this.Dispatcher.Invoke(() =>
                {
                    if (this.Sudoku.Stop)
                    {
                        Utility.ShowMessage("Old initializing stoped!");
                    }
                    else
                    {
                        Utility.ShowMessage("Initialize successfully!");
                    }
                    this.Riddle = result;
                    this.LoadFinished = true;
                    this.IsAnswerEnable = true;
                    this.TimerState = CalculagraphState.Start;
                    Utility.CloseProgress();
                    this.startTime = DateTime.Now;
                    this.endTime = DateTime.MinValue;
                });
            });
        }

        protected override void ExecuteBackCommand()
        {
            AppSetting.Settings.AdventureSudoku = this.Riddle.Changed() && !this.Riddle.Resolved() ?
                this.Riddle : null;

            base.ExecuteBackCommand();
        }
    }
}
