using System;
using System.Collections.Generic;
using System.Text;
using AsteroidGame.Persistence;
using System.Threading.Tasks;

namespace AsteroidGame.Model
{
    public class GameModel
    {
        #region Private data
        private ModelTable _gameTable;
        private double _timerCount = 0;
        private IModelDataAccess _dataAccess;
        #endregion

        #region Property
        public ModelTable Table { get { return _gameTable; } }
        public ModelTable GameTable { get => _gameTable; set => _gameTable = value; }
        public double TimerCount { get => _timerCount; set => _timerCount = (int)value; }
        public bool IsOver { get; private set; } = false;
        #endregion

        #region Events
        public event EventHandler GameOver;
        public event EventHandler RefreshTable;
        public event EventHandler GameCreated;
        #endregion

        #region Constructor
        public GameModel(IModelDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _gameTable = new ModelTable();
        }
        #endregion

        public void AdvanceTime()
        {
            if (IsOver)
            {
                onGameOver();
            }
            Step();
            makeAsteroid();
            onRefreshTable();
        }
        #region NewLoadSave
        public void NewGame()
        {
            _gameTable = new ModelTable();
            _timerCount = 0;
            generateFields();
            IsOver = false;
            onGameCreated();
        }

        public async Task LoadGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _gameTable = await _dataAccess.LoadAsync(path);
            _timerCount = _gameTable.GameTime;
            onRefreshTable();
        }

        public async Task SaveGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, this);
        }

        #endregion


        #region MethodsForTable
        public int GetValue(Int32 x, Int32 y)
        {
            return _gameTable[x, y];
        }

        private void generateFields()
        {
            for (int i = 0; i < _gameTable.Size; ++i)
            {
                for (int j = 0; j < _gameTable.Size; ++j)
                {
                    _gameTable.SetValue(i, j, 0);
                }
            }

            _gameTable.SetValue(_gameTable.Size - 1, _gameTable.Size / 2, 1);
            _gameTable.SpaceshipPos = _gameTable.Size / 2;
        }

        private void makeAsteroid()
        {
            Random r = new Random();
            Int32 position = r.Next(_gameTable.Size);
            _gameTable.SetValue(0, position, 2);
        }

        public void RightMove()
        {
            if (_gameTable.SpaceshipPos + 1 < _gameTable.Size)
            {
                _gameTable.SetValue(_gameTable.Size - 1, _gameTable.SpaceshipPos, 0);
                ++_gameTable.SpaceshipPos;
            }
            _gameTable.SetValue(_gameTable.Size - 1, _gameTable.SpaceshipPos, 1);
            onRefreshTable();
        }

        public void LeftMove()
        {
            if (_gameTable.SpaceshipPos - 1 >= 0)
            {
                _gameTable.SetValue(_gameTable.Size - 1, _gameTable.SpaceshipPos, 0);
                --_gameTable.SpaceshipPos;
            }
            _gameTable.SetValue(_gameTable.Size - 1, _gameTable.SpaceshipPos, 1);
            onRefreshTable();
        }

        public void Step()
        {

            for (int i = _gameTable.Size - 1; i >= 0; --i)
            {
                for (int j = 0; j < _gameTable.Size; ++j)
                {
                    if (i + 1 < _gameTable.Size && _gameTable.GetValue(i + 1, j) == 1 && _gameTable.GetValue(i, j) == 2)
                    {
                        IsOver = true;
                    }

                    if (_gameTable.GetValue(i, j) == 2)
                    {
                        _gameTable.StepValue(i, j);
                    }
                }
            }
        }
        #endregion
        #region Private event methods

        private void onRefreshTable()
        {
            if (RefreshTable != null)
            {
                RefreshTable.Invoke(this, EventArgs.Empty);
            }
        }
        private void onGameOver()
        {
            if (GameOver != null)
            {
                GameOver.Invoke(this, EventArgs.Empty);
            }
        }

        private void onGameCreated()
        {
            if (GameCreated != null)
                GameCreated.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}