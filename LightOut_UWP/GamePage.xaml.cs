using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LightOut_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        int GameID;
        Game _game;

        string successText = "WooHoo! You Won!";
        string failText = "Oops, please try again...";

        public GamePage()
        {
            this.InitializeComponent();

            // hook up events
            RestartButton.Click += OnRestart;
            NextLevelButton.Click += OnNextLevelClick;
            MenuButton.Click += OnMenuClick;

            NextLevelButton.Visibility = Visibility.Collapsed;
        }

        private void SetUpGame(Game game)
        {
            // initialize game
            game.InitializeGame(GameBoard, GameStatueText, RestartButton);
            // hooking up game events
            Window.Current.SizeChanged += game.OnWindowSizeChanged;
            //RestartButton.Click += game.OnRestart;
            
        }

        // Receive data on navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Debug.WriteLine(e.Parameter);
            try
            {
                var parameter = (NavigationInfo)e.Parameter;

                LoadGame(parameter.GameID);
            }
            catch(Exception error)
            {
                Debug.WriteLine($"Error when navigating to GamePage: {error}");
                Frame.Navigate(typeof(MainMenuPage));
            }
        }

        void OnMenuClick(Object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainMenuPage));
        }

        void OnNextLevelClick(Object sender, RoutedEventArgs e)
        {
            if (GameID < GameStorage.Games.Length - 1) // check if there is next level
            {
                // load next game
                LoadGame(GameID + 1);
            }
        }

        void OnRestart(Object sender, RoutedEventArgs e)
        {
            _game.Restart();
            GameStatueText.Text = _game.GameDescription;

            NextLevelButtonFlash.Stop();
        }

        void LoadGame(int gameID)
        {
            _game = GameStorage.Games[gameID]; // store the game
            GameID = gameID; // store Game ID
            SetUpGame(_game);

            // subscribe to game's game end event
            _game.GameEnded += OnGameEnded;

            RestartButtonFlash.Stop();
            NextLevelButtonFlash.Stop();

            GameStatueText.Text = _game.GameDescription;

            // hide next level button
            NextLevelButton.Visibility = Visibility.Collapsed;
        }

        void OnGameEnded(object sender, GameEndEventArgs e)
        {
            if (e.gameResult == GameResult.Succeeded)
            {
                NextLevelButton.Visibility = Visibility.Visible;
                NextLevelButtonFlash.Begin();

                GameStatueText.Text = successText;
            }
            else if (e.gameResult == GameResult.Failed)
            {
                RestartButtonFlash.Begin();

                GameStatueText.Text = failText;
            }
        }


    }
}
