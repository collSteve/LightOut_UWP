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
        int[,] lightMatrix = new int[,] {
            { 1, 0, 1, 1, 1 },
            { 0, 0, 0, 1, 1 },
            { 1, 0, 1, 1, 1 } };
        Game game;

        public GamePage()
        {
            this.InitializeComponent();
            //LightGame game = new LightGame(lightMatrix);

            
        }

        private void SetUpGame()
        {
            // initialize game
            game.InitializeGame(GameBoard, GameStatueText, RestartButton);
            // hooking up game events
            Window.Current.SizeChanged += game.OnWindowSizeChanged;
            RestartButton.Click += game.OnRestart;
        }

        // Receive data on navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Debug.WriteLine(e.Parameter);
            try
            {
                var parameter = (NavigationInfo)e.Parameter;
                game = GameStorage.Games[parameter.GameID];
                SetUpGame();
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


    }
}
