// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using ModernSudoku.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ModernSudoku.ViewModels
{
    public class PerformanceViewModel : MBaseViewModel
    {
        private PerformanceModel selectedItem;

        /// <summary>
        /// General performances.
        /// </summary>
        public List<PerformanceModel> Performances { get; set; }

        /// <summary>
        /// Selected performance.
        /// </summary>
        public PerformanceModel SelectedItem
        {
            get { return this.selectedItem; }
            set { this.SetProperty(ref this.selectedItem, value); }
        }

        public PerformanceViewModel()
        {
            this.Performances = this.GetRanged(PerformanceOperation.GetGeneral());
            this.SelectedItem = this.Performances.FirstOrDefault();
        }

        /// <summary>
        /// Get ranged performance models.
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private List<PerformanceModel> GetRanged(IEnumerable<PerformanceModel> models)
        {
            IEnumerable<PerformanceModel> orderedModels = models.OrderBy(s => s.CostTime = s.EndTime - s.StartTime);

            for (int i = 0; i < orderedModels.Count(); i++)
            {
                PerformanceModel model = orderedModels.ElementAt(i);
                model.Index = i + 1;

                switch (i)
                {
                    case 0:
                        {
                            model.Background = Colors.Gold;
                            model.ImageUri = new Uri(@"/ModernSudoku;component/Images/gold.png", UriKind.RelativeOrAbsolute);
                        }break;
                    case 1:
                        {
                            model.Background = Colors.Silver;
                            model.ImageUri = new Uri(@"/ModernSudoku;component/Images/silver.png", UriKind.RelativeOrAbsolute);
                        } break;
                    case 2:
                        {
                            model.Background = Colors.Orange;
                            model.ImageUri = new Uri(@"/ModernSudoku;component/Images/cu.png", UriKind.RelativeOrAbsolute);
                        } break;
                    default:
                        {
                            model.Background = Colors.LightSalmon;
                            model.ImageUri = new Uri(@"/ModernSudoku;component/Images/Star.png", UriKind.RelativeOrAbsolute);
                        } break;
                }
            }

            return orderedModels.ToList();
        }
    }
}
