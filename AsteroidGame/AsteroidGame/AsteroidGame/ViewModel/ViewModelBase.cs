using System.ComponentModel;
using System;
using System.Runtime.CompilerServices;

namespace AsteroidGame.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase() { }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}