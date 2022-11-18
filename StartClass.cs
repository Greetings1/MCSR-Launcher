using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Session_Control
{
    internal class StartClass
    {
        public static void Start1()
        {
            SyncSettings();
            launcher();
            programs();
        }
        public static void Start2()
        {
            enterThread();
            //enter();

        }

        public static void SyncSettings()
        {
            if (Settings.settings_sync)
            {
                try
                {
                    string standardSettingsString = File.ReadAllText(Settings.StandardSettings);
                    for (int i = 1; i <= Settings.instance_count; i++)
                    {
                        File.WriteAllText($"{Settings.MultiMCSplit[0]}\\instances\\{Settings.Instance_Format}{i}\\.minecraft\\config\\standardoptions.txt", standardSettingsString);
                    }
                    //Console.WriteLine("Syncing Standardsettings");
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not sync Settings. Path missing or invalid.");
                }

            }
        }

        public static void launcher()
        {
            if (Settings.start_instances)
            {
                try
                {
                    processStarter(Settings.MultiMCSplit[0], $"start MultiMC.exe");
                    Thread.Sleep(2000);

                    //Console.WriteLine("Starting MultiMC");
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not run MultiMC. Path missing or invalid.");
                }
            }


        }

        public static void programs()
        {
            if (Settings.start_ninjabrain)
            {
                try
                {
                    processStarter(Settings.NinjaBotSplit[0], $"javaw -jar {Settings.NinjaBotSplit[1]}");
                    //Console.WriteLine("Starting NinBot");
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not run Ninjabrain Bot. Path missing or invalid.");
                }

            }
            if (Settings.start_Tracker)
            {
                try
                {
                    processStarter(Settings.TrackerSplit[0], $"start {Settings.TrackerSplit[1]}");
                    //Console.WriteLine("Starting Tracker");
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not run Tracker. Path missing or invalid.");
                }

            }
        }

        public static void enterThread()
        {
            Thread thread = new Thread(enter);
            thread.IsBackground = true;
            thread.Start();
        }

        public static void enter()
        {
            if (Settings.reset_macro)
            {
                Thread.Sleep(15000);
                while (true)
                {
                    try
                    {
                        int x = 0;
                        for (int i = 1; i <= Settings.instance_count; i++)
                        {
                            var logs = File.Open($@"{Settings.MultiMCSplit[0]}\instances\{Settings.Instance_Format}{i}\.minecraft\logs\latest.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                            StreamReader logReader = new(logs);

                            try
                            {
                                string[] logLines = logReader.ReadToEnd().Split("\n");

                                if (logLines[logLines.Length - 2].Contains("minecraft:textures/atlas/mob_effects.png-atlas"))
                                {
                                    x++;
                                }
                            }
                            catch (Exception)
                            {

                            }
                            Thread.Sleep(100);

                        }
                        Thread.Sleep(2000);
                        if (x == Settings.instance_count)
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Could not find logs. Path missing or invalid.");
                        break;
                    }
                }

                Thread.Sleep(1000);

                processStarter(Environment.CurrentDirectory, "enter.ahk");

                while (true)
                {
                    try
                    {
                        int x = 0;
                        for (int i = 1; i <= Settings.instance_count; i++)
                        {
                            var logs = File.Open($"{Settings.MultiMCSplit[0]}\\instances\\{Settings.Instance_Format}{i}\\.minecraft\\logs\\latest.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                            StreamReader logReader = new(logs);

                            if (logReader.ReadToEnd().Contains("joined the game"))
                            {
                                x++;
                            }
                            Thread.Sleep(100);
                        }
                        if (x == Settings.instance_count)
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Could not find logs. Path missing or invalid.");
                        break;
                    }
                }

                Thread.Sleep(3000);
                processStarter(Environment.CurrentDirectory, "paus.ahk");
            }

            macro_startup();
            obs_startup();
            //EndClass.killTask("MultiMC", false);
        }

        public static void macro_startup()
        {
            if (Settings.reset_macro)
            {
                try
                {
                    processStarter(Settings.WallMacroSplit[0], Settings.WallMacroSplit[1]);
                    //Console.WriteLine("Starting reset macro...");
                    Thread.Sleep(3 + ((Settings.instance_count * 1000) / 4));
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not run WallMacro. Path missing or invalid.");
                }
            }
        }

        public static void obs_startup()
        {
            if (Settings.start_obs)
            {
                try
                {
                    processStarter(Settings.OBSSplit[0], Settings.OBSSplit[1] + " --scene \"Source Record\"");
                    //Console.WriteLine("Starting OBS...");
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not run OBS. Path missing or invalid.");
                }
            }
        }


        public static void processStarter(string directoryLocation, string cmdCommand)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "/c " + cmdCommand;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WorkingDirectory = directoryLocation;
            Thread.Sleep(200);
            Process.Start(startInfo);
        }
    }
}
