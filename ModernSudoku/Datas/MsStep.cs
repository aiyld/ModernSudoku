// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernSudoku.Datas
{
    /// <summary>
    /// Step
    /// </summary>
    public class MsStep
    {
        /// <summary>
        /// Current point.
        /// </summary>
        public MsPoint Point { get; set; }

        /// <summary>
        /// Current node.
        /// </summary>
        public BrickNode Node { get; set; }
    }
}
