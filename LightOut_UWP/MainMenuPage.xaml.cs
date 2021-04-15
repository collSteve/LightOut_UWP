using System;
using System.Collections.Generic;
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
    public sealed partial class MainMenuPage : Page
    {
        List<GameSlectingItem> GameSelectingItems;
        public MainMenuPage()
        {
            this.InitializeComponent();
            InitializeGameSelectingItems();

            GameSelectingFlipView.ItemsSource = GameSelectingItems;
        }

        void InitializeGameSelectingItems()
        {
            GameSelectingItems = new List<GameSlectingItem>();
            for (int i=0; i< GameStorage.Games.Length; i++)
            {
                GameSlectingItem item = new GameSlectingItem();
                item.GameID = i;

                GameSelectingItems.Add(item);
            }
        }

        void OnGo(Object sender, RoutedEventArgs e)
        {
            //int id;
            GameSlectingItem item = (GameSlectingItem)GameSelectingFlipView.SelectedItem;
            /*if (int.TryParse(item.Text, out id) && 0<=id && id <=GameStorage.Games.Length)
            {
                NavigationInfo parameter = new NavigationInfo();
                parameter.GameID = id;
                Frame.Navigate(typeof(GamePage), parameter);
            }*/
            NavigationInfo parameter = new NavigationInfo();
            parameter.GameID = item.GameID;
            Frame.Navigate(typeof(GamePage), parameter);
        }
    }

    class GameSlectingItem
    {
        public string Name { set; get; }
        public int GameID { set; get; }
    }

}
