#SingleInstance, Force
#NoEnv
SetWorkingDir, %A_ScriptDir%
SetKeyDelay, 0

WinGet, windows, List

Loop, %windows% {
    WinGet, pid, PID, % "ahk_id " windows%A_Index%
    WinGetTitle, title, ahk_pid %pid%
    if (InStr(title, "Minecraft")) {
        WinMinimize, ahk_pid %pid%
        ControlSend,, {Blind}{Tab 8}{Enter}, ahk_pid %pid%
    }
}
