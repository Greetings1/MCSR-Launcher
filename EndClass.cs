using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MCSRLauncherBackup;
using System.Runtime.CompilerServices;

namespace MCSRLauncherBackup
{
    internal class EndClass
    {
        public static bool DOWDone = false;
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

            Thread DOWthread = new Thread(world_deletion);
            DOWthread.IsBackground = true;
            Thread DOWLoadingthread = new Thread(DOWLoading);
            DOWLoadingthread.IsBackground = true;
            DOWLoadingthread.Start();
            DOWthread.Start();
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
                        //DirectoryInfo dir = new DirectoryInfo($"{Settings.MultiMCSplit[0]}\\instances\\{Settings.Instance_Format}{i}\\.minecraft\\saves");


                        var directories = Directory.GetDirectories(Settings.MultiMCSplit[0] + "\\instances");

                        foreach (var item in directories)
                        {
                            try
                            {
                                string instancecfg = File.ReadAllText(item + "\\instance.cfg");
                                if (instancecfg.Contains($"name={Settings.Instance_Format}{i}"))
                                {
                                    DirectoryInfo dir = new DirectoryInfo(item + "\\.minecraft\\saves");

                                    foreach (var saveFile in dir.GetDirectories())
                                    {
                                        if (Regex.IsMatch(saveFile.Name, @"^([a-öA-Ö]{0,20}\s?Speedrun\s?#[0-9]{1,7}$)|(^[Nn]ew [Ww]orld$)"))
                                        {
                                            saveFile.Delete(true);
                                            //Console.WriteLine("Deleted \"" + saveFile.Name + "\" in instance " + i);
                                            deleted_worlds++;
                                        }
                                    }
                                }
                            }
                            catch (Exception){}

                        }



                    }
                    int deletedWorldsFromFile;
                    try
                    {
                        deletedWorldsFromFile = Convert.ToInt32(File.ReadAllText(Environment.CurrentDirectory + "\\Data\\DeletedWorlds.txt"));
                    }
                    catch (Exception)
                    {
                        deletedWorldsFromFile = 0;
                        File.WriteAllText(Environment.CurrentDirectory + "\\Data\\DeletedWorlds.txt", deletedWorldsFromFile.ToString());
                    }

                    deletedWorldsFromFile = deletedWorldsFromFile + deleted_worlds;
                    File.WriteAllText(Environment.CurrentDirectory + "\\Data\\DeletedWorlds.txt", deletedWorldsFromFile.ToString());
                    MessageBox.Show("This session : " + deleted_worlds + "\nTotal: " + deletedWorldsFromFile, "Deleted worlds    ");

                }
                catch (Exception)
                {
                    MessageBox.Show("Could not delete saves. Path missing or invalid.");
                }
                //catch ()
                //{

                //}
                DOWDone = true;
            }
        }

        public static void DOWLoading()
        {
            if (Settings.delete_old_worlds)
            {
                while (DOWDone == false)
                {
                    Application.Current.Dispatcher.Invoke(() => { MainWindow.AppWindow.WorldDeletionsTbk.Text = "Deleting worlds"; });
                    Thread.Sleep(600);
                    Application.Current.Dispatcher.Invoke(() => { MainWindow.AppWindow.WorldDeletionsTbk.Text = "Deleting worlds ."; });
                    Thread.Sleep(600);
                    Application.Current.Dispatcher.Invoke(() => { MainWindow.AppWindow.WorldDeletionsTbk.Text = "Deleting worlds . ."; });
                    Thread.Sleep(600);
                    Application.Current.Dispatcher.Invoke(() => { MainWindow.AppWindow.WorldDeletionsTbk.Text = "Deleting worlds . . ."; });
                    Thread.Sleep(600);
                }

                Application.Current.Dispatcher.Invoke(() => { MainWindow.AppWindow.WorldDeletionsTbk.Text = ""; });
                Application.Current.Dispatcher.Invoke(() => { MainWindow.AppWindow.DOWDonetbk.Visibility = Visibility.Visible; });
                Thread.Sleep(5000);
                Application.Current.Dispatcher.Invoke(() => { MainWindow.AppWindow.DOWDonetbk.Visibility = Visibility.Hidden; });

                //while (DOWDone == false)
                //    {
                //        MainWindow.AppWindow.WorldDeletionsTbk.Text = "Deleting worlds";
                //        Thread.Sleep(600);
                //        MainWindow.AppWindow.WorldDeletionsTbk.Text = "Deleting worlds .";
                //        Thread.Sleep(600);
                //        MainWindow.AppWindow.WorldDeletionsTbk.Text = "Deleting worlds . .";
                //        Thread.Sleep(600);
                //        MainWindow.AppWindow.WorldDeletionsTbk.Text = "Deleting worlds . . .";
                //        Thread.Sleep(600);
                //    }
                //    DOWDone = false;
                //    MainWindow.AppWindow.WorldDeletionsTbk.Text = "";
                //    MainWindow.AppWindow.DOWDonetbk.Visibility = Visibility.Visible;
                //    Thread.Sleep(5000);
                //    MainWindow.AppWindow.DOWDonetbk.Visibility = Visibility.Hidden;




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
