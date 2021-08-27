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
        static readonly int MASH_HZ = 7;

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
            Console.WriteLine("Pressing " + button.Name);
            controller.SetButtonState(button, true);
            Thread.Sleep(sleepTime);
            controller.SetButtonState(button, false);
            Thread.Sleep(sleepTime);
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
        public static void MoveUp(int duration = -1)
        {
            duration = Math.Max(duration, MASH_HZ);

            controller.SetAxisValue(Xbox360Axis.LeftThumbY, MAX_AXIS);
            Thread.Sleep(duration);
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
            Thread.Sleep(MASH_HZ);
        }
    }
}
