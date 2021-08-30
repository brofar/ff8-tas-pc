using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace FF8_TAS
{
    class FF8_controller
    {
        static IXbox360Controller controller;
        static readonly short MAX_AXIS = 32767;
        static readonly short MIN_AXIS = -32768;
        static readonly int MASH_HZ = 16; //7

        public static void Init()
        {
            ViGEmClient client = new ViGEmClient();
            controller = client.CreateXbox360Controller();
            controller.Connect();
            Logger.WriteLog("Controller connected.");
        }
        public static void Kill()
        {
            controller.Disconnect();
            Logger.WriteLog("Controller disconnected.");
        }
        static void PressButton(Xbox360Button button, int delay = -1)
        {
            int sleepTime = (delay == -1) ? MASH_HZ : delay;
            //Console.WriteLine("Pressing " + button.Name);
            controller.SetButtonState(button, true);
            Thread.Sleep(sleepTime);
            controller.SetButtonState(button, false);
            Thread.Sleep(MASH_HZ);
        }
        public static void PressA(int delay = -1)
        {
            PressButton(Xbox360Button.A, delay);
        }
        public static void PressB(int delay = -1)
        {
            PressButton(Xbox360Button.B, delay);
        }
        public static void PressX(int delay = -1)
        {
            PressButton(Xbox360Button.X, delay);
        }
        public static void PressY(int delay = -1)
        {
            PressButton(Xbox360Button.Y, delay);
        }
        public static void PressStart(int delay = -1)
        {
            PressButton(Xbox360Button.Start, delay);
        }
        public static void HoldUp()
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, MAX_AXIS);
            Thread.Sleep(MASH_HZ);
        }
        public static void ReleaseUp()
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
            Thread.Sleep(MASH_HZ);
        }
        public static void HoldDown()
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, MIN_AXIS);
            Thread.Sleep(MASH_HZ);
        }
        public static void ReleaseDown()
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
            Thread.Sleep(MASH_HZ);
        }
        public static void HoldLeft()
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, MIN_AXIS);
            Thread.Sleep(MASH_HZ);
        }
        public static void ReleaseLeft()
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
            Thread.Sleep(MASH_HZ);
        }
        public static void HoldRight()
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, MAX_AXIS);
            Thread.Sleep(MASH_HZ);
        }
        public static void ReleaseRight()
        {
            controller.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
            Thread.Sleep(MASH_HZ);
        }
        public static void HoldUR()
        {
            HoldUp();
            HoldRight();
        }
        public static void ReleaseUR()
        {
            ReleaseRight();
            ReleaseUp();
        }
        public static void HoldUL()
        {
            HoldUp();
            HoldLeft();
        }
        public static void ReleaseUL()
        {
            ReleaseLeft();
            ReleaseUp();
        }
        
        public static void HoldDL()
        {
            HoldDown();
            HoldLeft();
        }
        public static void ReleaseDL()
        {
            ReleaseLeft();
            ReleaseDown();
        }

        public static void HoldDR()
        {
            HoldDown();
            HoldRight();
        }
        public static void ReleaseDR()
        {
            ReleaseRight();
            ReleaseDown();
        }
        public static void MoveUR(int duration = 0)
        {
            HoldUR();
            Thread.Sleep(duration);
            ReleaseUR();
        }
        public static void MoveUL(int duration = 0)
        {
            HoldUL();
            Thread.Sleep(duration);
            ReleaseUL();
        }
        public static void MoveDL(int duration = 0)
        {
            HoldDL();
            Thread.Sleep(duration);
            ReleaseDL();
        }
        public static void MoveDR(int duration = 0)
        {
            HoldDR();
            Thread.Sleep(duration);
            ReleaseDR();
        }
        public static void MoveUp(int duration = 0)
        {
            HoldUp();
            Thread.Sleep(duration);
            ReleaseUp();
        }
        public static void MoveDown(int duration = 0)
        {
            HoldDown();
            Thread.Sleep(duration);
            ReleaseDown();
        }
        public static void MoveLeft(int duration = 0)
        {
            HoldLeft();
            Thread.Sleep(duration);
            ReleaseLeft();
        }
        public static void MoveRight(int duration = 0)
        {
            HoldRight();
            Thread.Sleep(duration);
            ReleaseRight();
        }
    }
}
