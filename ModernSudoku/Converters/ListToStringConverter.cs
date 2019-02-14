using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ModernSudoku.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = string.Empty;

            try
            {
                IEnumerable<int> results = (IEnumerable<int>)value;

                foreach (var r in results)
                {
                    result += r + ",";
                }

                if (result.EndsWith(","))
                {
                    result = result.Remove(result.Length - 1);
                }
            }
            catch { }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<int> result = new ObservableCollection<int>();

            try
            {
                string[] tar = value.ToString().Split(',');

                foreach (var s in tar)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        result.Add(int.Parse(s));
                    }
                }

            }catch{}

            return result;
        }
    }
}
