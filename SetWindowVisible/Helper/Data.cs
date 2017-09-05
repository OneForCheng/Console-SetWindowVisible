using System;

namespace SetWindowVisible.Helper
{
    [Serializable]
    public class Data
    {
        public Data()
        {
            Pid = 0;
            MainWindowHandle = 0;
        }

        public Data(int pid, long hwnd)
        {
            Pid = pid;
            MainWindowHandle = hwnd;
        }

        public int Pid { get; set; }

        public long MainWindowHandle { get; set; }

    }
}