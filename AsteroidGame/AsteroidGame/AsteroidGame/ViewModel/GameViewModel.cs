using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using AsteroidGame.Model;
using Xamarin.Forms;

namespace AsteroidGame.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        #region Fields
        private GameModel _model;
        #endregion

        #region Properties
        public DelegateCommand PauseCommand { get; private set; }
        
        public DelegateCommand MoveRightCommand { get; private set; }
        
        public DelegateCommand MoveLeftCommand { get; private set; }
        
        public DelegateCommand NewGameCommand { get; private set; }
        
        public DelegateCommand LoadGameCommand { get; private set; }
        
        public DelegateCommand SaveGameCommand { get; private set; }
        
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand ReturnedCommand { get; private set; }
        public DelegateCommand ReturnedSCommand { get; private set; }
        public ObservableCollection<Field> Fields { get; set; }
        #endregion

        #region Events
        
        public event EventHandler Pause;
        
        public event EventHandler NewGame;
        
        public event EventHandler LoadGame;
        
        public event EventHandler SaveGame;
        
        public event EventHandler ExitGame;
        public event EventHandler ReturnedGame;
        public event EventHandler ReturnedSettings;

        #endregion

        #region Constructors
        public GameViewModel(GameModel model)
        {
            _model = model;
            _model.GameCreated += new EventHandler (Model_GameCreated);
            _model.RefreshTable += new EventHandler(Model_GameStep);
            
            PauseCommand = new DelegateCommand(param => onPauseCommand());
            NewGameCommand = new DelegateCommand(param => onNewGame());
            LoadGameCommand = new DelegateCommand(param => onLoadGame());
            SaveGameCommand = new DelegateCommand(param => onSaveGame());
            ExitCommand = new DelegateCommand(param => onExitGame());
            MoveRightCommand = new DelegateCommand(param => onMoveRight());
            MoveLeftCommand = new DelegateCommand(param => onMoveLeft());
            ReturnedCommand = new DelegateCommand(param => onReturnedCommand());
            ReturnedSCommand = new DelegateCommand(param => onReturnedSCommand());
            
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < _model.Table.Size; i++)
            {
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    Fields.Add(new Field
                    {
                        Images = _model.GameTable[i, j],
                        X = i,
                        Y = j,
                        Number = i * _model.Table.Size + j,
                       
                    }); ;
                }
            }

            RefreshTable();
        }

        private void onReturnedSCommand()
        {
            if (ReturnedSettings != null)
                ReturnedSettings(this, EventArgs.Empty);
        }

        #endregion

        #region Private methods
        
        public void RefreshTable()
        {
            foreach (Field field in Fields)
            {
                field.Images = _model.GameTable[field.X, field.Y];
            }
        }
        #endregion

        #region Game event handlers
        
        private void Model_GameCreated(object sender,EventArgs e)
        {
            RefreshTable();
        }

        private void Model_GameStep(object sender, EventArgs e)
        {
            RefreshTable();
        }
        public void Model_NewGame()
        {
            _model.NewGame();
            RefreshTable();
        }
        #endregion

        #region Event methods
        
        private void onNewGame()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }
        
        private void onPauseCommand()
        {
            if (Pause != null)
                Pause(this, EventArgs.Empty);
        }
        private void onReturnedCommand()
        {
            if (ReturnedGame != null)
                ReturnedGame(this, EventArgs.Empty);
        }
        private void onLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }
        
        private void onSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }
        
        private void onExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        private void onMoveRight()
        {
            if (!_model.IsOver)
            {
                _model.RightMove();
            }
        }

        private void onMoveLeft()
        {
            if (!_model.IsOver)
            { 
                _model.LeftMove();
            }
        }
        #endregion
    }
}