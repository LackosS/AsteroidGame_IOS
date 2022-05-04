using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidGame.ViewModel
{
    public class Field : ViewModelBase
    {
        private int _Images;
        public int Images
        {
            get { return _Images; }
            set
            {
                if (_Images!= value)
                {
                    _Images = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public Int32 X { get; set; }
        
        public Int32 Y { get; set; }
        
        public Int32 Number { get; set; }

    }
}