using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using MCSRLauncherBackup;

namespace MCSRLauncherBackup
{
    internal class MiscFunctionality
    {
        public static void CheckBoxGetter()
        {
            string[] Checkboxes = File.ReadAllLines(Environment.CurrentDirectory + "\\Data\\CheckBoxes.txt");

            Settings.settings_sync = Convert.ToBoolean(Checkboxes[0]);
            Settings.start_ninjabrain = Convert.ToBoolean(Checkboxes[1]);
            Settings.start_Tracker = Convert.ToBoolean(Checkboxes[2]);
            Settings.start_instances = Convert.ToBoolean(Checkboxes[3]);
            Settings.reset_macro = Convert.ToBoolean(Checkboxes[4]);
            Settings.start_obs = Convert.ToBoolean(Checkboxes[5]);
            Settings.delete_old_worlds = Convert.ToBoolean(Checkboxes[6]);
            Settings.start_second_obs = Convert.ToBoolean(Checkboxes[7]);
            Settings.start_AddApp1 = Convert.ToBoolean(Checkboxes[8]);
            Settings.start_AddApp2 = Convert.ToBoolean(Checkboxes[9]);
            Settings.start_AddApp3 = Convert.ToBoolean(Checkboxes[10]);
        }
        public static void CheckBoxes(int CheckboxNr, bool Checked)
        {


            switch (CheckboxNr)
            {
                case 1:
                    Settings.settings_sync = Checked;
                    break;
                case 2:
                    Settings.start_ninjabrain = Checked;
                    break;
                case 3:
                    Settings.start_Tracker = Checked;
                    break;
                case 4:
                    Settings.start_instances = Checked;
                    break;
                case 5:
                    Settings.reset_macro = Checked;
                    break;
                case 6:
                    Settings.start_obs = Checked;
                    break;
                case 7:
                    Settings.delete_old_worlds = Checked;
                    break;
                case 8:
                    Settings.start_second_obs = Checked;
                    break;
                case 9:
                    Settings.start_AddApp1 = Checked;
                    break;
                case 10:
                    Settings.start_AddApp2 = Checked;
                    break;
                case 11:
                    Settings.start_AddApp3 = Checked;
                    break;
                default:
                    break;
            }

            File.WriteAllText(Environment.CurrentDirectory + "\\Data\\CheckBoxes.txt", $"" +
                $"{Settings.settings_sync}" +
                $"\n{Settings.start_ninjabrain}" +
                $"\n{Settings.start_Tracker}" +
                $"\n{Settings.start_instances}" +
                $"\n{Settings.reset_macro}" +
                $"\n{Settings.start_obs}" +
                $"\n{Settings.delete_old_worlds}" +
                $"\n{Settings.start_second_obs}" +
                $"\n{Settings.start_AddApp1}" +
                $"\n{Settings.start_AddApp2}" +
                $"\n{Settings.start_AddApp3}"
                );
        }

        public static void InstanceNumberGetter()
        {
            try
            {
                string instanceCount = File.ReadAllText(Environment.CurrentDirectory + "\\Data\\InstanceCount.txt");

                Settings.instance_count = Convert.ToInt32(instanceCount);
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find InstanceCount.txt");
            }

        }

        public static void GetPathsFromFile()
        {
            try
            {
                string[] pathsInTxt = File.ReadAllLines(Environment.CurrentDirectory + "\\Data\\Paths.txt");

                Settings.StandardSettings = pathsInTxt[0];
                Settings.NinjaBot = pathsInTxt[1];
                Settings.Tracker = pathsInTxt[2];
                Settings.MultiMC = pathsInTxt[3];
                Settings.WallMacro = pathsInTxt[4];
                Settings.OBS = pathsInTxt[5];
                Settings.AddApp1 = pathsInTxt[6];
                Settings.AddApp2 = pathsInTxt[7];
                Settings.AddApp3 = pathsInTxt[8];
            }
            catch (Exception)
            {
                //MessageBox.Show("Could not find Paths.txt");
            }

        }

