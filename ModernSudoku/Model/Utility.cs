// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using ModernSudoku.Datas;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ModernSudoku.Model
{
    /// <summary>
    /// The utilty for app. Including progress control, message toast etc.
    /// </summary>
    public class Utility
    {
        #region Fields

        private const double MessageTimeSpan = 3;
        private const double MessageDisappearTimeSpan = 60;
        private const double MessageDisappearOpacity = 0.05;

        private static Grid messageGrid;

        private static DispatcherTimer MessageTimer { get; set; }
        private static DispatcherTimer MessageDisappearTimer { get; set; }

        #endregion

        static Utility()
        {
            Utility.InitializeMessageTimer();
        }

        #region UI Properties

        /// <summary>
        /// Main frame for application.
        /// </summary>
        public static Frame Frame { get; set; }

        /// <summary>
        /// The progress grid.
        /// </summary>
        public static Grid ProgressGrid { get; set; }

        /// <summary>
        /// The grid for message.
        /// </summary>
        public static Grid MessageGrid
        {
            get { return Utility.messageGrid; }
            set
            {
                if (value != null)
                {
                    value.MouseEnter += (s, e) =>
                    {
                        Utility.Show(value);
                        Utility.StopMessageDisappearTimer();
                        Utility.MessageTimer.Stop();
                    };
                    value.MouseLeave += (s, e) =>
                    {
                        if (value.Visibility == Visibility.Visible)
                        {
                            Utility.Show(Utility.MessageGrid);
                        }
                    };
                }

                Utility.messageGrid = value;
            }
        }

        /// <summary>
        /// The text block for message body.
        /// </summary>
        public static TextBlock MessageBody { get; set; }

        /// <summary>
        /// The action button for message.
        /// </summary>
        public static Button MessageAction { get; set; }

        #endregion

        #region Message

        private static void InitializeMessageTimer()
        {
            Utility.MessageTimer = new DispatcherTimer();
            Utility.MessageTimer.Tick += (s, e) =>
            {
                Utility.StartMessageDisappearTimer();
                Utility.MessageTimer.Stop();
            };
            Utility.MessageTimer.Interval = TimeSpan.FromSeconds(Utility.MessageTimeSpan);

            Utility.MessageDisappearTimer = new DispatcherTimer();
            Utility.MessageDisappearTimer.Tick += MessageDisappearTimer_Tick;
            Utility.MessageDisappearTimer.Interval = TimeSpan.FromMilliseconds(Utility.MessageDisappearTimeSpan);
        }

        private static void MessageDisappearTimer_Tick(object sender, EventArgs e)
        {
            if (Utility.MessageGrid.Opacity >= 0)
            {
                Utility.MessageGrid.Opacity -= Utility.MessageDisappearOpacity;
            }
            else
            {
                Utility.StopMessageDisappearTimer();
                Utility.MessageGrid.Visibility = Visibility.Collapsed;
            }
        }

        private static void StartMessageDisappearTimer()
        {
            Utility.MessageDisappearTimer.Tick += Utility.MessageDisappearTimer_Tick;
            Utility.MessageDisappearTimer.Start();
        }

        private static void StopMessageDisappearTimer()
        {
            Utility.MessageDisappearTimer.Tick -= Utility.MessageDisappearTimer_Tick;
            Utility.MessageDisappearTimer.Stop();
        }

        private static void Show(Grid messageGrid)
        {
            messageGrid.Opacity = 1;
            messageGrid.Visibility = Visibility.Visible;
            Utility.StopMessageDisappearTimer();
            Utility.MessageTimer.Start();
        }

        #endregion

        #region Public Methods

        #region Message

        /// <summary>
        /// Show toast message.
        /// </summary>
        /// <param name="message">Message body.</param>
        /// <param name="kind">Message kind.</param>
        /// <param name="actionName">The content of response button.</param>
        /// <param name="action">The action for response button.</param>
        public static void ShowMessage(string message, MessageKind kind = MessageKind.General, string actionName = "", Action action = null)
        {
            Color color = Colors.Green;

            switch (kind)
            {
                case MessageKind.General: break;
                case MessageKind.Warning: color = Colors.Orange; break;
                case MessageKind.Error: color = Colors.Red; break;
            }

            Utility.MessageBody.Text = message ?? string.Empty;
            Utility.MessageGrid.Background = new SolidColorBrush(color);
            Utility.MessageGrid.Visibility = Visibility.Visible;
            Utility.MessageGrid.Opacity = 1;
            Utility.MessageGrid.MouseDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    Utility.MessageGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Utility.Show(Utility.MessageGrid);
                }
            };

            if (action != null)
            {
                Utility.MessageAction.Visibility = Visibility.Visible;
                Utility.MessageAction.Click += (s, e) =>
                {
                    action();
                    Utility.MessageGrid.Visibility = Visibility.Collapsed;
                };
                Utility.MessageAction.Content = actionName;
            }
            else
            {
                Utility.MessageAction.Visibility = Visibility.Collapsed;
            }

            Utility.MessageDisappearTimer.Stop();
            Utility.MessageTimer.Start();
        }

        #endregion

        #region Progress

        /// <summary>
        /// Open Progress.
        /// </summary>
        public static void OpenProgress()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Utility.ProgressGrid.Visibility = Visibility.Visible;
            });
        }

        /// <summary>
        /// Close progress.
        /// </summary>
        public static void CloseProgress()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Utility.ProgressGrid.Visibility = Visibility.Collapsed;
            });
        }

        #endregion

        #endregion
    }
}
