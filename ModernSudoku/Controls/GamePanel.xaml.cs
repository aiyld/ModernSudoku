using CommonUtilities.MVVM;
using ModernSudoku.Datas;
using ModernSudoku.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModernSudoku.Controls
{
    /// <summary>
    /// GamePanel.xaml 的交互逻辑
    /// </summary>
    public partial class GamePanel : UserControl
    {
        private const double BrickMarinValue = 0.6;

        #region Dependency Properties

        #region ItemSource

        private static DependencyProperty ItemSourceProperty = DependencyProperty.Register(
            "ItemSource",
            typeof(object),
            typeof(GamePanel),
            new PropertyMetadata(BrickNode.CreateGeneralSudoku(), new PropertyChangedCallback(OnItemSourceChanged)));

        /// <summary>
        /// Item source
        /// </summary>
        public object ItemSource
        {
            get { return this.GetValue(ItemSourceProperty); }
            set { this.SetValue(ItemSourceProperty, value); }
        }

        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                GamePanel gamePanel = d as GamePanel;
                gamePanel.ItemSource = e.NewValue;
                gamePanel.Refresh();
            }
        }

        #endregion

        #region BrickSource

        private static DependencyProperty BrickSourceProperty = DependencyProperty.Register(
            "BrickSource",
            typeof(ObservableCollection<int>),
            typeof(GamePanel),
            new PropertyMetadata(new PropertyChangedCallback(OnBrickSourceChanged)));

        /// <summary>
        /// Item source
        /// </summary>
        public ObservableCollection<int> BrickSource
        {
            get { return (ObservableCollection<int>)this.GetValue(BrickSourceProperty); }
            set { this.SetValue(BrickSourceProperty, value); }
        }

        private static void OnBrickSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                GamePanel gamePanel = d as GamePanel;
                gamePanel.BrickSource = e.NewValue as ObservableCollection<int>;
            }
        }

        #endregion

        #region Event Command

        private static DependencyProperty EventCommandProperty = DependencyProperty.Register(
            "EventCommand",
            typeof(ICommand),
            typeof(GamePanel),
            new PropertyMetadata(new PropertyChangedCallback(OnEventCommandChanged)));

        /// <summary>
        /// Event Command
        /// </summary>
        public ICommand EventCommand
        {
            get { return (ICommand)this.GetValue(EventCommandProperty); }
            set { this.SetValue(EventCommandProperty, value); }
        }

        private static void OnEventCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                GamePanel gamePanel = d as GamePanel;
                gamePanel.EventCommand = (ICommand)e.NewValue;
            }
        }

        #endregion

        #region Command Parameter

        private static DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(GamePanel),
            new PropertyMetadata(new PropertyChangedCallback(OnCommandParameterChanged)));

        /// <summary>
        /// Command Parameter
        /// </summary>
        public object CommandParameter
        {
            get { return (object)this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                GamePanel gamePanel = d as GamePanel;
                gamePanel.CommandParameter = e.NewValue;
            }
        }

        #endregion

        #region Current Point

        private static DependencyProperty MsPointProperty = DependencyProperty.Register(
            "MsPoint",
            typeof(MsPoint),
            typeof(GamePanel),
            new PropertyMetadata(new PropertyChangedCallback(OnMsPointChanged)));

        /// <summary>
        /// MsPoint
        /// </summary>
        public MsPoint MsPoint
        {
            get { return (MsPoint)this.GetValue(MsPointProperty); }
            set { this.SetValue(MsPointProperty, value); }
        }

        private static void OnMsPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                GamePanel gamePanel = d as GamePanel;
                gamePanel.MsPoint = (MsPoint)e.NewValue;
            }
        }

        #endregion

        #region Steps

        private static DependencyProperty StepsProperty = DependencyProperty.Register(
            "Steps",
            typeof(Stack<MsStep>),
            typeof(GamePanel),
            new PropertyMetadata(new Stack<MsStep>(), new PropertyChangedCallback(OnStepsChanged)));

        /// <summary>
        /// Steps
        /// </summary>
        public Stack<MsStep> Steps
        {
            get { return (Stack<MsStep>)this.GetValue(StepsProperty); }
            set { this.SetValue(StepsProperty, value); }
        }

        private static void OnStepsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                GamePanel gamePanel = d as GamePanel;
                gamePanel.Steps = (Stack<MsStep>)e.NewValue;
            }
        }

        #endregion

        #region TextSize

        private static DependencyProperty TextSizeProperty = DependencyProperty.Register(
            "TextSize",
            typeof(double),
            typeof(GamePanel),
            new PropertyMetadata(20d, new PropertyChangedCallback(OnTextSizeChanged)));

        /// <summary>
        /// Text Size
        /// </summary>
        public double TextSize
        {
            get { return (double)this.GetValue(TextSizeProperty); }
            set { this.SetValue(TextSizeProperty, value); }
        }

        private static void OnTextSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region Visible

        private static DependencyProperty VisibleProperty = DependencyProperty.Register(
            "Visible",
            typeof(bool),
            typeof(GamePanel),
            new PropertyMetadata(true, new PropertyChangedCallback(OnVisibleChanged)));

        /// <summary>
        /// Visible
        /// </summary>
        public bool Visible
        {
            get { return (bool)this.GetValue(TextSizeProperty); }
            set { this.SetValue(TextSizeProperty, value); }
        }

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                bool newValue = (bool)e.NewValue;
                GamePanel gamePanel = d as GamePanel;

                gamePanel.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Sudoku nodes from item source.
        /// </summary>
        public BrickNode[,] SudokuNodes
        {
            get
            {
                return this.ItemSource as BrickNode[,];
            }
        }

        public GamePanel()
        {
            InitializeComponent();

            this.InitializeBricks();
        }

        #region Methods

        private void InitializeBricks()
        {
            // Initialize rows.
            for (int i = 0; i < Constants.RowCount; i++)
            {
                this.gridBricks.RowDefinitions.Add(new RowDefinition());
            }
            // Initialize columns.
            for (int i = 0; i < Constants.ColumnCount; i++)
            {
                this.gridBricks.ColumnDefinitions.Add(new ColumnDefinition());
            }

            BrickNode[,] nodes = this.SudokuNodes;

            // Initialize items.
            for (int i = 0; i < Constants.RowCount; i++)
            {
                for (int j = 0; j < Constants.ColumnCount; j++)
                {
                    Brick brick = new Brick();
                    brick.Text = nodes[i, j].Number.ToString();
                    brick.Kind = nodes[i, j].Kind;
                    brick.TextSize = this.TextSize;
                    brick.Margin = new Thickness(BrickMarinValue, BrickMarinValue, BrickMarinValue, BrickMarinValue);
                    brick.sNumber.Click += (s, e) =>
                    {
                        this.UpdateBrickSource(brick);
                    };
                    brick.EventCommand = new UserCommand((s) =>
                    {
                        int number = (s == null || string.IsNullOrWhiteSpace(s.ToString()) ?
                            0 : int.Parse(brick.Text));

                        this.SudokuNodes[Grid.GetRow(brick), Grid.GetColumn(brick)].Number = number;
                        this.UpdateBrickSource(brick);

                        if (number == 0)
                        {
                            // Remove step
                            this.RemoveStep();
                        }
                        else
                        {
                            // Add one step
                            this.AddStep(new MsStep
                            {
                                Point = this.MsPoint,
                                Node = new BrickNode { Number = number, Kind = brick.Kind }
                            });
                        }

                        this.InvokeCommand(s);
                    });

                    brick.GotFocus += (s, e) =>
                    {
                        this.UpdateBrickSource(brick);
                    };

                    Grid.SetRow(brick, i);
                    Grid.SetColumn(brick, j);

                    this.gridBricks.Children.Add(brick);
                }
            }

            Keyboard.Focus(this.gridBricks.Children[0]);
        }

        /// <summary>
        /// Refresh numbers in panel.
        /// </summary>
        private void Refresh()
        {
            if (this.ItemSource != null)
            {
                for (int i = 0; i < Constants.RowCount; i++)
                {
                    for (int j = 0; j < Constants.ColumnCount; j++)
                    {
                        Brick brick = this.gridBricks.Children[i * Constants.RowCount + j] as Brick;
                        brick.Text = this.SudokuNodes[i, j].Number.ToString();
                        brick.Kind = this.SudokuNodes[i, j].Kind;
                    }
                }
            }
        }

        /// <summary>
        /// Invoke event command.
        /// </summary>
        /// <param name="parameter"></param>
        private void InvokeCommand(object parameter)
        {
            if (this.EventCommand != null)
            {
                this.EventCommand.Execute(parameter);
            }
        }

        /// <summary>
        /// Add step
        /// </summary>
        /// <param name="point"></param>
        private void AddStep(MsStep step)
        {
            if (this.Steps == null)
            {
                this.Steps = new Stack<MsStep>();
            }

            this.Steps.Push(step);
        }

        /// <summary>
        /// Remove step
        /// </summary>
        /// <param name="point"></param>
        private void RemoveStep()
        {
            if (this.Steps.Count != 0)
            {
                this.Steps.Pop();
            }

            if (this.Steps.Count != 0)
            {
                Brick brick = this.gridBricks.Children[this.Steps.Peek().Point.X * Constants.RowCount + this.Steps.Peek().Point.Y] as Brick;
                Keyboard.Focus(brick);
            }
        }

        /// <summary>
        /// Update BrickSource for brick.
        /// </summary>
        private void UpdateBrickSource(Brick brick)
        {
            this.BrickSource = new ObservableCollection<int>(
                Sudoku.GetAvailableBrickNodes(
                this.SudokuNodes,
                this.MsPoint = new MsPoint
                {
                    X = Grid.GetRow(brick),
                    Y = Grid.GetColumn(brick)
                }).Select(c => c.Number));

            if (brick.ItemSource == null ||
                !(brick.ItemSource.Any(c => this.BrickSource.Any(b => b == c)) &&
                brick.ItemSource.Count == this.BrickSource.Count))
            {
                brick.ItemSource = this.BrickSource;
            }
        }

        #endregion
    }
}

