// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using ModernSudoku.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernSudoku.Model
{
    public class Sudoku
    {
        #region Properties

        /// <summary>
        /// Indicate if current sudoku has answer.
        /// </summary>
        public bool GotAnswer { get; private set; }

        /// <summary>
        /// Calculate count
        /// </summary>
        public int CalculateCount { get; private set; }

        /// <summary>
        /// Sudoku riddle.
        /// </summary>
        public BrickNode[,] Riddle { get; set; }

        /// <summary>
        /// Sudoku answer.
        /// </summary>
        public BrickNode[,] Answer { get; set; }

        /// <summary>
        /// Stop random and caculate.
        /// </summary>
        public bool Stop { get; set; }

        /// <summary>
        /// Indicate if soduku is caculating.
        /// </summary>
        public bool IsCaculating { get; private set; }

        #endregion

        public Sudoku()
        {
        }

        #region Public mehtods

        /// <summary>
        /// Calculate sudoku.
        /// </summary>
        public void Calculate(BrickNode[,] sudoku)
        {
            this.Calculate(sudoku, true);
        }

        /// <summary>
        /// Calculate sudoku.
        /// </summary>
        public void Calculate(BrickNode[,] sudoku, bool newCaculate)
        {
            this.IsCaculating = true;
            this.Stop = newCaculate ? false : this.Stop;
            this.Riddle = sudoku;
            this.GotAnswer = false;
            this.CalculateCount = 0;
            this.Calculating(sudoku, new MsPoint { });
            this.IsCaculating = false;
        }

        /// <summary>
        /// Get a random sudoku. The result would put in Riddle.
        /// </summary>
        /// <param name="originalCount"></param>
        public async Task<BrickNode[,]> RandomSudoku(int originalCount)
        {
            this.Stop = false;

            return await this.Random(originalCount);
        }

        /// <summary>
        /// Verify sudoku.
        /// </summary>
        /// <param name="sudoku">Input sudoku.</param>
        /// <returns>Return true if sudoku correct, otherwise false.</returns>
        public bool Verify(BrickNode[,] sudoku)
        {
            if (sudoku == null)
            {
                return false;
            }

            for (int i = 0; i <= 8; i++)
            {
                List<int> rowNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                List<int> columnNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                for (int j = 0; j <= 8; j++)
                {
                    if (rowNumbers.Contains(sudoku[i, j].Number))
                    {
                        rowNumbers.Remove(sudoku[i, j].Number);
                    }

                    if (columnNumbers.Contains(sudoku[j, i].Number))
                    {
                        columnNumbers.Remove(sudoku[j, i].Number);
                    }
                }

                if (rowNumbers.Any() || columnNumbers.Any())
                {
                    return false;
                }
            }



            for (int r = 0; r <= 6; r += 3)
            {
                for (int c = 0; c <= 6; c += 3)
                {
                    List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                    for (int i = r; i <= r + 2; i++)
                    {
                        for (int j = c; j <= c + 2; j++)
                        {
                            if (numbers.Contains(sudoku[i, j].Number))
                            {
                                numbers.Remove(sudoku[i, j].Number);
                            }
                        }
                    }

                    if (numbers.Any())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Get available BrickNodes with known x and y for sudoku.
        /// </summary>
        /// <returns></returns>
        public static List<BrickNode> GetAvailableBrickNodes(BrickNode[,] sudoku, MsPoint point)
        {
            List<BrickNode> availableBrickNodes = new List<BrickNode>();

            if (sudoku == null || sudoku[point.X, point.Y].Kind == BrickKind.Original)
            {
                return availableBrickNodes;
            }

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i <= 8; i++)
            {
                if (numbers.Contains(sudoku[i, point.Y].Number))
                {
                    numbers.Remove(sudoku[i, point.Y].Number);
                }
            }

            for (int i = 0; i <= 8; i++)
            {
                if (numbers.Contains(sudoku[point.X, i].Number))
                {
                    numbers.Remove(sudoku[point.X, i].Number);
                }
            }

            int minX = (point.X / 3) * 3, minY = (point.Y / 3) * 3;

            for (int i = minX; i <= minX + 2; i++)
            {
                for (int j = minY; j <= minY + 2; j++)
                {
                    if (numbers.Contains(sudoku[i, j].Number))
                    {
                        numbers.Remove(sudoku[i, j].Number);
                    }
                }
            }

            foreach (int number in numbers)
            {
                availableBrickNodes.Add(new BrickNode { Number = number, Kind = BrickKind.General });
            }

            return availableBrickNodes;
        }

        /// <summary>
        /// Get next point.
        /// </summary>
        public static MsPoint GetNextPoint(BrickNode[,] sudokuint, MsPoint point)
        {
            MsPoint newPoint = new MsPoint() { X = point.X, Y = point.Y };

            if (newPoint.X == 8 && newPoint.Y == 8)
            {
                return newPoint;
            }

            if (newPoint.Y == 8)
            {
                newPoint.X++;
                newPoint.Y = 0;
            }
            else
            {
                newPoint.Y++;
            }

            if (sudokuint[newPoint.X, newPoint.Y].Kind == BrickKind.Original)
            {
                return Sudoku.GetNextPoint(sudokuint, newPoint);
            }
            else
            {
                return newPoint;
            }
        }

        /// <summary>
        /// Remainder count
        /// </summary>
        public static int RemainderCount(BrickNode[,] sudokuint)
        {
            int count = 0;

            if (sudokuint != null)
            {
                foreach (var node in sudokuint)
                {
                    if (node.Number == 0)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Process for calculating.
        /// </summary>
        private void Calculating(BrickNode[,] sudokuParam, MsPoint point)
        {
            if (this.GotAnswer || this.Stop)
            {
                return;
            }

            this.CalculateCount++;

            BrickNode[,] sudoku = sudokuParam.Clone() as BrickNode[,];

            if (point.X == 8 && point.Y == 8 && sudoku[point.X, point.Y].Number != 0)
            {
                this.Answer = sudoku;
                this.GotAnswer = true;

                return;
            }

            if (sudoku[point.X, point.Y].Kind == BrickKind.Original)
            {
                this.Calculating(sudoku, Sudoku.GetNextPoint(sudoku, point));
            }
            else
            {
                List<BrickNode> BrickNodes = Sudoku.GetAvailableBrickNodes(sudoku, point);

                if (BrickNodes.Count == 0)
                {
                    return;
                }

                foreach (BrickNode BrickNode in BrickNodes)
                {
                    sudoku[point.X, point.Y] = BrickNode;

                    if (point.X == 8 && point.Y == 8 && sudoku[8, 8].Number != 0)
                    {
                        this.Answer = sudoku;
                        this.GotAnswer = true;

                        return;
                    }
                    //Debug.WriteLine(point.X + "," + point.Y + "  " + BrickNode.Number);
                    this.Calculating(sudoku, Sudoku.GetNextPoint(sudoku, point));
                }
            }
        }

        /// <summary>
        /// Get a random sudoku. The result would put in Riddle.
        /// </summary>
        /// <param name="originalCount"></param>
        private async Task<BrickNode[,]> Random(int originalCount)
        {
            return await Task.Factory.StartNew<BrickNode[,]>(() =>
            {
                BrickNode[,] sudoku = BrickNode.CreateGeneralSudoku();

                if (this.Stop)
                {
                    return BrickNode.CreateGeneralSudoku();
                }

                for (int i = 0; i <= 8; i++)
                {
                    for (int j = 0; j <= 8; j++)
                    {
                        sudoku[i, j].Number = 0;
                        sudoku[i, j].Kind = BrickKind.General;

                    }
                }

                Random random = new Random(1);

                for (int i = 0; i < originalCount; i++)
                {
                    List<BrickNode> list = new List<BrickNode>();
                    int x = 0;
                    int y = 0;

                    while (list.Count == 0)
                    {
                        x = random.Next(8) * DateTime.Now.Millisecond % 9;
                        y = random.Next(8) * DateTime.Now.Millisecond % 9;

                        list = Sudoku.GetAvailableBrickNodes(sudoku, new MsPoint { X = x, Y = y });
                    }

                    sudoku[x, y].Number = list[random.Next(9) * DateTime.Now.Millisecond % list.Count].Number;
                    sudoku[x, y].Kind = BrickKind.Original;
                }

                this.Calculate(sudoku);

                if (!this.GotAnswer && !this.Stop)
                {
                    return this.Riddle = this.RandomSudoku(originalCount).Result;
                }
                else
                {
                    return this.Riddle = sudoku;
                }
            });
        }

        #endregion

    }
}
