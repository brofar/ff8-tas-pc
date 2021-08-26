using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using System.Threading;
using System.Diagnostics;

namespace FF8_TAS
{
    //https://forums.vigem.org/topic/273/vigem-net-feeder-example-step-by-step
    class Program
    {
        static readonly string FF8_PATH = @"D:\Steam\steamapps\common\FINAL FANTASY VIII\FF8_Launcher.exe";
        static IXbox360Controller controller;
        static readonly short MAX_AXIS = 32767;
        static readonly short MIN_AXIS = -32768;
        static readonly int MASH_HZ = 7;
        static void Main(string[] args)
        {
            ViGEmClient client = new ViGEmClient();
            controller = client.CreateXbox360Controller();

            controller.Connect();

            Logger.WriteLog("Controller connected.");

            // Launch Game
            Logger.WriteLog("Running game launcher.");
            Process.Start(FF8_PATH);

            FF8_memory.Start();

            NewGame();
            //TestValveMash();
            NameSquall();

            controller.Disconnect();
            Logger.WriteLog("Controller disconnected.");
            Console.ReadLine();
        }
        private void Test(object sender, EventArgs e)
        {
            Logger.WriteLog("FF8 exited.");
        }
        static void NewGame()
        {
            Logger.WriteLog("Waiting for title screen.");
            while (!FF8_memory.AtTitleScreen)
            {
                Thread.Sleep(500);
            }
            while (FF8_memory.TitleScreenChoice != 0)
            {
                Move("up");
            }
            PressButton(Xbox360Button.A);
            int startFmvSec = (3 * 60) + 25;
            Logger.WriteLog("New game started. Waiting " + startFmvSec + " seconds");
            //Thread.Sleep(startFmvSec * 1000);
        }
        static void NameSquall()
        {
            bool ready;
            do
            {
                ready = FF8_memory.InNamingMenu;
                PressButton(Xbox360Button.A, 16);
            } while (!ready);

            // Wait for game to accept inputs
            // TODO: see if there's a way to do this from game memory instead of timer.
            Thread.Sleep(1000);

            // Delete quall
            PressButton(Xbox360Button.B, 16);
            PressButton(Xbox360Button.B, 16);
            PressButton(Xbox360Button.B, 16);
            PressButton(Xbox360Button.B, 16);
            PressButton(Xbox360Button.B, 16);
            PressButton(Xbox360Button.Start);
            PressButton(Xbox360Button.A);
        }
        static void TestValveMash()
        {

            // TESTING
            while (true)
            {
                Console.WriteLine("Enter Delay:");
                int freq = Convert.ToInt32(Console.ReadLine());
                Thread.Sleep(1500);
                for (int i = 0; i < 500; i++)
                {
                    PressButton(Xbox360Button.X, freq);
                    Console.WriteLine(i);
                }
                Logger.WriteLog("Enter to loop again.");
            }
            // END TESTING
        }
        static void PressButton(Xbox360Button button, int delay = -1)
        {
            int sleepTime = (delay == -1) ? MASH_HZ : delay;
            Console.WriteLine("Pressing " + button.Name);
            controller.SetButtonState(button, true);
            Thread.Sleep(sleepTime);
            controller.SetButtonState(button, false);
            Thread.Sleep(sleepTime);
        }
        static void Mash()
        {
            Logger.WriteLog("Mashing");
            controller.SetButtonState(Xbox360Button.A, true);
            Thread.Sleep(MASH_HZ);
            controller.SetButtonState(Xbox360Button.X, true);
            controller.SetButtonState(Xbox360Button.A, false);
            Thread.Sleep(MASH_HZ);
            controller.SetButtonState(Xbox360Button.X, false);
        }
        static void Move(string direction, int duration = -1)
        {
            // I hate this.
            if (duration == -1)
                duration = MASH_HZ;

            Logger.WriteLog("Holding " + direction + " for " + duration + "ms.");
            switch (direction.ToLower())
            {
                case "up":
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, MAX_AXIS);
                    Thread.Sleep(duration);
                    controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
                    Thread.Sleep(MASH_HZ);
                    break;
            }
        }
    }
}
