// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using ModernSudoku.Datas;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Serialization;
using XmlOperation.LiDongYang;

namespace ModernSudoku.Model
{
    /// <summary>
    /// Performances root.
    /// </summary>
    public class PerformanceOperation
    {

        /// <summary>
        /// Get performance by file name.
        /// </summary>
        /// <param name="fileName">File Name.</param>
        /// <returns></returns>
        private static List<PerformanceModel> GetPerformances(string fileName)
        {
            List<PerformanceModel> results = XmlUtils.Get<List<PerformanceModel>>(fileName);

            return results == null ? new List<PerformanceModel>() : results;
        }

        /// <summary>
        /// Save performance.
        /// </summary>
        /// <param name="model"></param>
        private static void SavePerformance(string fileName, PerformanceModel model)
        {
            XmlEditor xmlEditor = new XmlEditor(XmlUtils.GetPath(fileName), Constants.PerformanceRootNodeName);

            xmlEditor.AddNode(Constants.PerformanceRootNodeName, XmlSerialization.Serialize(model));
        }

        /// <summary>
        /// Get general performance.
        /// </summary>
        /// <returns></returns>
        public static List<PerformanceModel> GetGeneral()
        {
            return GetPerformances(Constants.PerformanceGeneralFileName);
        }

        /// <summary>
        /// Get customize performance.
        /// </summary>
        /// <returns></returns>
        public static List<PerformanceModel> GetCustomize()
        {
            return GetPerformances(Constants.PerformanceCustomiseFileName);
        }

        /// <summary>
        /// Save general performance.
        /// </summary>
        /// <param name="model"></param>
        public static void SaveGeneral(PerformanceModel model)
        {
            SavePerformance(Constants.PerformanceGeneralFileName, model);
        }

        /// <summary>
        /// Save customize performance.
        /// </summary>
        /// <param name="model"></param>
        public static void SaveCustomise(PerformanceModel model)
        {
            SavePerformance(Constants.PerformanceCustomiseFileName, model);
        }
    }

    /// <summary>
    /// Performance data structure.
    /// </summary>
    public class PerformanceModel
    {
        private BrickNode[,] sudokuResult;

        /// <summary>
        /// The index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Player.
        /// </summary>
        public SudokuUser Player { get; set; }

        /// <summary>
        /// Game mode of this performance.
        /// </summary>
        public GameMode GameMode { get; set; }

        /// <summary>
        /// The start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The end time.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Call help times.
        /// </summary>
        public int CallHelpTimes { get; set; }

        /// <summary>
        /// Performance result.
        /// </summary>
        public List<BrickNode> Result { get; set; }

        /// <summary>
        /// Sudoku result.
        /// </summary>
        [XmlIgnore]
        public BrickNode[,] SudokuResult
        {
            get
            {
                if (this.sudokuResult == null && this.Result != null)
                {
                    this.sudokuResult = this.Result.ToSudoku();
                }

                return this.sudokuResult;
            }
            set
            {
                this.sudokuResult = value;
                this.Result = value.ToList();
            }
        }

        /// <summary>
        /// Cost time.
        /// </summary>
        [XmlIgnore]
        public TimeSpan CostTime { get; set; }

        /// <summary>
        /// Background.
        /// </summary>
        [XmlIgnore]
        public Color Background { get; set; }

        /// <summary>
        /// Image uri.
        /// </summary>
        [XmlIgnore]
        public Uri ImageUri { get; set; }
    }
}
