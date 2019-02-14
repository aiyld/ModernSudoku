// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.

namespace ModernSudoku.Datas
{
    /// <summary>
    /// Indicates the kind of brick
    /// </summary>
    public enum BrickKind : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Original, cannot be changed.
        /// </summary>
        Original = 1,

        /// <summary>
        /// General, can be changed.
        /// </summary>
        General = 2,
    }
}
