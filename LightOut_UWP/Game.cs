using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LightOut_UWP
{
    public abstract class Game
    {
        public Grid GameBoard;
        public TextBlock GameStatueText;
        public Button RestartButton;

        protected float gameBoardRowSpacing = 5f;
        protected float gameBoardColumnSpacing = 5f;
        protected float windowMarginScale = 0.9f;
        protected float gameBoardWindowHeightRatio = 1 / 2f;
        protected float gameBoardWindowWidthRatio = 1f;

        public Game()
        {
            
        }

        public virtual void InitializeGame(Grid gameBoard, TextBlock gameStatueText, Button restartButton)
        {
            SetUpGameControls(gameBoard, gameStatueText, restartButton);
            InitializeGameBoard(GameBoard);
            
        }

        protected virtual void SetUpGameControls(Grid gameBoard, TextBlock gameStatueText, Button restartButton)
        {
            GameBoard = gameBoard;
            GameStatueText = gameStatueText;
            RestartButton = restartButton;
        }

        public void OnRestart(object sender, RoutedEventArgs e)
        {
            InitializeGame(GameBoard, GameStatueText, RestartButton);
        }

        public void Restart()
        {
            InitializeGame(GameBoard, GameStatueText, RestartButton);
        }

        protected virtual void InitializeGameBoard(Grid board)
        {
            // empty game board
            board.Children.Clear();
            board.RowDefinitions.Clear();
            board.ColumnDefinitions.Clear();

            // style
            ResizeGameBoardByWindowSize();
        }

        public virtual void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            ResizeGameBoardByWindowSize();
            //Debug.WriteLine($"Smaller:{smallerDimension}, boardWidth: {gameBoardWidth}, boardHeight: {gameBoardHeight}");
        }

        virtual protected void ResizeGameBoardByWindowSize()
        {
            float maxHeight = (float)Window.Current.Bounds.Height * gameBoardWindowHeightRatio;
            float maxWidth = (float)Window.Current.Bounds.Width * gameBoardWindowWidthRatio;

            float smallerDimension = maxHeight > maxWidth ? maxWidth : maxHeight;

            GameBoard.Width = smallerDimension;
            GameBoard.Height = smallerDimension;
        }


    }
}
