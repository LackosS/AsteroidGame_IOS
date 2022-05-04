using System;
using System.Threading.Tasks;
using AsteroidGame.Model;
using AsteroidGame.Persistence;
using AsteroidGame.View;
using AsteroidGame.ViewModel;
using Xamarin.Forms;
using System.Timers;
using Xamarin.Forms.Xaml;
using System.Reflection;

namespace AsteroidGame
{
    public partial class App : Application
    {
        #region Fields

        private IModelDataAccess _modelDataAccess;
        private GameModel _gameModel;
        private GameViewModel _gameViewModel;
        private GamePage _gamePage;
        private SettingsPage _settingsPage;

        private IStore _store;
        private StoredGameBrowserModel _storedGameBrowserModel;
        private StoredGameBrowserViewModel _storedGameBrowserViewModel;
        private LoadPage _loadGamePage;
        private SavePage _saveGamePage;
        private Pause _pausePage;

        private Timer _timer;
        private NavigationPage _mainPage;

        #endregion

        #region Application method

        public App()
        {
            /*var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            foreach (var res in assembly.GetManifestResourceNames()) {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
            */
            _modelDataAccess = DependencyService.Get<IModelDataAccess>();
            _gameModel = new GameModel(_modelDataAccess);
            _gameModel.GameOver += new EventHandler(GameModel_GameOver);

            _gameViewModel = new GameViewModel(_gameModel);
            _gameViewModel.NewGame += new EventHandler(GameViewModel_NewGame);
            _gameViewModel.LoadGame += new EventHandler(GameViewModel_LoadGame);
            _gameViewModel.SaveGame += new EventHandler(GameViewModel_SaveGame);
            _gameViewModel.ExitGame += new EventHandler(GameViewModel_ExitGame);
            _gameViewModel.Pause += new EventHandler(GameViewModel_PauseGame);
            _gameViewModel.ReturnedGame += new EventHandler(GameViewModel_ReturnedGame);
            _gameViewModel.ReturnedSettings += new EventHandler(GameViewModel_ReturnedSettings);
            _gamePage = new GamePage();
            _gamePage.BindingContext = _gameViewModel;

            _settingsPage = new SettingsPage();
            _settingsPage.BindingContext = _gameViewModel;

            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += Timer_Tick;
            _timer.Start();

            _store = DependencyService.Get<IStore>();
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading +=
                new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameLoading);
            _storedGameBrowserViewModel.GameSaving +=
                new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameSaving);

            _loadGamePage = new LoadPage();
            _loadGamePage.BindingContext = _storedGameBrowserViewModel;

            _saveGamePage = new SavePage();
            _saveGamePage.BindingContext = _storedGameBrowserViewModel;

            _mainPage = new NavigationPage(_gamePage);
            _pausePage = new Pause();
            _pausePage.BindingContext = _gameViewModel;
            MainPage = _mainPage;
        }

        private void Timer_Tick(Object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Random r = new Random();
                Int32 rand = r.Next(1, 100);
                _gameModel.TimerCount += e.SignalTime.Millisecond;
                if (_gameModel.TimerCount % rand == 0 && _timer.Interval - 50 > 0)
                {
                    _timer.Interval -= 50;
                }

                _gameModel.AdvanceTime();
            });
        }

        protected override void OnStart()
        {
            _gameModel.NewGame();
            _gameViewModel.RefreshTable();
        }

        protected override void OnSleep()
        {
            try
            {
                Task.Run(async () => await _gameModel.SaveGame("SuspendedGame"));
            }
            catch
            {
            }
        }

        protected override void OnResume()
        {
            try
            {
                Task.Run(async () =>
                {
                    await _gameModel.LoadGame("SuspendedGame");
                    _gameViewModel.RefreshTable();
                });
            }
            catch
            {
            }
        }

        #endregion

        #region ViewModel event handlers

        private void GameViewModel_NewGame(object sender, EventArgs e)
        {
            _gameModel.NewGame();
            _timer.Start();
            _mainPage.PopAsync();
        }

        private void GameViewModel_PauseGame(object sender, EventArgs e)
        {
            _timer.Stop();
            _mainPage.PushAsync(_pausePage);
        }

        private void GameViewModel_ReturnedGame(object sender, EventArgs e)
        {
            if (!_timer.Enabled)
            {
                _timer.Start();
                _mainPage.PopAsync();
            }
        }
        private void GameViewModel_ReturnedSettings(object sender, EventArgs e)
        {

            _mainPage.PopAsync();

        }

        private async void GameViewModel_LoadGame(object sender, System.EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await _mainPage.PushAsync(_loadGamePage);
        }

        private async void GameViewModel_SaveGame(object sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await _mainPage.PushAsync(_saveGamePage);
        }

        private async void GameViewModel_ExitGame(object sender, EventArgs e)
        {
            await _mainPage.PushAsync(_settingsPage);
        }


        private async void StoredGameBrowserViewModel_GameLoading(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync();

            try
            {
                await _gameModel.LoadGame(e.Name);
                _gameViewModel.RefreshTable();
            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message + " " + a.StackTrace);
                await MainPage.DisplayAlert("Asteroid game", "Unsuccessful loading.", "OK");
            }
            await MainPage.DisplayAlert("Asteroid game", "Successfully loaded.", "OK");
        }

        private async void StoredGameBrowserViewModel_GameSaving(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync();

            try
            {
                await _gameModel.SaveGame(e.Name);
            }
            catch
            {
                await MainPage.DisplayAlert("Asteroid game", "Unsuccessful saving.", "OK");
            }

            await MainPage.DisplayAlert("Asteroid game", "Successfully saved.", "OK");
        }

        #endregion

        #region Model event handlers

        private async void GameModel_GameOver(object sender, EventArgs e)
        {
            _timer.Stop();
            await MainPage.DisplayAlert("Asteroid Game", "Your lifetime was: " + _gameModel.TimerCount / 1000, "OK");
            _gameModel.NewGame();
            _timer.Start();
        }

        #endregion
    }
}