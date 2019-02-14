// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using CommonUtilities.Utilities;
using XmlOperation.LiDongYang;
using YldExtensions;

namespace ModernSudoku.Model
{
    /// <summary>
    /// The opeartions for save files.
    /// </summary>
    public class XmlUtils
    {
        /// <summary>
        /// Get setting path.
        /// </summary>
        /// <returns>Setting path</returns>
        public static string GetPath(string fileName)
        {
            return ConfigureUtil.GetAppCurrentDirectory().AddDelimiter() + fileName;
        }

        /// <summary>
        /// Save data.
        /// </summary>
        public static void Save<T>(T data, string fileName)
        {
            XmlSerialization.Serialize(data, GetPath(fileName));
        }

        /// <summary>
        /// Get Data.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="fileName">The storage file name.</param>
        /// <returns>Data result.</returns>
        public static T Get<T>(string fileName)
        {
            return XmlSerialization.Deserialize<T>(GetPath(fileName));
        }
    }
}
