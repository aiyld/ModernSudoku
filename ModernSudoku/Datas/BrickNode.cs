// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using System.Collections.Generic;
using System.Linq;

namespace ModernSudoku.Datas
{
    public struct BrickNode
    {
        /// <summary>
        /// Number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Node kind.
        /// </summary>
        public BrickKind Kind { get; set; }

        /// <summary>
        /// Create empty general style sudoku.
        /// </summary>
        /// <returns>Empty sudoku.</returns>
        public static BrickNode[,] CreateGeneralSudoku()
        {
            BrickNode[,] nodes = new BrickNode[Constants.RowCount, Constants.ColumnCount];

            for (int i = 0; i < Constants.RowCount; i++)
            {
                for (int j = 0; j < Constants.ColumnCount; j++)
                {
                    nodes[i, j].Kind = BrickKind.General;
                }
            }

            return nodes;
        }

        /// <summary>
        /// Create empty none style sudoku.
        /// </summary>
        /// <returns>Empty sudoku.</returns>
        public static BrickNode[,] CreateNoneSudoku()
        {
            return new BrickNode[Constants.RowCount, Constants.ColumnCount];
        }

        /// <summary>
        /// Transform brick node collection, if node's value is bigger than 0, then make all nodes' kind to Original.
        /// </summary>
        /// <param name="source">Brick node collection.</param>
        /// <returns>Original kind Brick node collection.</returns>
        public static BrickNode[,] TransformOriginal(BrickNode[,] source)
        {
            BrickNode[,] nodes = new BrickNode[Constants.RowCount, Constants.ColumnCount];

            for (int i = 0; i < Constants.RowCount; i++)
            {
                for (int j = 0; j < Constants.ColumnCount; j++)
                {
                    if (source[i, j].Number > 0)
                    {
                        nodes[i, j].Kind = BrickKind.Original;
                    }
                    else
                    {
                        nodes[i, j].Kind = BrickKind.General;
                    }

                    nodes[i, j].Number = source[i, j].Number;
                }
            }

            return nodes;
        }
    }

    public static class BrickNodeExtension
    {
        /// <summary>
        /// Convert collection to sudoku.
        /// </summary>
        /// <param name="source">Collection souce.</param>
        /// <returns>A sudoku.</returns>
        public static BrickNode[,] ToSudoku(this IEnumerable<BrickNode> source)
        {
            BrickNode[,] nodes = null;

            if (source != null && source.Any())
            {
                nodes = new BrickNode[Constants.RowCount, Constants.ColumnCount];

                for (int i = 0; i < source.Count(); i++)
                {
                    if ((i + 1) > (Constants.RowCount * Constants.ColumnCount))
                    {
                        return nodes;
                    }
                    else
                    {
                        int row = i / Constants.RowCount;
                        int column = i - row * Constants.RowCount;
                        nodes[row, column] = source.ElementAt(i);
                    }
                }
            }

            return nodes;
        }

        /// <summary>
        /// Convert BrickNode[,] array to collection.
        /// </summary>
        /// <param name="source">BrickNode[,] array.</param>
        /// <returns>A list of BrickNode.</returns>
        public static List<BrickNode> ToList(this BrickNode[,] source)
        {
            List<BrickNode> nodes = new List<BrickNode>();

            if (source != null)
            {
                foreach (var item in source)
                {
                    nodes.Add(item);
                }
            }

            return nodes;
        }

        /// <summary>
        /// Check if BrickNode[,] collection has items changed.
        /// </summary>
        /// <param name="source">BrickNode[,] array.</param>
        /// <returns>True if changed, otherwise false.</returns>
        public static bool Changed(this BrickNode[,] source)
        {
            if (source == null)
            {
                return false;
            }

            foreach (var item in source)
            {
                if (item.Kind != BrickKind.Original && item.Number > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if BrickNode[,] collection has been resolved.
        /// </summary>
        /// <param name="source">BrickNode[,] array</param>
        /// <returns>True if resolved, otherwise false.</returns>
        public static bool Resolved(this BrickNode[,] source)
        {
            if (source == null)
            {
                return false;
            }

            foreach (var item in source)
            {
                if (item.Number == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Transform brick node collection, if node's value is bigger than 0, then make all nodes' kind to Original.
        /// </summary>
        /// <param name="source">Brick node collection.</param>
        /// <returns>Original kind Brick node collection.</returns>
        public static BrickNode[,] TransformOriginal(this BrickNode[,] source)
        {
            BrickNode[,] nodes = new BrickNode[Constants.RowCount, Constants.ColumnCount];

            for (int i = 0; i < Constants.RowCount; i++)
            {
                for (int j = 0; j < Constants.ColumnCount; j++)
                {
                    if (source[i, j].Number > 0)
                    {
                        nodes[i, j].Kind = BrickKind.Original;
                    }
                    else
                    {
                        nodes[i, j].Kind = BrickKind.General;
                    }

                    nodes[i, j].Number = source[i, j].Number;
                }
            }

            return nodes;
        }

    }
}
