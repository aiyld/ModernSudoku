// Writer: Winter Yang
// Mail: 1161226280@qq.com
// All rights reserved.
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ModernSudoku.Model
{
    public class NotificationObject : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SetProperty<T>(ref T propertyValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (!object.Equals(propertyValue, newValue))
            {
                propertyValue = newValue;
                this.RaisePropertyChanged(propertyName);
            }
        }
    }
}