        public static void SavePathToFile(string applicationName, string inputPath)
        {
            //if (inputPath != "")
            {
                switch (applicationName)
                {
                    case "StandardSettings":
                        Settings.StandardSettings = inputPath;
                        break;
                    case "NinjaBot":
                        Settings.NinjaBot = inputPath;
                        break;
                    case "Tracker":
                        Settings.Tracker = inputPath;
                        break;
                    case "MultiMC":
                        Settings.MultiMC = inputPath;
                        break;
                    case "WallMacro":
                        Settings.WallMacro = inputPath;
                        break;
                    case "OBS":
                        Settings.OBS = inputPath;
                        break;
                    case "AddApp1":
                        Settings.AddApp1 = inputPath;
                        break;
                    case "AddApp2":
                        Settings.AddApp2 = inputPath;
                        break;
                    case "AddApp3":
                        Settings.AddApp3= inputPath;
                        break;
                    default:
                        break;
                }

                File.WriteAllText(Environment.CurrentDirectory + "\\Data\\Paths.txt", $"" +
                    $"{Settings.StandardSettings}\n" +
                    $"{Settings.NinjaBot}\n" +
                    $"{Settings.Tracker}\n" +
                    $"{Settings.MultiMC}\n" +
                    $"{Settings.WallMacro}\n" +
                    $"{Settings.OBS}\n" +
                    $"{Settings.AddApp1}\n" +
                    $"{Settings.AddApp2}\n" +
                    $"{Settings.AddApp3}");
            }
        }

        public static void PathSplitter()
        {
            try
            {
                int a = Settings.NinjaBot.LastIndexOf(@"\");
                Settings.NinjaBotSplit[0] = Settings.NinjaBot.Substring(0, a); Settings.NinjaBotSplit[1] = Settings.NinjaBot.Substring(a + 1);
            }
            catch (Exception)
            {

            }

            try
            {
                int b = Settings.Tracker.LastIndexOf(@"\");
                Settings.TrackerSplit[0] = Settings.Tracker.Substring(0, b); Settings.TrackerSplit[1] = Settings.Tracker.Substring(b + 1);
            }
            catch (Exception)
            {

            }

            try
            {
                int c = Settings.WallMacro.LastIndexOf(@"\");
                Settings.WallMacroSplit[0] = Settings.WallMacro.Substring(0, c); Settings.WallMacroSplit[1] = Settings.WallMacro.Substring(c + 1);
            }
            catch (Exception)
            {

            }

            try
            {
                int d = Settings.OBS.LastIndexOf(@"\");
                Settings.OBSSplit[0] = Settings.OBS.Substring(0, d); Settings.OBSSplit[1] = Settings.OBS.Substring(d + 1);
            }
            catch (Exception)
            {

            }

            try
            {
                int e = Settings.MultiMC.LastIndexOf(@"\");
                Settings.MultiMCSplit[0] = Settings.MultiMC.Substring(0, e); Settings.MultiMCSplit[1] = Settings.MultiMC.Substring(e + 1);
            }
            catch (Exception)
            {

            }

            try
            {
                int f = Settings.StandardSettings.LastIndexOf(@"\");
                Settings.StandardSettingsSplit[0] = Settings.StandardSettings.Substring(0, f); Settings.StandardSettingsSplit[1] = Settings.StandardSettings.Substring(f + 1);
            }
            catch (Exception)
            {

            }

            try
            {
                int a = Settings.AddApp1.LastIndexOf(@"\");
                Settings.AddApp1Split[0] = Settings.AddApp1.Substring(0, a); Settings.AddApp1Split[1] = Settings.AddApp1.Substring(a + 1);
            }
            catch (Exception)
            {

            }

            try
            {
                int a = Settings.AddApp2.LastIndexOf(@"\");
                Settings.AddApp2Split[0] = Settings.AddApp2.Substring(0, a); Settings.AddApp2Split[1] = Settings.AddApp2.Substring(a + 1);
            }
            catch (Exception)
            {

            }

            try
            {
                int a = Settings.AddApp3.LastIndexOf(@"\");
                Settings.AddApp3Split[0] = Settings.AddApp3.Substring(0, a); Settings.AddApp3Split[1] = Settings.AddApp3.Substring(a + 1);
            }
            catch (Exception)
            {

            }

        }

        public static void WallBypassChanger(bool wallByPassOn)
        {
            string wallSettings = File.ReadAllText(Settings.WallMacroSplit[0] + "\\settings.ahk"), newWallSettings = "";

            if (wallByPassOn == false)
            {
                newWallSettings = wallSettings.Replace("global mode := \"B\" ;", "global mode := \"W\" ;");
            }
            else if (wallByPassOn == true)
            {
                newWallSettings = wallSettings.Replace("global mode := \"W\" ;", "global mode := \"B\" ;");
            }

            File.WriteAllText(Settings.WallMacroSplit[0] + "\\settings.ahk", newWallSettings);
        }

        public static bool ByPassCheckInitalizer(bool errorMessage)
        {
            try
            {
                string wallSettings = File.ReadAllText(Settings.WallMacroSplit[0] + "\\settings.ahk");

                bool isChecked = false;

                isChecked = wallSettings.Contains("global mode := \"B\" ;");

                return isChecked;
            }
            catch (Exception)
            {
                if (errorMessage)
                {
                    MessageBox.Show("WallMacro path missing or invalid.");
                }
                return false;
            }
        }
    }
}
