using CommonControls.Common;
using ModernSudoku.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ModernSudoku.Views
{
    /// <summary>
    /// CenterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CenterWindow : LayoutControl
    {
        private List<Grid> Grids;
        private List<Storyboard> Storyboards;

        public CenterWindow()
        {
            InitializeComponent();
            this.DataContext = new CenterViewModel(this);
            this.Grids = new List<Grid>()
            {
                this.gridStart,
                this.gridPerformance,
                this.gridAbout,
                this.gridCustomize,
                this.gridSettings,
            };
            this.Storyboards = new List<Storyboard>()
            {
                null, 
                this.sbPerformance,
                this.sbAbout,
                this.sbCustomize,
                this.sbSettings,
            };

            this.sbPerformance.Begin();
            this.sbPerformance.Stop();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Control button = sender as Control;
            Grid ownerGrid = button.Parent as Grid;
            int gridIndex = this.GetGridIndex(ownerGrid);

            if (gridIndex != 0)
            {
                this.Storyboards[gridIndex].Pause();
            }

            this.SetGridSurface(ownerGrid);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Control button = sender as Control;
            Grid ownerGrid = button.Parent as Grid;
            int gridIndex = this.GetGridIndex(ownerGrid);

            if (gridIndex != 0)
            {
                this.Storyboards[gridIndex].Begin();
            }
        }

        /// <summary>
        /// Get grid index.
        /// </summary>
        private int GetGridIndex(Grid grid)
        {
            int index = 0;

            if (grid == this.gridStart)
            {
                index = 0;
            }
            else if (grid == this.gridPerformance)
            {
                index = 1;
            }
            else if (grid == this.gridAbout)
            {
                index = 2;
            }
            else if (grid == this.gridCustomize)
            {
                index = 3;
            }
            else if (grid == this.gridSettings)
            {
                index = 4;
            }

            return index;
        }

        /// <summary>
        /// Set grid to the surface.
        /// </summary>
        private void SetGridSurface(Grid grid)
        {
            Grid.SetZIndex(grid, 4);

            List<int> indexes = new List<int>() { 0, 1, 2, 3 };

            foreach (Grid element in this.Grids)
            {
                if (element != grid)
                {
                    int cIndex = indexes.First();
                    indexes.Remove(cIndex);

                    Grid.SetZIndex(element, cIndex);
                }
            }
        }
    }
}
