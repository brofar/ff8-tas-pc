using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FF8_TAS
{
    class FF8_Balamb_Intro
    {
        public static void NameSquall()
        {
            bool ready;
            do
            {
                ready = FF8_memory.NamingMenuInputReady;
                FF8_controller.PressA(16);
            } while (!ready);

            // Wait for game to accept inputs
            // TODO: see if there's a way to do this from game memory instead of timer.
            Thread.Sleep(175);

            // Delete quall
            FF8_controller.PressB(16); // l
            FF8_controller.PressB(16); // l
            FF8_controller.PressB(16); // a
            FF8_controller.PressB(16); // u
            FF8_controller.PressB(16); // q
            FF8_controller.PressStart(16);
            FF8_controller.PressA(16);
        }

        public static void QuistisWalk()
        {
            bool ready;
            int progress = 0;
            do
            {
                ready = FF8_memory.MapId == 229;
                FF8_controller.PressA();
            } while (!ready);

            // hold down
            //SetAxisValue(Xbox360Axis.LeftThumbY, Globals.MIN_AXIS);

            do
            {
                progress = FF8_memory.StoryProgress;
                if (progress > 12)
                {
                    // Let go of down
                    //Globals.controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
                }
                //PressButton(Xbox360Button.A);
            } while (progress < 14);
        }
    }
}
