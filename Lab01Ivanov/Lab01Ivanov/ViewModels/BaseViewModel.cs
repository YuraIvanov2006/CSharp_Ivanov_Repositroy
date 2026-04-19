using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab01Ivanov.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _isBusy;
        /// <summary>True while an async operation is running. Bound to ActivityIndicator.</summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetProperty<T>(ref T field, T value,
            [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        /// <summary>Sets IsBusy, runs the action, then clears IsBusy.</summary>
        protected async Task RunBusyAsync(Func<Task> action)
        {
            IsBusy = true;
            try   { await action(); }
            finally { IsBusy = false; }
        }
    }
}
