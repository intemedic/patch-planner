using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace PatchPlanner
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            this.PropertyChanged?.Invoke(this, args);
        }

        protected internal void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.RaisePropertyChanged(propertyName);

            return true;
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            onChanged?.Invoke();
            this.RaisePropertyChanged(propertyName);

            return true;
        }

        public static void InvokeOnUIThread(Action action)
        {
            var dispatcher = Application.Current != null && Application.Current.Dispatcher != null
                ? Application.Current.Dispatcher
                : Dispatcher.CurrentDispatcher;
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.BeginInvoke(action);
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}