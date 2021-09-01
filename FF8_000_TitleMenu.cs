using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FF8_TAS
{
    class FF8_000_TitleMenu
    {
        public static void NewGame()
        {
            Logger.WriteLog("Waiting for title screen.");
            while (!FF8_memory.AtTitleScreen)
            {
                Thread.Sleep(500);
            }

            // Up to "new game"
            while (FF8_memory.TitleScreenChoice != 0)
            {
                FF8_controller.MoveUp();
            }

            // Start new game
            FF8_controller.PressA(16);

            Logger.WriteLog("New game started.");
            while (FF8_memory.PlayTime == 0) ;
            Thread.Sleep(4000);
        }
    }
}
