// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.

namespace ModernSudoku.Datas
{
    public struct MsPoint
    {
        /// <summary>
        /// Sudoku X
        /// 0-8
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Dudoku Y
        /// 0-8
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Equal override.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(MsPoint a, MsPoint b)
        {
            bool isEqual = false;

            if (a == null && b == null)
            {
                isEqual = true;
            }
            else if (a != null && b != null)
            {
                isEqual = (a.X == b.X && a.Y == b.Y);
            }

            return isEqual;
        }

        /// <summary>
        /// Not equal override.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(MsPoint a, MsPoint b)
        {
            bool isEqual = !(a == b);

            return isEqual;
        }

    }
}
