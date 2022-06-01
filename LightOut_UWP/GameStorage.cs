using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightOut_UWP
{
    static class GameStorage
    {
        public static Game[] Games = new Game[] {
            new LightGame(new int[,] {{ 1, 0, 1, 1, 1 },
                                      { 0, 0, 0, 1, 1 },
                                      { 1, 0, 1, 1, 1 } }),
            new LightGame(new int[,] {{ 1, 0, 1 },
                                      { 0, 0, 0 },
                                      { 1, 0, 1 } }),
            new LightGame(new int[,] {{ 1, 0 },
                                      { 0, 0 },
                                      { 1, 0 } }),
            new LightGame(new int[,] {{ 1, 0 },
                                      { 0, 0 },
                                      { 0, 1 } }),
            new LightGame(new int[,] {{ 1, 0, 1 },
                                      { 0, 0, 1 },
                                      { 1, 0, 0 } })

    };

    }

    public class NavigationInfo
    {
        public int GameID { get; set; }
    }
}
