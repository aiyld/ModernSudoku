// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using CommonUtilities.Utilities;
using ModernSudoku.Datas;
using System.Collections.Generic;
using System.Xml.Serialization;
using XmlOperation.LiDongYang;
using YldExtensions;

namespace ModernSudoku.Model
{
    [XmlRoot]
    public class AppSetting : NotificationObject
    {
        private static AppSetting settings;
        private int originalCount;
        private bool referenceTips;
        private bool popupTips;
        private BrickNode[,] adventureSudoku;
        private BrickNode[,] customizeSudoku;
        private bool timerOn;
        private bool referenceAlwaysOneNine;

        /// <summary>
        /// Global setting value.
        /// </summary>
        public static AppSetting Settings
        {
            get
            {
                if (AppSetting.settings == null)
                {
                    AppSetting.settings = AppSetting.Create();
                }

                return AppSetting.settings;
            }
            set { AppSetting.settings = value; }
        }

        /// <summary>
        /// Sodoku User.
        /// </summary>
        [XmlElement]
        public SudokuUser SudokuUser { get; set; }

        /// <summary>
        /// Saved Adventure Sudoku.
        /// </summary>
        [XmlIgnore]
        public BrickNode[,] AdventureSudoku
        {
            get
            {
                if (this.adventureSudoku == null && this.AdventureCollection != null)
                {
                    this.adventureSudoku = this.AdventureCollection.ToSudoku();
                }

                return this.adventureSudoku;
            }
            set
            {
                this.adventureSudoku = value;
                this.AdventureCollection = value.ToList();
            }
        }

        /// <summary>
        /// Saved Customize Sudoku.
        /// </summary>
        [XmlIgnore]
        public BrickNode[,] CustomizeSudoku
        {
            get
            {
                if (this.customizeSudoku == null && this.CustomizeCollection != null)
                {
                    this.customizeSudoku = this.CustomizeCollection.ToSudoku();
                }

                return this.customizeSudoku;
            }
            set
            {
                this.customizeSudoku = value;
                this.CustomizeCollection = value.ToList();
            }
        }

        /// <summary>
        /// Adventure nodes collection.
        /// </summary>
        [XmlArrayItem]
        public List<BrickNode> AdventureCollection { get; set; }

        /// <summary>
        /// Customize nodes collection.
        /// </summary>
        [XmlArrayItem]
        public List<BrickNode> CustomizeCollection { get; set; }

        [XmlElement]
        /// <summary>
        /// Sudoku original count.
        /// </summary>
        public int OriginalCount
        {
            get { return this.originalCount; }
            set { this.SetProperty(ref this.originalCount, value); }
        }

        [XmlElement]
        /// <summary>
        /// Indicate if open reference tips.
        /// </summary>
        public bool ReferenceTips
        {
            get { return this.referenceTips; }
            set { this.SetProperty(ref this.referenceTips, value); }
        }

        /// <summary>
        /// Indicate if open popup tips.
        /// </summary>
        [XmlElement]
        public bool PopupTips
        {
            get { return this.popupTips; }
            set { this.SetProperty(ref this.popupTips, value); }
        }

        /// <summary>
        /// Indicate if show timer in game panel.
        /// </summary>
        [XmlElement]
        public bool TimerOn
        {
            get { return this.timerOn; }
            set { this.SetProperty(ref this.timerOn, value); }
        }

        /// <summary>
        /// Indicate if reference always show from 1 to 9.
        /// </summary>
        [XmlElement]
        public bool ReferenceAlwaysOneNine
        {
            get { return this.referenceAlwaysOneNine; }
            set { this.SetProperty(ref this.referenceAlwaysOneNine, value); }
        }

        /// <summary>
        /// Create new AppSetting data.
        /// </summary>
        /// <returns>AppSetting object.</returns>
        public static AppSetting Create()
        {
            return new AppSetting()
            {
                OriginalCount = 26,
                ReferenceTips = true,
                PopupTips = true,
                TimerOn = true,
                ReferenceAlwaysOneNine = true,
            };
        }

        /// <summary>
        /// Get setting path.
        /// </summary>
        /// <returns>Setting path</returns>
        public static string GetPath()
        {
            return ConfigureUtil.GetAppCurrentDirectory().AddDelimiter() + Constants.SettingFileName;
        }

        /// <summary>
        /// Save app settings.
        /// </summary>
        public static void Save()
        {
            XmlSerialization.Serialize(AppSetting.Settings, Model.AppSetting.GetPath());
        }

        /// <summary>
        /// Indicate if app has unfinished adventure sudoku.
        /// </summary>
        /// <returns>Return true if has, otherwise return false.</returns>
        public bool HasAdventureSudoku()
        {
            return this.AdventureSudoku != null;
        }

        /// <summary>
        /// Indicate if app has unfinished customize sudoku.
        /// </summary>
        /// <returns>Return true if has, otherwise return false.</returns>
        public bool HasCustomizeSudoku()
        {
            return this.CustomizeSudoku != null;
        }

        /// <summary>
        /// Has sodoku user.
        /// </summary>
        /// <returns></returns>
        public bool HasUser()
        {
            return this.SudokuUser != null &&
                (!string.IsNullOrEmpty(this.SudokuUser.Name) ||
                 !string.IsNullOrEmpty(this.SudokuUser.Account));
        }
    }
}
