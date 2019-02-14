using ModernSudoku.Datas;
using ModernSudoku.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ModernSudoku.Controls
{
    /// <summary>
    /// Brick.xaml 的交互逻辑
    /// </summary>
    public partial class Brick : UserControl
    {
        private const string EmptyBrickText = "0";

        #region Dependency Properties

        #region ItemSource

        private static DependencyProperty ItemSourceProperty = DependencyProperty.Register(
            "ItemSource",
            typeof(ObservableCollection<int>),
            typeof(Brick),
            new PropertyMetadata(new PropertyChangedCallback(OnItemSourceChanged)));

        /// <summary>
        /// Item source
        /// </summary>
        public ObservableCollection<int> ItemSource
        {
            get { return (ObservableCollection<int>)this.GetValue(ItemSourceProperty); }
            set { this.SetValue(ItemSourceProperty, value); }
        }

        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                Brick brick = d as Brick;
                brick.ItemSource = e.NewValue as ObservableCollection<int>;
                brick.listViewNumbers.ItemsSource = brick.ItemSource;
            }
        }

        #endregion

        #region Text

        private static DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(Brick),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTextChanged)));

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                Brick brick = d as Brick;
                brick.Text = e.NewValue as string;
                brick.sNumber.Content = brick.Text.Equals(EmptyBrickText) ? string.Empty : brick.Text;
            }
        }

        #endregion

        #region Background color

        private static DependencyProperty BgColorProperty = DependencyProperty.Register(
            "BgColor",
            typeof(Color),
            typeof(Brick),
            new PropertyMetadata(Colors.LightYellow, new PropertyChangedCallback(OnBgColorChanged)));

        /// <summary>
        /// Background color
        /// </summary>
        public Color BgColor
        {
            get { return (Color)this.GetValue(BgColorProperty); }
            set { this.SetValue(BgColorProperty, value); }
        }

        private static void OnBgColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                Brick brick = d as Brick;
                brick.BgColor = e.NewValue == null ? Colors.White : (Color)e.NewValue;
                brick.Background = new SolidColorBrush(brick.BgColor);
            }
        }

        #endregion

        #region Event Command

        private static DependencyProperty EventCommandProperty = DependencyProperty.Register(
            "EventCommand",
            typeof(ICommand),
            typeof(Brick),
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
                Brick brick = d as Brick;
                brick.EventCommand = e.NewValue as ICommand;
            }
        }

        #endregion

        #region Command Parameter

        private static DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(Brick),
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
                Brick brick = d as Brick;
                brick.CommandParameter = e.NewValue;
            }
        }

        #endregion

        #region Brick Kind

        private static DependencyProperty BrickKindProperty = DependencyProperty.Register(
            "Kind",
            typeof(BrickKind),
            typeof(Brick),
            new PropertyMetadata(BrickKind.None, new PropertyChangedCallback(OnBrickKindChanged)));

        /// <summary>
        /// Brick Kind
        /// </summary>
        public BrickKind Kind
        {
            get { return (BrickKind)this.GetValue(BrickKindProperty); }
            set { this.SetValue(BrickKindProperty, value); }
        }

        private static void OnBrickKindChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                Brick brick = d as Brick;
                brick.Kind = e.NewValue == null ? BrickKind.None : (BrickKind)e.NewValue;

                Color color = Colors.PaleGreen;

                switch (brick.Kind)
                {
                    case BrickKind.None: color = Colors.SkyBlue; break;
                    case BrickKind.General: break;
                    case BrickKind.Original: color = Colors.Orange; break;
                }

                brick.border.Background = new SolidColorBrush(color);
            }
        }

        #endregion

        #region TextSize

        private static DependencyProperty TextSizeProperty = DependencyProperty.Register(
            "TextSize",
            typeof(double),
            typeof(Brick),
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
            if (!object.Equals(e.NewValue, e.OldValue))
            {
                Brick brick = d as Brick;
                brick.sNumber.FontSize = (double)e.NewValue;
            }
        }

        #endregion

        #endregion

        #region Constructs

        public Brick()
        {
            InitializeComponent();

            this.Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            this.listViewNumbers.SelectedIndex = -1;

            if (this.ItemSource != null && this.ItemSource.Any())
            {
                this.listViewNumbers.SelectedIndex = -1;
            }

            this.sNumber.Click += (s, e) =>
            {
                if (this.ItemSource != null && this.ItemSource.Count != 0)
                {
                    this.ShowPopup();
                }
            };

            this.popup.LostFocus += (s, e) =>
            {
                this.ClosePopup();
            };

            this.listViewNumbers.SelectionChanged += (s, e) =>
            {
                this.ClosePopup();

                if (this.listViewNumbers.SelectedIndex != -1)
                {
                    this.Text = this.listViewNumbers.SelectedItem.ToString();
                    this.InvokeCommand();
                    this.listViewNumbers.SelectedIndex = -1;
                }
            };
        }

        #region Popup

        /// <summary>
        /// Show popup
        /// </summary>
        private void ShowPopup()
        {
            if (AppSetting.Settings.PopupTips)
            {
                this.UpdatePopupPlacement();

                this.OpenPopup();
            }
        }

        /// <summary>
        /// Open popup
        /// </summary>
        private void OpenPopup()
        {
            if (this.Kind != BrickKind.Original && !this.popup.IsOpen)
            {
                this.popup.IsOpen = true;
            }
        }

        private void ClosePopup()
        {
            if (this.popup.IsOpen)
            {
                this.popup.IsOpen = false;
            }
        }

        /// <summary>
        /// Update popup placement according to the position
        /// </summary>
        private void UpdatePopupPlacement()
        {
            if (this.Kind == BrickKind.Original)
            {
                return;
            }

            try
            {
                Window window = Window.GetWindow(this);
                Point point = this.TransformToAncestor(window).Transform(new Point(0, 0));

                double left = point.X;
                double right = window.Width - point.X;
                double top = point.Y;
                double bottom = window.Height - point.Y;

                this.popup.Placement = PlacementMode.Bottom;
                this.popup.Margin = new Thickness(0, 0, 0, 0);

                if (left + this.popup.Width - window.Width > 10)
                {
                    if (window.Height - top - this.popup.Height < 10)
                    {
                        this.popup.Placement = PlacementMode.Top;
                    }
                    else
                    {
                        this.popup.Placement = PlacementMode.Bottom;
                    }
                }
                else if (top + this.popup.Height - window.Height > 10)
                {
                    this.popup.Placement = PlacementMode.Top;
                }
            }
            catch { }
        }

        #endregion

        /// <summary>
        /// Invoke binded command.
        /// </summary>
        private void InvokeCommand()
        {
            if (this.EventCommand != null && this.EventCommand.CanExecute(this.Text))
            {
                this.EventCommand.Execute(this.Text);
            }
        }

        #endregion

        #region Override methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (this.Kind != BrickKind.Original)
            {
                switch (e.Key)
                {
                    case Key.D1:
                    case Key.D2:
                    case Key.D3:
                    case Key.D4:
                    case Key.D5:
                    case Key.D6:
                    case Key.D7:
                    case Key.D8:
                    case Key.D9:
                    case Key.NumPad1:
                    case Key.NumPad2:
                    case Key.NumPad3:
                    case Key.NumPad4:
                    case Key.NumPad5:
                    case Key.NumPad6:
                    case Key.NumPad7:
                    case Key.NumPad8:
                    case Key.NumPad9:
                        {
                            int key = (int)e.Key;
                            int number = key < 74 ? key - 34 : key - 74;
                            if (this.ItemSource != null && this.ItemSource.Any(n => n == number))
                            {
                                this.Text = number.ToString();
                            }
                            else
                            {
                                Utility.ShowMessage(string.Format("According to the calculation, number {0} is not allowed to be filled in.", number), MessageKind.Warning);
                            }

                            this.ClosePopup();
                            this.InvokeCommand();
                        } break;
                    case Key.NumPad0:
                    case Key.Back:
                        {
                            this.Text = string.Empty;
                            this.ClosePopup();
                            this.InvokeCommand();
                        } break;
                    case Key.Enter:
                        {
                            if (this.ItemSource != null && this.ItemSource.Count != 0)
                            {
                                this.ShowPopup();
                            }
                        } break;
                    default:
                        {
                            this.ClosePopup();
                        }; break;
                }
            }
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.sNumber.BorderThickness = new Thickness(2);

            base.OnGotKeyboardFocus(e);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.sNumber.BorderThickness = new Thickness(0);

            base.OnLostKeyboardFocus(e);
        }

        #endregion
    }
}

