using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using SetWindowVisible.Helper;

namespace SetWindowVisible
{

    /// <summary>Enumeration of the different ways of showing a window using ShowWindow</summary>
    public enum WindowShowStyle
    {
        /// <summary>Hides the window and activates another window.</summary>
        /// <remarks>See SW_HIDE</remarks>
        Hide = 0,
        /// <summary>Activates and displays a window. If the window is minimized
        /// or maximized, the system restores it to its original size and
        /// position. An application should specify this flag when displaying
        /// the window for the first time.</summary>
        /// <remarks>See SW_SHOWNORMAL</remarks>
        ShowNormal = 1,
        /// <summary>Activates the window and displays it as a minimized window.</summary>
        /// <remarks>See SW_SHOWMINIMIZED</remarks>
        ShowMinimized = 2,
        /// <summary>Activates the window and displays it as a maximized window.</summary>
        /// <remarks>See SW_SHOWMAXIMIZED</remarks>
        ShowMaximized = 3,
        /// <summary>Maximizes the specified window.</summary>
        /// <remarks>See SW_MAXIMIZE</remarks>
        Maximize = 3,
        /// <summary>Displays a window in its most recent size and position.
        /// This value is similar to "ShowNormal", except the window is not
        /// actived.</summary>
        /// <remarks>See SW_SHOWNOACTIVATE</remarks>
        ShowNormalNoActivate = 4,
        /// <summary>Activates the window and displays it in its current size
        /// and position.</summary>
        /// <remarks>See SW_SHOW</remarks>
        Show = 5,
        /// <summary>Minimizes the specified window and activates the next
        /// top-level window in the Z order.</summary>
        /// <remarks>See SW_MINIMIZE</remarks>
        Minimize = 6,
        /// <summary>Displays the window as a minimized window. This value is
        /// similar to "ShowMinimized", except the window is not activated.</summary>
        /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
        ShowMinNoActivate = 7,
        /// <summary>Displays the window in its current size and position. This
        /// value is similar to "Show", except the window is not activated.</summary>
        /// <remarks>See SW_SHOWNA</remarks>
        ShowNoActivate = 8,
        /// <summary>Activates and displays the window. If the window is
        /// minimized or maximized, the system restores it to its original size
        /// and position. An application should specify this flag when restoring
        /// a minimized window.</summary>
        /// <remarks>See SW_RESTORE</remarks>
        Restore = 9,
        /// <summary>Sets the show state based on the SW_ value specified in the
        /// STARTUPINFO structure passed to the CreateProcess function by the
        /// program that started the application.</summary>
        /// <remarks>See SW_SHOWDEFAULT</remarks>
        ShowDefault = 10,
        /// <summary>Windows 2000/XP: Minimizes a window, even if the thread
        /// that owns the window is hung. This flag should only be used when
        /// minimizing windows from a different thread.</summary>
        /// <remarks>See SW_FORCEMINIMIZE</remarks>
        ForceMinimized = 11
    }

    public static class Extentions
    {

        public static void Add(this Dict dict, int pid, long hwnd)
        {
            var data = dict.Datas.SingleOrDefault(m => m.Pid == pid);
            if (data != null)
            {
                data.MainWindowHandle = hwnd;
            }
            else
            {
                dict.Datas.Add(new Data(pid, hwnd));
            }
            dict.Save();
        }

        public static long Find(this Dict dict, int pid)
        {
            var data = dict.Datas.SingleOrDefault(m => m.Pid == pid);
            if (data != null)
            {
                return data.MainWindowHandle;
            }
            else
            {
                return 0;
            }
        }

        public static void PrintHiddenWindow(this Dict dict)
        {
            var num = 0;
           
            var procs = Process.GetProcesses();
            for (var i = dict.Datas.Count - 1; i >= 0; i--)
            {
                var data = procs.SingleOrDefault(m => m.Id == dict.Datas[i].Pid);
                if (data != null && data.MainWindowHandle == IntPtr.Zero)
                {
                    if (num == 0)
                    {
                        Console.WriteLine("{0} {1} {2}", "Index".PadRight(8), "ProcessName".PadRight(25), "PID".PadRight(8));
                        Console.WriteLine("{0} {1} {2}", string.Empty.PadRight(8, '='), string.Empty.PadRight(25, '='), string.Empty.PadRight(8, '='));
                    }
                    num++;
                    var str = num < 10 ? $"[0{num}]" : $"[{num}]";
                    var name = data.ProcessName;
                    if (name.Length > 25)
                    {
                        name = name.Substring(0, 25);
                    }
                    Console.WriteLine("{0} {1} {2}", str.PadRight(8), name.PadRight(25), data.Id.ToString().PadRight(8));
                }
                else
                {
                    dict.Datas.RemoveAt(i);
                }
            }
            dict.Save();
            if (num == 0)
            {
                Console.WriteLine("暂无隐藏窗口信息。");
            }
        }

        [DllImport("user32.dll", EntryPoint = "IsWindow")]

        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(this IntPtr hwnd, WindowShowStyle nCmdShow);

    }
}
