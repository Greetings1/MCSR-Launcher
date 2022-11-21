using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using MCSRLauncherBackup;

namespace MCSRLauncherBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;
        public MainWindow()
        {
            InitializeComponent();
            AppWindow = this;

            CheckBoxSetter();

            MiscFunctionality.InstanceNumberGetter();

            cboNumZones.SelectedValue = Settings.instance_count;

            MiscFunctionality.GetPathsFromFile();

            MiscFunctionality.PathSplitter();

            try
            {
                InstanceFormatTextBox.Text = File.ReadAllText(Environment.CurrentDirectory + "\\Data\\InstanceFormat.txt");
                Settings.Instance_Format = File.ReadAllText(Environment.CurrentDirectory + "\\Data\\InstanceFormat.txt");
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find InstanceFormat.txt");
            }

            try
            {
                string[] temp = File.ReadAllLines(Environment.CurrentDirectory + "\\Data\\OBSSceneFormat.txt");
                OBSSceneFormat1TextBox.Text = $"{temp[0]}";
                try
                {
                    OBSSceneFormat2TextBox.Text = $"{temp[1]}";
                }
                catch (Exception)
                {
                    OBSSceneFormat2TextBox.Text = "";
                }


                Settings.OBSSceneName1 = $"{temp[0]}";
                try
                {
                    Settings.OBSSceneName2 = $"{temp[1]}";
                }
                catch (Exception)
                {
                    Settings.OBSSceneName2 = "";
                }


            }
            catch (Exception)
            {
                MessageBox.Show("Could not find OBSSceneFormat.txt");
            }

            WallByPasscbx.IsChecked = MiscFunctionality.ByPassCheckInitalizer(false);

            FileNameChanger();
        }
        public int inst;
        ProcessStartInfo MCtask;
        Process proc;

        private void Start_btn(object sender, RoutedEventArgs e)
        {
            Settings.Instance_Format = InstanceFormatTextBox.Text;
            StartClass.Start1();
            instance_startup();
            StartClass.Start2();
        }

        private void Kill_btn(object sender, RoutedEventArgs e)
        {
            EndClass.End();
        }

        private void SyStSebtn_Click(object sender, RoutedEventArgs e)
        {
            StartClass.SyncSettings();
        }

        private void Settingsbtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxGrid.Visibility == Visibility.Visible)
            {
                CheckBoxGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                CheckBoxGrid.Visibility = Visibility.Visible;
                //PathSetterGrid.Visibility = Visibility.Collapsed;
                //StandardSettingsGrid.Visibility = Visibility.Collapsed;
                //MacroGrid.Visibility = Visibility.Collapsed;
                //GeneralOptionsGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void SyncSettingsCbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(1, Convert.ToBoolean(SyncSettingsCbx.IsChecked));
        }

        private void NinBotCbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(2, Convert.ToBoolean(NinBotCbx.IsChecked));
        }

        private void TrackerCbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(3, Convert.ToBoolean(TrackerCbx.IsChecked));
        }

        private void InstancesCbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(4, Convert.ToBoolean(InstancesCbx.IsChecked));
        }

        private void MacroCbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(5, Convert.ToBoolean(MacroCbx.IsChecked));
        }

        private void ObsCbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(6, Convert.ToBoolean(ObsCbx.IsChecked));
        }

        private void DelOldWorCbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(7, Convert.ToBoolean(DelOldWorCbx.IsChecked));
        }

        private void SecondOBS_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(8, Convert.ToBoolean(SecondOBScbx.IsChecked));
        }

        private void CheckBoxSetter()
        {
            try
            {
                string[] CheckboxState = File.ReadAllLines(Environment.CurrentDirectory + "\\Data\\CheckBoxes.txt");

                SyncSettingsCbx.IsChecked = Convert.ToBoolean(CheckboxState[0]);
                NinBotCbx.IsChecked = Convert.ToBoolean(CheckboxState[1]);
                TrackerCbx.IsChecked = Convert.ToBoolean(CheckboxState[2]);
                InstancesCbx.IsChecked = Convert.ToBoolean(CheckboxState[3]);
                MacroCbx.IsChecked = Convert.ToBoolean(CheckboxState[4]);
                ObsCbx.IsChecked = Convert.ToBoolean(CheckboxState[5]);
                DelOldWorCbx.IsChecked = Convert.ToBoolean(CheckboxState[6]);
                SecondOBScbx.IsChecked = Convert.ToBoolean(CheckboxState[7]);
                AddApp1cbx.IsChecked = Convert.ToBoolean(CheckboxState[8]);
                AddApp2cbx.IsChecked = Convert.ToBoolean(CheckboxState[9]);
                AddApp3cbx.IsChecked = Convert.ToBoolean(CheckboxState[10]);
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find Checkboxes.txt");
            }

        }

        //public void instance_startup_thread()
        //{
        //    Thread thread = new Thread(instance_startup);
        //    thread.IsBackground = true;
        //    thread.Start();
        //}

        public void instance_startup()
        {
            if (Settings.start_instances)
            {
                if (Settings.MultiMC != "")
                {
                    try
                    {
                        for (int i = 1; i <= Settings.instance_count; i++)
                        {
                            //Console.Write($"Starting {Settings.Instance_Format}{i}...");
                            Thread.Sleep(100);
                            ProcessStarter(i);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    inst = 1;
                }
                else
                {
                    MessageBox.Show("Could not start instances. Path or instance format missing or invalid.");
                }


            }
        }

        public void ProcessStarter(int i)
        {
            MCtask = new ProcessStartInfo();
            MCtask.FileName = "cmd.exe";
            MCtask.CreateNoWindow = true;
            MCtask.WindowStyle = ProcessWindowStyle.Hidden;
            MCtask.Arguments = "/c " + $"{Settings.MultiMCSplit[1]} -l {Settings.Instance_Format}{i}";
            MCtask.RedirectStandardInput = true;
            MCtask.RedirectStandardOutput = true;
            MCtask.RedirectStandardError = true;
            MCtask.UseShellExecute = false;
            MCtask.WorkingDirectory = Settings.MultiMCSplit[0];

            Thread thread = new Thread(RunProcess);
            thread.IsBackground = true;
            thread.Start();
        }
        private void RunProcess()
        {
            proc = new Process();
            proc.StartInfo = MCtask;
            inst++;
            proc.Start();
            proc.WaitForExit();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void cboNumZonesChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.instance_count = cboNumZones.SelectedIndex + 1;
            File.WriteAllText(Environment.CurrentDirectory + "\\Data\\InstanceCount.txt", Convert.ToString(Settings.instance_count));
        }

        private void FilePathbtnSSS_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                Settings.StandardSettings = openFileDialog.FileName;
                int f = Settings.StandardSettings.LastIndexOf(@"\");
                Settings.StandardSettingsSplit[0] = Settings.StandardSettings.Substring(0, f); Settings.StandardSettingsSplit[1] = Settings.StandardSettings.Substring(f + 1);
            }

            FileNameChanger();
            MiscFunctionality.SavePathToFile("StandardSettings", openFileDialog.FileName);
        }

        private void PathbtnNinbot_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                Settings.NinjaBot = openFileDialog.FileName;
                int a = Settings.NinjaBot.LastIndexOf(@"\");
                Settings.NinjaBotSplit[0] = Settings.NinjaBot.Substring(0, a); Settings.NinjaBotSplit[1] = Settings.NinjaBot.Substring(a + 1);
            }

            FileNameChanger();
            MiscFunctionality.SavePathToFile("NinjaBot", openFileDialog.FileName);
        }

        private void FilebtnTracker_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                Settings.Tracker = openFileDialog.FileName;
                int b = Settings.Tracker.LastIndexOf(@"\");
                Settings.TrackerSplit[0] = Settings.Tracker.Substring(0, b); Settings.TrackerSplit[1] = Settings.Tracker.Substring(b + 1);
            }

            FileNameChanger();
            MiscFunctionality.SavePathToFile("Tracker", openFileDialog.FileName);
        }

        private void PathbtnMultiMC_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                Settings.MultiMC = openFileDialog.FileName;
                int f = Settings.MultiMC.LastIndexOf(@"\");
                Settings.MultiMCSplit[0] = Settings.MultiMC.Substring(0, f); Settings.MultiMCSplit[1] = Settings.MultiMC.Substring(f + 1);
            }

            FileNameChanger();
            MiscFunctionality.SavePathToFile("MultiMC", openFileDialog.FileName);
        }

        private void PathbtnTheWall_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                Settings.WallMacro = openFileDialog.FileName;
                int c = Settings.WallMacro.LastIndexOf(@"\");
                Settings.WallMacroSplit[0] = Settings.WallMacro.Substring(0, c); Settings.WallMacroSplit[1] = Settings.WallMacro.Substring(c + 1);
            }

            FileNameChanger();
            MiscFunctionality.SavePathToFile("WallMacro", openFileDialog.FileName);
            WallByPasscbx.IsChecked = MiscFunctionality.ByPassCheckInitalizer(false);
        }

        private void PathbtnOBS_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                Settings.OBS = openFileDialog.FileName;
                int d = Settings.OBS.LastIndexOf(@"\");
                Settings.OBSSplit[0] = Settings.OBS.Substring(0, d); Settings.OBSSplit[1] = Settings.OBS.Substring(d + 1);
            }

            FileNameChanger();
            MiscFunctionality.SavePathToFile("OBS", openFileDialog.FileName);
        }

        private void PathHider_Click(object sender, RoutedEventArgs e)
        {
            if (PathSetterGrid.Visibility == Visibility.Visible)
            {
                PathSetterGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                PathSetterGrid.Visibility = Visibility.Visible;
                //CheckBoxGrid.Visibility = Visibility.Collapsed;
                //StandardSettingsGrid.Visibility = Visibility.Collapsed;
                //MacroGrid.Visibility = Visibility.Collapsed;
                //GeneralOptionsGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void InstanceFormatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            File.WriteAllText(Environment.CurrentDirectory + "\\Data\\InstanceFormat.txt", InstanceFormatTextBox.Text);
        }

        private void MacroReloadbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartClass.processStarter(Settings.WallMacroSplit[0], Settings.WallMacroSplit[1]);
            }
            catch (Exception)
            {
                MessageBox.Show("Could not reload WallMacro. Path missing or invalid.");
            }

        }

        private void SSSOpenbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartClass.processStarter(Settings.StandardSettingsSplit[0], Settings.StandardSettingsSplit[1]);
            }
            catch (Exception)
            {
                MessageBox.Show("Could not open Standard Settings. Path missing or invalid.");
            }

        }

        private void WallByPasscbx_Checked(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(WallByPasscbx.IsChecked) == true)
            {
                try
                {
                    MiscFunctionality.WallBypassChanger(true);
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not enable WallBypass. WallMacro Path missing or invalid.");
                }

            }
            else if (Convert.ToBoolean(WallByPasscbx.IsChecked) == false)
            {
                try
                {
                    MiscFunctionality.WallBypassChanger(false);
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not disable WallBypass. WallMacro Path missing or invalid.");
                }

            }
        }

        public void FileNameChanger()
        {
            try
            {
                SSSTextbox.Text = Settings.StandardSettingsSplit[1];
                NinBotTextBox.Text = Settings.NinjaBotSplit[1];
                TrackerTextBox.Text = Settings.TrackerSplit[1];
                MultiMCTextBox.Text = Settings.MultiMCSplit[1];
                WallMacroTextBox.Text = Settings.WallMacroSplit[1];
                OBSTextBox.Text = Settings.OBSSplit[1];
                AddApp1TextBox.Text = Settings.AddApp1Split[1];
                AddApp2TextBox.Text = Settings.AddApp2Split[1];
                AddApp3TextBox.Text = Settings.AddApp3Split[1];
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find filenames. Path missing or invalid.");
            }

        }

        private void StandardSettingsandMacro_Click(object sender, RoutedEventArgs e)
        {
            if (StandardSettingsGrid.Visibility == Visibility.Visible)
            {
                StandardSettingsGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                StandardSettingsGrid.Visibility = Visibility.Visible;
                //CheckBoxGrid.Visibility = Visibility.Collapsed;
                //PathSetterGrid.Visibility = Visibility.Collapsed;
                //MacroGrid.Visibility = Visibility.Collapsed;
                //GeneralOptionsGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void Macro_Click(object sender, RoutedEventArgs e)
        {
            if (MacroGrid.Visibility == Visibility.Visible)
            {
                MacroGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                MacroGrid.Visibility = Visibility.Visible;
                //StandardSettingsGrid.Visibility = Visibility.Collapsed;
                //CheckBoxGrid.Visibility = Visibility.Collapsed;
                //PathSetterGrid.Visibility = Visibility.Collapsed;
                //GeneralOptionsGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void GeneralOptions_Click(object sender, RoutedEventArgs e)
        {
            if (GeneralOptionsGrid.Visibility == Visibility.Visible)
            {
                GeneralOptionsGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                GeneralOptionsGrid.Visibility = Visibility.Visible;
                //StandardSettingsGrid.Visibility = Visibility.Collapsed;
                //CheckBoxGrid.Visibility = Visibility.Collapsed;
                //PathSetterGrid.Visibility = Visibility.Collapsed;
                //MacroGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void AddApp1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                Settings.AddApp1 = openFileDialog.FileName;
                int d = Settings.AddApp1.LastIndexOf(@"\");
                Settings.AddApp1Split[0] = Settings.AddApp1.Substring(0, d); Settings.AddApp1Split[1] = Settings.AddApp1.Substring(d + 1);
            }

            FileNameChanger();
            MiscFunctionality.SavePathToFile("AddApp1", openFileDialog.FileName);
        }

        private void AddApp2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                Settings.AddApp2 = openFileDialog.FileName;
                int d = Settings.AddApp2.LastIndexOf(@"\");
                Settings.AddApp2Split[0] = Settings.AddApp2.Substring(0, d); Settings.AddApp2Split[1] = Settings.AddApp2.Substring(d + 1);
            }

            FileNameChanger();
            MiscFunctionality.SavePathToFile("AddApp2", openFileDialog.FileName);
        }

        private void AddApp3_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                Settings.AddApp3 = openFileDialog.FileName;
                int d = Settings.AddApp3.LastIndexOf(@"\");
                Settings.AddApp3Split[0] = Settings.AddApp3.Substring(0, d); Settings.AddApp3Split[1] = Settings.AddApp3.Substring(d + 1);
            }

            FileNameChanger();
            MiscFunctionality.SavePathToFile("AddApp3", openFileDialog.FileName);
        }

        private void AddApp1cbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(9, Convert.ToBoolean(AddApp1cbx.IsChecked));
        }

        private void AddApp2cbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(10, Convert.ToBoolean(AddApp2cbx.IsChecked));
        }

        private void AddApp3cbx_Checked(object sender, RoutedEventArgs e)
        {
            MiscFunctionality.CheckBoxes(11, Convert.ToBoolean(AddApp3cbx.IsChecked));
        }

        private void OBSSceneFormat1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] OBSSceneNames = File.ReadAllLines(Environment.CurrentDirectory + "\\Data\\OBSSceneFormat.txt");

            try
            {
                File.WriteAllText(Environment.CurrentDirectory + "\\Data\\OBSSceneFormat.txt", $"{OBSSceneFormat1TextBox.Text}\n{OBSSceneNames[1]}");
            }
            catch (Exception)
            {
                File.WriteAllText(Environment.CurrentDirectory + "\\Data\\OBSSceneFormat.txt", $"{OBSSceneFormat1TextBox.Text}\n");
            }

        }

        private void OBSSceneFormat2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] OBSSceneNames = File.ReadAllLines(Environment.CurrentDirectory + "\\Data\\OBSSceneFormat.txt");

            File.WriteAllText(Environment.CurrentDirectory + "\\Data\\OBSSceneFormat.txt", $"{OBSSceneNames[0]}\n{OBSSceneFormat2TextBox.Text}");
        }
    }
}
