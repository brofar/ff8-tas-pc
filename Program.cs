using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace FF8_TAS
{
    static class Globals
    {
        // are we using lite%? (for testing)
        public static readonly bool LITE = true;
    }
    //https://forums.vigem.org/topic/273/vigem-net-feeder-example-step-by-step
    class Program
    {
        static readonly string FF8_PATH = @"D:\Steam\steamapps\common\FINAL FANTASY VIII\FF8_Launcher.exe";

        static void Main(string[] args)
        {

            FF8_controller.Init();
            // Launch Game
            Logger.WriteLog("Running game launcher.");
            Process.Start(FF8_PATH);

            FF8_memory.Start();

            FF8_000_TitleMenu.NewGame();
            //TestValveMash();
            FF8_001_Balamb_Intro.Infirmary();
            FF8_001_Balamb_Intro.QuistisWalk();
            FF8_001_Balamb_Intro.Classroom();
            FF8_001_Balamb_Intro.Hallway2F();

            FF8_controller.Kill();
            Console.ReadLine();
        }
        private void Test(object sender, EventArgs e)
        {
            Logger.WriteLog("FF8 exited.");
        }

        
        /*static void TestValveMash()
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
        }*/
    }
}
