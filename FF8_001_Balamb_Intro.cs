using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FF8_TAS
{
    class FF8_001_Balamb_Intro
    {
        public static void Infirmary()
        {
            Logger.WriteLog("Starting infirmary.");
            do
            {
                FF8_controller.PressA(16);
            } while (!FF8_memory.NamingMenuInputReady);

            // Wait for game to accept inputs
            // TODO: see if there's a way to do this from game memory instead of timer.
            Thread.Sleep(210);

            Logger.WriteLog("Naming Squall.");
            // Delete quall
            FF8_controller.PressB(16); // l
            FF8_controller.PressB(16); // l
            FF8_controller.PressB(16); // a
            FF8_controller.PressB(16); // u
            FF8_controller.PressB(16); // q
            FF8_controller.PressStart(16);
            FF8_controller.PressA(16);

            // Rest of infirmary dialogue
            do
            {
                FF8_controller.PressA();
            } while (FF8_memory.MapId != 229); // 229 == Hallway Map
        }
        public static void QuistisWalk()
        {
            Logger.WriteLog("Starting hallway.");

            // hold down
            FF8_controller.HoldDown();

            int progress;
            do
            {
                progress = FF8_memory.StoryProgress;
                if (progress > 12)
                {
                    // Let go of down
                    FF8_controller.ReleaseDown();
                }
                FF8_controller.PressA();
            } while (progress < 14);
        }
        public static void Classroom()
        {
            Logger.WriteLog("Starting classroom (2nd half).");

            // Quistis starts class
            while (FF8_memory.ControlsLocked)
            {
                FF8_controller.PressA();
            }

            // U then R, not UR
            FF8_controller.HoldUR();

            // Move to front
            while (FF8_memory.CameraUsed == 0);

            FF8_controller.ReleaseUR();
            FF8_controller.HoldDR();

            // Quistis at 967, -2847
            while (FF8_memory.FieldCoordX > 940);

            FF8_controller.ReleaseDR();
            FF8_controller.HoldRight();
            while (FF8_memory.FieldCoordY > -2778);

            FF8_controller.ReleaseRight();

            //Quistis dialogue
            while (FF8_memory.StoryProgress < 16)
            {
                FF8_controller.PressA();
            }

            FF8_controller.HoldUR();

            // Classroom Door at 1458, -3313
            while (FF8_memory.FieldCoordY > -3313);

            FF8_controller.ReleaseUR();
            FF8_controller.HoldRight();

            // Leave classroom
            while (FF8_memory.FieldCoordX > 0 && FF8_memory.MapId != 139);
            
            FF8_controller.ReleaseRight();
        }
        public static void Hallway2F()
        {
            Logger.WriteLog("Starting Selphie hallway.");

            // Ideal entry point is -369, -6600

            // Down until -1160 - -1233, -6506
            // DL until selphie

            // Wait for squall to be in position
            
            FF8_controller.HoldDown();
            FF8_controller.HoldLeft();

            // Field coords don't update as fast as map does so wait a bit before checking against coords
            Thread.Sleep(3000);

            while (FF8_memory.FieldCoordX < -800) ;

            Logger.WriteLog("Releasing Left & Holding right");
            FF8_controller.ReleaseLeft();
            FF8_controller.HoldRight();
            while (!FF8_memory.DialogueBoxOpen) ;
            FF8_controller.ReleaseDown();
            FF8_controller.ReleaseRight();

            Logger.WriteLog("Waiting for a choice.");
            while (!FF8_memory.DialogeChoiceAvailable)
            {
                FF8_controller.PressA();
            }
            Logger.WriteLog("Choice found.");
            Thread.Sleep(25);

            // "Are you okay?" First choice.
            FF8_controller.PressA();

            // Mash until tour option
            Logger.WriteLog("Mash until tour option.");
            while (!FF8_memory.DialogeChoiceAvailable)
            {
                FF8_controller.PressA();
            }

            Logger.WriteLog("Skip tour.");
            // Skip tour
            FF8_controller.MoveDown();
            FF8_controller.PressA();
        }
    }
}
