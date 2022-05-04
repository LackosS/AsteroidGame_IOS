using System;

namespace AsteroidGame.ViewModel
{
    public class StoredGameViewModel : ViewModelBase
    {
        private String _name;
        private DateTime _modified;
        
        public String Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime Modified
        {
            get { return _modified; }
            set
            {
                if (_modified != value)
                {
                    _modified = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Betöltés parancsa.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; set; }

        /// <summary>
        /// Mentés parancsa.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; set; }
    }
}