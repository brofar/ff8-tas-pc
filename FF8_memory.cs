using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessMemoryReaderLib;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.ComponentModel;

namespace FF8_TAS
{
    class FF8_memory : INotifyPropertyChanged
    {
        

        static readonly string GAME = "FF8_FR";
        static IntPtr baseAddress;
        static Process ff8;
        static System.Timers.Timer memRead;
        public static Dictionary<string, int> memory = new Dictionary<string, int>();

        static Dictionary<string, int> addresses = new Dictionary<string, int>() {
            {"InBattle", 0x18FF520},
            {"InBattleResults", 0x167897C},
            {"InNamingMenu", 0x18E45E3},
            {"AtTitleScreen", 0x1976945},
            {"TitleScreenChoice", 0x1976968},
            {"DialogueBoxOpen", 0x192B25D},
            {"DialogueBoxText", 0x192B01E}
        };

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        //
        //https://stackoverflow.com/questions/2246777/raise-an-event-whenever-a-propertys-value-changed
        //
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(
    [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        public void SetProperty(string name, int value)
        {
            if (value != memory[name])
            {
                memory[name] = value;
                OnPropertyChanged(name);
                OnImageFullPathChanged(EventArgs.Empty);
            }
        }
        public int GetProperty(string name)
        {
            memory.TryGetValue(name, out int value);
            return value;
        }

        public void Start()
        {
            ff8 = this.DetectGame();
            this.ActivateGame();
            this.BeginMonitoring();
        }

        ///<summary>
        ///Checks if a process is running, then stores the base address for reading memory.
        ///</summary>
        Process DetectGame()
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

        void ActivateGame() {

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
        private void Myprc_Exited(object sender, EventArgs e)
        {
            Logger.WriteLog("FF8 exited.");
        }

        string GetActiveProcessFileName()
        {
            IntPtr hwnd = GetForegroundWindow();
            GetWindowThreadProcessId(hwnd, out uint pid);
            Process p = Process.GetProcessById((int)pid);
            return p.ProcessName;
        }

        void BeginMonitoring()
        {
            this.InitializeMemory();
            this.SetTimer();
        }
        void InitializeMemory()
        {
            Logger.WriteLog("Initializing memory dictionary");
            foreach (KeyValuePair<string, int> entry in addresses)
            {
                memory.Add(entry.Key, ReadMemoryAddress(entry.Value, 1));
            }
            Logger.WriteLog("Initializing memory dictionary: complete");
        }
        void SetTimer()
        {
            // Create a timer with a two second interval.
            memRead = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            memRead.Elapsed += OnTimedEvent;
            memRead.AutoReset = true;
            memRead.Enabled = true;
        }

        void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            foreach (KeyValuePair<string, int> entry in addresses)
            {
                this.SetProperty(entry.Key, ReadMemoryAddress(entry.Value, 1));
            }
        }

        /***************** M E M O R Y *****************/

        private int ReadMemoryAddress(int offset, uint bytelength)
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

        private int ByteToInt(byte[] bytes, int size)
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

        /***************** V A L U E S *****************/

        /*
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
        public static bool AtTitleScreen { get { return (ReadMemoryAddress(0x1976945, 1) == 1); }}

        // 0 = Nouvelle Partie
        // 1 = Continuer
        // 2 = Credits
        public static int TitleScreenChoice { get { return ReadMemoryAddress(0x1976968, 1); } }
        public static bool DialogueBoxOpen { get { return (ReadMemoryAddress(0x192B25D, 1) == 1); } }
        public static bool DialogueBoxText { get { return (ReadMemoryAddress(0x192B01E, 1) == 1); } }
        */
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
