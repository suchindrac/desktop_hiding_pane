# Desktop Hiding Pane

## Summary:

This is an app which helps people whose eyes are more sensitive to light. There are a lot of people who use dark 
 desktops. This is what made the so called dark-apps a craze. Honestly, this not only reduces power consumed by monitor,
 but also reduces the stress on eyes. This app creates a dark pane which covers most of the screen, except what can be 
 called the work area. The darkness of the pane, the size of the work area and the location of the work area can be 
 controlled in real time through keyboard shortcuts. Most importantly, it does not cause any trouble to the user in
 terms of work done on desktop


## Usage:

There are two binaries, namely hide_pane_normal.exe and hide_pane_inverted.exe, for normal and inverted desktops 
respectively. Upon execution of the binary, you can see that most of the desktop is hidden, except a small work area. 

The following commands can be used to control the work area:
* Movement - Arrow keys
* Horizontally increase/reduce the size of work area - Page Up/Page Down
* Horizontally increase/reduce the size of work area - Insert/Delete
* Focus on windows below the pane - Alt-Tab
* Focus on the pane - Shift+Space
* Increase or reduce the darkness of the pane - i/d
* Invert the work area - v
* Quit - q

_Note: All the above commands work when the pane is focused. When the pane is not focused, normal operations on the 
  other windows are executed as expected_
 
## Compilation/Execution:

_NOTE: The code requires DotNet (specifically csc.exe) to compile_

* Please execute the following commands:

C:\Hide_Pane> compile.bat
  
* Double click on hide_pane_normal.exe or hide_pane_inverted.exe
