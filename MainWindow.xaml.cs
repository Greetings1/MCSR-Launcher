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

namespace Session_Control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CheckBoxSetter();

            MiscFunctionality.InstanceNumberGetter();

            cboNumZones.SelectedValue = Settings.instance_count;

            MiscFunctionality.GetPathsFromFile();

            MiscFunctionality.PathSplitter();

            try
            {
                InstanceFormatTextBox.Text = File.ReadAllText(Environment.CurrentDirectory + "\\InstanceFormat.txt");
                Settings.Instance_Format = File.ReadAllText(Environment.CurrentDirectory + "\\InstanceFormat.txt");
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find InstanceFormat.txt");
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
            if (SyncSettingsCbx.Visibility == Visibility.Visible)
            {
                SyncSettingsCbx.Visibility = Visibility.Hidden;
                NinBotCbx.Visibility = Visibility.Hidden;
                TrackerCbx.Visibility = Visibility.Hidden;
                InstancesCbx.Visibility = Visibility.Hidden;
                MacroCbx.Visibility = Visibility.Hidden;
                ObsCbx.Visibility = Visibility.Hidden;
                DelOldWorCbx.Visibility = Visibility.Hidden;
                cboNumZones.Visibility = Visibility.Hidden;
                InstanceFormatTextBlock.Visibility = Visibility.Hidden;
                InstanceFormatTextBox.Visibility = Visibility.Hidden;
            }
            else
            {
                SyncSettingsCbx.Visibility = Visibility.Visible;
                NinBotCbx.Visibility = Visibility.Visible;
                TrackerCbx.Visibility = Visibility.Visible;
                InstancesCbx.Visibility = Visibility.Visible;
                MacroCbx.Visibility = Visibility.Visible;
                ObsCbx.Visibility = Visibility.Visible;
                DelOldWorCbx.Visibility = Visibility.Visible;
                cboNumZones.Visibility= Visibility.Visible;
                InstanceFormatTextBlock.Visibility = Visibility.Visible;
                InstanceFormatTextBox.Visibility = Visibility.Visible;
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

        private void CheckBoxSetter()
        {
            try
            {
                string[] CheckboxState = File.ReadAllLines(Environment.CurrentDirectory + "\\CheckBoxes.txt");

                SyncSettingsCbx.IsChecked = Convert.ToBoolean(CheckboxState[0]);
                NinBotCbx.IsChecked = Convert.ToBoolean(CheckboxState[1]);
                TrackerCbx.IsChecked = Convert.ToBoolean(CheckboxState[2]);
                InstancesCbx.IsChecked = Convert.ToBoolean(CheckboxState[3]);
                MacroCbx.IsChecked = Convert.ToBoolean(CheckboxState[4]);
                ObsCbx.IsChecked = Convert.ToBoolean(CheckboxState[5]);
                DelOldWorCbx.IsChecked = Convert.ToBoolean(CheckboxState[6]);
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
                    MessageBox.Show("Could not start instances. Path or instance format missing or invalid.");
                }
                inst = 1;

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
            File.WriteAllText(Environment.CurrentDirectory + "\\InstanceCount.txt", Convert.ToString(Settings.instance_count));
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
            if (FilePathbtnSSS.Visibility == Visibility.Visible)
            {
                FilePathbtnSSS.Visibility = Visibility.Hidden;
                SSSTextbox.Visibility = Visibility.Hidden;
                PathbtnNinbot.Visibility = Visibility.Hidden;
                NinBotTextBox.Visibility = Visibility.Hidden;
                FilebtnTracker.Visibility = Visibility.Hidden;
                TrackerTextBox.Visibility = Visibility.Hidden;
                PathbtnMultiMC.Visibility = Visibility.Hidden;
                MultiMCTextBox.Visibility = Visibility.Hidden;
                PathbtnTheWall.Visibility = Visibility.Hidden;
                WallMacroTextBox.Visibility = Visibility.Hidden;
                PathbtnOBS.Visibility = Visibility.Hidden;
                OBSTextBox.Visibility= Visibility.Hidden;
            }
            else
            {
                FilePathbtnSSS.Visibility = Visibility.Visible;
                SSSTextbox.Visibility = Visibility.Visible;
                PathbtnNinbot.Visibility = Visibility.Visible;
                NinBotTextBox.Visibility = Visibility.Visible;
                FilebtnTracker.Visibility = Visibility.Visible;
                TrackerTextBox.Visibility = Visibility.Visible;
                PathbtnMultiMC.Visibility = Visibility.Visible;
                MultiMCTextBox.Visibility = Visibility.Visible;
                PathbtnTheWall.Visibility = Visibility.Visible;
                WallMacroTextBox.Visibility = Visibility.Visible;
                PathbtnOBS.Visibility = Visibility.Visible;
                OBSTextBox.Visibility = Visibility.Visible;
            }
        }

        private void InstanceFormatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            File.WriteAllText(Environment.CurrentDirectory + "\\InstanceFormat.txt", InstanceFormatTextBox.Text);
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
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find filenames. Path missing or invalid.");
            }

        }
    }
}
