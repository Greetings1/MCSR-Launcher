﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Session_Control;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Session_Control
{
    internal class EndClass
    {
        public static void End()
        {
            //Console.WriteLine("Press enter to end session");
            //Console.ReadLine();

            kill();
            Thread_world_deletion();
        }
        public static void kill()
        {
            killTask("javaw", true);
            killTask("AutoHotkey", true);
            killTask("MultiMC", true);
            killTask("obs64", true);
            killTask("resetTracker", true);
        }

        public static void Thread_world_deletion()
        {

            Thread thread = new Thread(world_deletion);
            thread.IsBackground = true;
            thread.Start();
        }

        public static void world_deletion()
        {
            if (Settings.delete_old_worlds)
            {
                try
                {
                    int deleted_worlds = 0;
                    for (int i = 1; i <= Settings.instance_count; i++)
                    {
                        DirectoryInfo dir = new DirectoryInfo($"{Settings.MultiMCSplit[0]}\\instances\\{Settings.Instance_Format}{i}\\.minecraft\\saves");

                        foreach (var saveFile in dir.GetDirectories())
                        {
                            if (Regex.IsMatch(saveFile.Name, @"^(Random Speedrun #[0-9]{1,7}$)|(^New World$)"))
                            {
                                saveFile.Delete(true);
                                //Console.WriteLine("Deleted \"" + saveFile.Name + "\" in instance " + i);
                                deleted_worlds++;
                            }
                        }
                    }
                    int deletedWorldsFromFile = Convert.ToInt32(File.ReadAllText(Environment.CurrentDirectory + "\\DeletedWorlds.txt"));
                    deletedWorldsFromFile = deletedWorldsFromFile + deleted_worlds;
                    File.WriteAllText(Environment.CurrentDirectory + "\\DeletedWorlds.txt", deletedWorldsFromFile.ToString());
                    MessageBox.Show("This session : " + deleted_worlds + "\nTotal: " + deletedWorldsFromFile, "Deleted worlds    ");
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not delete saves. Path missing or invalid.");
                }

            }
        }

        public static void killTask(string taskName, bool killTaskTree)
        {
            foreach (var process in Process.GetProcesses())
            {


                if (process.ProcessName.Contains(taskName))
                {
                    //Console.WriteLine("Killed: " + process.ProcessName);
                    process.Kill(killTaskTree);
                }
            }
        }
    }
}
