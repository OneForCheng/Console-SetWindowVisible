using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using SetWindowVisible.Helper;

namespace SetWindowVisible
{

    class Program
    {
        private static void Usage(string fileName)
        {
            var spaces = new string(' ', 4);
            if (fileName != string.Empty)
            {
                if (fileName.EndsWith("_"))
                {
                    fileName = fileName.Substring(0, fileName.Length - 1);
                }
            }
            var states = new[]
            {
                "Hide",
                "ShowNormal",
                "ShowMinimized",
                "ShowMaximized",
                "Maximize",
                "ShowNormalNoActivate",
                "Show",
                "Minimize",
                "ShowMinNoActivate",
                "ShowNoActivate",
                "Restore",
                "ShowDefault",
                "ForceMinimized",
            };
            Console.WriteLine("{0}设置窗体程序的可视化状态。{1}", Environment.NewLine, Environment.NewLine);
            Console.WriteLine("{0} /a", fileName);
            Console.WriteLine("{0} /h", fileName);
            Console.WriteLine("{0} PID [State]{1}", fileName, Environment.NewLine);
            Console.WriteLine("{0}/a         显示所有进程信息。", spaces);
            Console.WriteLine("{0}/h         显示已隐藏窗口的信息。", spaces);
            Console.WriteLine("{0}PID        指定进程的PID值。", spaces);
            Console.WriteLine("{0}State      窗体的状态值(0-11),此参数缺省时默认为0。{1}", spaces, Environment.NewLine);
            Console.WriteLine("{0}State值对应的窗体状态如下:{1}", spaces, Environment.NewLine);
            for (var i = 0; i < states.Length; i++)
            {
                Console.WriteLine("    {0}{1}   -   {2}", spaces, i.ToString().PadRight(2), states[i]);
            }
            Console.WriteLine("{0}{1}注：当参数为空时，默认显示可视化窗体程序列表。", Environment.NewLine, spaces);
        }

        private static void ShowProcessInfo(bool isAll)
        {
            var num = 0;
            Console.WriteLine("{0} {1} {2} {3}", "Index".PadRight(8), "ProcessName".PadRight(25), "PID".PadRight(8), "MainWindowTitle");
            Console.WriteLine("{0} {1} {2} {3}", string.Empty.PadRight(8, '='), string.Empty.PadRight(25, '='), string.Empty.PadRight(8, '='), string.Empty.PadRight(25, '='));
            var procs = isAll
                ? Process.GetProcesses().OrderBy(m => m.ProcessName)
                : Process.GetProcesses().Where(m => m.MainWindowHandle != IntPtr.Zero).OrderBy(m => m.ProcessName);
            foreach (var item in procs)
            {
                num++;
                var str = num < 10 ? $"[0{num}]" : $"[{num}]";
                var name = item.ProcessName;
                if (name.Length > 25)
                {
                    name = name.Substring(0, 25);
                }
                Console.WriteLine("{0} {1} {2} {3}", str.PadRight(8), name.PadRight(25), item.Id.ToString().PadRight(8), item.MainWindowTitle);
            }
        }

        static int Main(string[] args)
        {
            try
            {
                if (args.Length < 3)
                {
                    if (args.Length == 0)
                    {
                       ShowProcessInfo(false);
                    }
                    else if (args.Any(m => m.Contains("/?")))
                    {
                        var fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
                        fileName = fileName.Substring(0, fileName.LastIndexOf('.')).ToUpper();
                        Usage(fileName);
                    }
                    else if (args[0].ToLower() == "/a")
                    {
                        ShowProcessInfo(true);
                    }
                    else if (args[0].ToLower() == "/h")
                    {
                        var dict = new Dict();
                        dict.Load();
                        dict.PrintHiddenWindow();
                    }
                    else
                    {
                        if (Regex.IsMatch(args[0], "^[1-9][0-9]*$"))
                        {
                            int pid;
                            if (int.TryParse(args[0], out pid))
                            {
                                var style = WindowShowStyle.Hide;
                                if (args.Length == 2)
                                {
                                    int val;
                                    if (int.TryParse(args[1], out val) && val >= 0 && val < 12)
                                    {
                                        style = (WindowShowStyle)val;
                                    }
                                    else
                                    {
                                        Console.WriteLine("命令语法不正确。");
                                        return 1;
                                    }
                                }

                               
                                var proc = Process.GetProcesses().FirstOrDefault(m => m.Id == pid);
                                if (proc != null)
                                {
                                    var dict = new Dict();
                                    dict.Load();
                                    IntPtr hwnd;
                                    if (proc.MainWindowHandle != IntPtr.Zero)
                                    {
                                        hwnd = proc.MainWindowHandle;
                                        dict.Add(proc.Id, hwnd.ToInt64());
                                    }
                                    else
                                    {
                                        hwnd = new IntPtr(dict.Find(pid));
                                        if (hwnd == IntPtr.Zero || !Extentions.IsWindow(hwnd))
                                        {
                                            Console.WriteLine("无法获取指定进程的主窗口句柄。");
                                            return 0;
                                        }
                                    }
                                    hwnd.ShowWindow(style);
                                }
                                else
                                {
                                    Console.WriteLine("不存在指定进程。");
                                }
                            }
                            else
                            {
                                Console.WriteLine("无效参数:{0}", args[0]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("命令语法不正确。");
                            return 1;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("命令语法不正确。");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }
            return 0;
        }
    }
}
