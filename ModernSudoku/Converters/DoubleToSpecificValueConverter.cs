using System;
using System.Globalization;
using System.Windows.Data;

namespace ModernSudoku.Converters
{
    public class DoubleToSpecificValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 0;

            if (double.TryParse(value.ToString(), out result))
            {
                string tarFunStr = parameter as string;

                if (!string.IsNullOrWhiteSpace(tarFunStr))
                {
                    char funChar = tarFunStr[0];
                    string numStr = tarFunStr.Substring(1);
                    double num = 0;

                    if (double.TryParse(numStr, out num))
                    {
                        switch (funChar)
                        {
                            case '+': result += num; break;
                            case '-': result -= num; break;
                            case '*': result *= num; break;
                            case '/': result /= num; break;
                            case '%': result %= num; break;
                        }
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
