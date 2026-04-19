using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab01Ivanov.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels.
    /// Implements INotifyPropertyChanged so the UI updates when properties change.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Sets a backing field and raises PropertyChanged only when the value actually changes.
        /// Returns true if the value changed.
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
