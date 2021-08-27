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

            if (!Globals.LITE)
            {
                int startFmvSec = (3 * 60) + 25;
                Logger.WriteLog("Waiting " + startFmvSec + " seconds for FMV.");
                Thread.Sleep(startFmvSec * 1000);
            }
            else
            {
                Thread.Sleep(4000);
            }
        }
    }
}
