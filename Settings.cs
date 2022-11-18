using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session_Control
{
    internal class Settings
    {
        // Configure

        // Set to 1 if you want these things to start up, set to 0 to not do that

        public static bool
            settings_sync,      // Set to 1 if you want it to sync the standartoptions.txt from your settings_folder
            start_ninjabrain,   // Set to 1 if using Ninjabrain bot
            start_Tracker,      // Set to 1 if using reset-tracker
            start_instances,    // Set to 1 to make it start up your instances (only works for MultiMc set to 0 if not using it)
            reset_macro,        // Set to 1 if using a reset macro
            start_obs,          // Set to 1 if using obs
            delete_old_worlds;  // Set to 1 to make it delete your worlds
        public static int
            instance_count; // Change to match amount of instances

        public static string //leave empty if not using
            StandardSettings,
            NinjaBot,
            Tracker,
            MultiMC,
            WallMacro,
            OBS,
            Instance_Format;

        public static string[]
            NinjaBotSplit = new string[2],
            TrackerSplit = new string[2],
            MultiMCSplit = new string[2],
            WallMacroSplit = new string[2],
            OBSSplit = new string[2],
            StandardSettingsSplit = new string[2];

            //instance_format = "Instance",                                            // The name format of your instances (i have instance1, instance2 etc so its 'instance')
            //mmc = @"C:\MultiMC",                                                     // Change this to match your MultiMc.exe location
            //settings_folder = @"C:\Users\Oscar Eriksson\Desktop\MCSR\Startup",   // A folder where your standartoptions.txt is located in (You can just put it in mod_folder)

        // can contain spaces, put double backslash (leave empty "" if not using, would recomend to put the all into the same folder for simplicity)
        //ninja_bot = @"C:\Users\Oscar Eriksson\Desktop\MCSR\Startup",                        // location of your ninja bot
        //reset_tracker = @"C:\Users\Oscar Eriksson\Desktop\MCSR\Startup\resetTracker1.1",   // location of your tracker
        //macro = @"C:\Users\Oscar Eriksson\Desktop\MCSR\MultiResetWall",                     // location of your macro script
        //obs_exe = @"C:\Program Files\obs-studio\bin\64bit",                                  // location of your obs.exe, probably same as mine

        // Name of your Programs, should not contain spaces (leave empty '' if not using)
        //name_macro = "TheWall.ahk",         // Name of your reset Macro
        //name_ninja = "Calc.jar",            // Name of your Ninjabrain-bot
        //name_tracker = "resetTracker.exe",  // Name of your resettracker
        //name_obs = "obs64.exe",               // Name of your obs .bat

        // Don't Configure
        //instance_folder = @$"{mmc}\instances";

    }
}
