using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace Session_Control
{
    internal class MiscFunctionality
    {
        public static void CheckBoxGetter()
        {
            string[] Checkboxes = File.ReadAllLines(Environment.CurrentDirectory + "\\CheckBoxes.txt");

            Settings.settings_sync = Convert.ToBoolean(Checkboxes[0]);
            Settings.start_ninjabrain = Convert.ToBoolean(Checkboxes[1]);
            Settings.start_Tracker = Convert.ToBoolean(Checkboxes[2]);
            Settings.start_instances = Convert.ToBoolean(Checkboxes[3]);
            Settings.reset_macro = Convert.ToBoolean(Checkboxes[4]);
            Settings.start_obs = Convert.ToBoolean(Checkboxes[5]);
            Settings.delete_old_worlds = Convert.ToBoolean(Checkboxes[6]);
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
                default:
                    break;
            }

            File.WriteAllText(Environment.CurrentDirectory + "\\CheckBoxes.txt", $"" +
                $"{Settings.settings_sync}" +
                $"\n{Settings.start_ninjabrain}" +
                $"\n{Settings.start_Tracker}" +
                $"\n{Settings.start_instances}" +
                $"\n{Settings.reset_macro}" +
                $"\n{Settings.start_obs}" +
                $"\n{Settings.delete_old_worlds}");
        }

        public static void InstanceNumberGetter()
        {
            try
            {
                string instanceCount = File.ReadAllText(Environment.CurrentDirectory + "\\InstanceCount.txt");

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
                string[] pathsInTxt = File.ReadAllLines(Environment.CurrentDirectory + "\\Paths.txt");

                Settings.StandardSettings = pathsInTxt[0];
                Settings.NinjaBot = pathsInTxt[1];
                Settings.Tracker = pathsInTxt[2];
                Settings.MultiMC = pathsInTxt[3];
                Settings.WallMacro = pathsInTxt[4];
                Settings.OBS = pathsInTxt[5];
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find Paths.txt");
            }

        }

        public static void SavePathToFile(string applicationName, string inputPath)
        {
            if (inputPath != "")
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
                    default:
                        break;
                }

                File.WriteAllText(Environment.CurrentDirectory + "\\Paths.txt", $"{Settings.StandardSettings}\n{Settings.NinjaBot}\n{Settings.Tracker}\n{Settings.MultiMC}\n{Settings.WallMacro}\n{Settings.OBS}");
            }
        }

        public static void PathSplitter()
        {
            try
            {
                int a = Settings.NinjaBot.LastIndexOf(@"\");
                Settings.NinjaBotSplit[0] = Settings.NinjaBot.Substring(0, a); Settings.NinjaBotSplit[1] = Settings.NinjaBot.Substring(a + 1);

                int b = Settings.Tracker.LastIndexOf(@"\");
                Settings.TrackerSplit[0] = Settings.Tracker.Substring(0, b); Settings.TrackerSplit[1] = Settings.Tracker.Substring(b + 1);

                int c = Settings.WallMacro.LastIndexOf(@"\");
                Settings.WallMacroSplit[0] = Settings.WallMacro.Substring(0, c); Settings.WallMacroSplit[1] = Settings.WallMacro.Substring(c + 1);

                int d = Settings.OBS.LastIndexOf(@"\");
                Settings.OBSSplit[0] = Settings.OBS.Substring(0, d); Settings.OBSSplit[1] = Settings.OBS.Substring(d + 1);

                int e = Settings.MultiMC.LastIndexOf(@"\");
                Settings.MultiMCSplit[0] = Settings.MultiMC.Substring(0, e); Settings.MultiMCSplit[1] = Settings.MultiMC.Substring(e + 1);

                int f = Settings.StandardSettings.LastIndexOf(@"\");
                Settings.StandardSettingsSplit[0] = Settings.StandardSettings.Substring(0, f); Settings.StandardSettingsSplit[1] = Settings.StandardSettings.Substring(f + 1);
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find valid paths");
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
