using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessMemoryReaderLib;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace FF8_TAS
{
    class FF8_memory
    {
        static readonly string GAME = "FF8_FR";
        static IntPtr baseAddress;
        static Process ff8;


        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        static public void Start()
        {
            ff8 = DetectGame();
            ActivateGame();
        }

        ///<summary>
        ///Checks if a process is running, then stores the base address for reading memory.
        ///</summary>
        static Process DetectGame()
        {
            Logger.WriteLog("Looking for " + GAME + " in system process list.");

            // Find the FF8 process.
            List<Process> processes = new List<Process>();
            while (processes.Count == 0)
            {
                processes = Process.GetProcesses()
                    .Where(x => x.ProcessName.Equals(GAME, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Our executable will be the first (and only) process in the list of processes.
            Process gameProcess = processes.FirstOrDefault();
            Logger.WriteLog(GAME + " found. PID: " + gameProcess.Id);

            // Add event handler for exited process
            gameProcess.EnableRaisingEvents = true;
            gameProcess.Exited += new EventHandler(Myprc_Exited);

            // Calculate the game's base address, for reading memory offsets later.
            baseAddress = gameProcess.MainModule.BaseAddress;
            Logger.WriteLog(GAME + " base address: " + baseAddress.ToString("x"));

            return gameProcess;
        }
        static void ActivateGame() {

            string foregroundWindow;

            Logger.WriteLog("Attempting to focus " + GAME);
            do
            {
                Thread.Sleep(500);

                foregroundWindow = GetActiveProcessFileName();

                if (ff8.MainWindowHandle != IntPtr.Zero)
                    SetForegroundWindow(ff8.MainWindowHandle);
            } while (foregroundWindow != GAME);
            Logger.WriteLog(GAME + " window is active.");
        }

        // If FF8 is exited
        static private void Myprc_Exited(object sender, EventArgs e)
        {
            Logger.WriteLog("FF8 exited.");
        }

        static string GetActiveProcessFileName()
        {
            IntPtr hwnd = GetForegroundWindow();
            GetWindowThreadProcessId(hwnd, out uint pid);
            Process p = Process.GetProcessById((int)pid);
            return p.ProcessName;
        }

        /***************** M E M O R Y *****************/

        static private int ReadMemoryAddress(int offset, uint bytelength)
        {
            ProcessMemoryReader reader = new ProcessMemoryReader
            {
                ReadProcess = ff8
            };
            reader.OpenProcess();

            IntPtr readAddress = IntPtr.Add(baseAddress, offset);
            byte[] mem = reader.ReadProcessMemory(readAddress, bytelength, out int bytesReadSize);

            int i = ByteToInt(mem, bytesReadSize);

            return i;
        }

        static private int ByteToInt(byte[] bytes, int size)
        {
            int i = 0;
            try
            {
                if (size == 4)
                {
                    i = BitConverter.ToInt32(bytes, 0);
                }
                else if (size == 2)
                {
                    i = BitConverter.ToInt16(bytes, 0);
                }
                else if (size == 1)
                {
                    i = bytes[0];
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog(e.Message);
            }
            return i;
        }

        /***************** F U N C T I O N S *****************/
        public static bool AwaitControl()
        {
            Logger.WriteLog("Waiting for player control.");
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (ControlsLocked)
            {
                if(stopwatch.ElapsedMilliseconds % (30 * 1000) == 0)
                {
                    Logger.WriteLog("Waiting for player control (" + stopwatch.ElapsedMilliseconds / 1000 + "s).");
                }

            }
            stopwatch.Stop();
            return true;
        }

        /***************** V A L U E S *****************/
        static List<int> PartyHP
        {
            get
            {
                int squall = ReadMemoryAddress(0x18FDDC0, 2);
                int zell = ReadMemoryAddress(0x18FDE58, 2);
                int irvine = ReadMemoryAddress(0x18FDEF0, 2);
                int quistis = ReadMemoryAddress(0x18FDF88, 2);
                int rinoa = ReadMemoryAddress(0x18FE020, 2);
                int selphie = ReadMemoryAddress(0x18FE0B8, 2);
                int seifer = ReadMemoryAddress(0x18FE150, 2);
                int edea = ReadMemoryAddress(0x18FE1E8, 2);

                return new List<int>() { squall, zell, irvine, quistis, rinoa, selphie, seifer, edea };
            }
        }

        public static bool InBattle {get { return (ReadMemoryAddress(0x18FF520, 1) == 1); }}

        // XP/AP Screen
        public static bool InBattleResults { get { return (ReadMemoryAddress(0x167897C, 1) == 1); }}
        public static bool InNamingMenu { get { return (ReadMemoryAddress(0x18E45E3, 1) == 1); }}
        public static bool NamingMenuInputReady { get { return (ReadMemoryAddress(0x1929F58, 1) == 1); }}
        public static bool AtTitleScreen { get { return (ReadMemoryAddress(0x1976945, 1) == 1); }}

        // 0 = Nouvelle Partie
        // 1 = Continuer
        // 2 = Credits
        public static int TitleScreenChoice { get { return ReadMemoryAddress(0x1976968, 1); } }
        public static bool DialogueBoxOpen { get { return (ReadMemoryAddress(0x192B25D, 1) == 1); } }
        public static bool DialogueBoxText { get { return (ReadMemoryAddress(0x192B01E, 1) == 1); } }
        public static int MapId { get { return ReadMemoryAddress(0x18D2C98, 2); } }
        public static int StoryProgress { get { return ReadMemoryAddress(0x18FE790, 2); } }
        public static int StepId { get { return ReadMemoryAddress(0x18D2C90, 1); } }
        public static int RealStepId { get { return ReadMemoryAddress(0x18DC418, 1); } }
        public static int DangerValue { get { return ReadMemoryAddress(0x18DC422, 1); } }
        public static int CarawayCode { get { return ReadMemoryAddress(0x18FE968, 1); } }
        public static int EncounterId { get { return ReadMemoryAddress(0x1996A80, 2); } }
        public static int ZellDuelTimer { get { return ReadMemoryAddress(0x1976428, 2); } }
        public static bool InFmv { get { return (ReadMemoryAddress(0x1C9A470, 1) == 1); } }
        public static bool ControlsLocked { get { return ReadMemoryAddress(0x199CCF0, 1) == 1; } }
        public static int CameraUsed { get { return ReadMemoryAddress(0x18E45DE, 1); } }
        public static int FieldCoordX { get { return ReadMemoryAddress(0x1676F10, 4); } }
        public static int FieldCoordY { get { return ReadMemoryAddress(0x1676F14, 4); } }
        public static bool DialogeChoiceAvailable { get { return ReadMemoryAddress(0x18FE762, 1) == 1; } }
        public static int SolidDialogueBoxChoice { get { return ReadMemoryAddress(0x192B033, 1); } }
    }
}
