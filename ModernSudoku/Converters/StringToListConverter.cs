using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ModernSudoku.Converters
{
    public class StringToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
    }
}
