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
            bool ready;
            do
            {
                ready = FF8_memory.NamingMenuInputReady;
                FF8_controller.PressA(16);
            } while (!ready);

            // Wait for game to accept inputs
            // TODO: see if there's a way to do this from game memory instead of timer.
            Thread.Sleep(200);

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
                ready = FF8_memory.MapId == 229; // 229 == Hallway Map
                FF8_controller.PressA();
            } while (!ready);
        }
        public static void QuistisWalk()
        {
            Logger.WriteLog("Starting hallway.");
            int progress;

            // hold down
            FF8_controller.HoldDown();

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

            bool locked;
            int camera, xCoord, yCoord, progress, mapId;

            // Quistis starts class
            do
            {
                locked = FF8_memory.ControlsLocked;
                FF8_controller.PressA();
            } while (locked);

            FF8_controller.HoldUR();

            // Move to front
            do
            {
                camera = FF8_memory.CameraUsed;
            } while (camera == 0);

            FF8_controller.ReleaseUR();
            FF8_controller.HoldDR();

            // Quistis at 967, -2847
            do
            {
                xCoord = FF8_memory.FieldCoordX;
            } while (xCoord > 940);

            FF8_controller.ReleaseDR();
            Thread.Sleep(16);
            FF8_controller.HoldRight();
            //Thread.Sleep(2000);
            
            do
            {
                yCoord = FF8_memory.FieldCoordY;
            } while (yCoord > -2778);

            FF8_controller.ReleaseRight();

            //Quistis dialogue
            do
            {
                progress = FF8_memory.StoryProgress;
                FF8_controller.PressA();
            } while (progress < 16);

            FF8_controller.HoldUR();

            // Classroom Door at 1458, -3313
            do
            {
                yCoord = FF8_memory.FieldCoordY;
            } while (yCoord > -3313);

            FF8_controller.ReleaseUR();
            Thread.Sleep(16);
            FF8_controller.HoldRight();

            // Leave classroom
            do
            {
                mapId = FF8_memory.MapId;
            } while (mapId != 139);

            FF8_controller.ReleaseRight();
            Thread.Sleep(16);
        }
        public static void Hallway2F()
        {
            Logger.WriteLog("Starting Selphie hallway.");
            bool dialogue;
            FF8_controller.HoldDL();
            Thread.Sleep(2000);
            FF8_controller.ReleaseDL();
            FF8_controller.HoldDown();
            do
            {
                dialogue = FF8_memory.DialogueBoxOpen;
            } while (!dialogue);
            FF8_controller.ReleaseDown();

        }
    }
}
