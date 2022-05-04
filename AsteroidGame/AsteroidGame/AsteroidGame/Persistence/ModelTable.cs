using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidGame.Persistence
{
    public class ModelTable
    {
        #region Fields
        private Int32 _gameSize;
        private Int32 _gametime;
        private Int32[,] _fieldValues;
        private Int32 _spaceshipPos;
        #endregion

        #region Property
        public Int32 GameSize { get { return _gameSize; } }
        public Int32 Size { get { return _fieldValues.GetLength(0); } }
        public int GameTime { get => _gametime; set => _gametime = value; }
        public Int32 this[Int32 x, Int32 y] { get { return GetValue(x, y); } }
        public Int32 SpaceshipPos { get { return _spaceshipPos; } set { _spaceshipPos = value; } }
        #endregion

        #region Constructor
        public ModelTable() : this(9) { }

        public ModelTable(int gameSize)
        {
            if (gameSize < 0)
                throw new ArgumentOutOfRangeException("The table size is less than 0.", "tableSize");
            _gameSize = gameSize;
            _fieldValues = new Int32[gameSize, gameSize];
        }
        #endregion

        #region Methods

        public Int32 GetValue(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldValues[x, y];
        }
        
        // 0 empty ; 1 spaceship ; 2 asteroid 
        public void SetValue(Int32 x, Int32 y, Int32 value)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");
            if (value < 0 || value > _fieldValues.GetLength(0) + 1)
                throw new ArgumentOutOfRangeException("value", "The value is out of range.");
            _fieldValues[x, y] = value;
        }
        
        public void StepValue(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");
            if (x + 1 < _fieldValues.GetLength(0) && _fieldValues[x + 1, y] == 0)
            {
                _fieldValues[x + 1, y] = 2;
                _fieldValues[x, y] = 0;
            }
            else
            {
                _fieldValues[x, y] = 0;
            }

        }
        
        public Boolean IsEmpty(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldValues[x, y] == 0;
        }
        #endregion
    }
}