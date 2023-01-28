using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiOcrSample.Extends
{
    /// <summary>
    /// ViewModelの基底クラス
    /// </summary>
    internal class BindableBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        /// <summary>
        /// PropertyChangedEventHandler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }
            field = value;

            // プロパティ変更を通知
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
