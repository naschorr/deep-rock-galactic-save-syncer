<p align="center"><img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/master/resources/images/logo.png" width="150"/></p>

# Deep Rock Galactic Save Syncer
![tests workflow](https://github.com/naschorr/deep-rock-galactic-save-syncer/actions/workflows/tests.yml/badge.svg)

Easily keep your Steam and Xbox save files for Deep Rock Galactic synced up! That means no more lost progress when swapping between platforms to play with your friends.

<p align="center"><img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/master/resources/images/example.png" width="800"/></p>

> Here's an example of the DRGSS interface

### What?
In a perfect world, DRG would be fully cross platform, and people could play together without any issues. However, we don't live in a perfect world. Enter Deep Rock Galactic Save Syncer (DRGSS)!

Instead of dealing with the hassle of manually transferring save files and keeping track of names, DRGSS handles all of that automagically with a single click!

### Features
#### Automatic DRG save file discovery for Steam and Xbox installations
There's no need to configure anything, just install and go!

#### Smart save file comparison
The save file that's progressed the most is automatically chosen to overwrite the other. In the screenshot above, the Steam save file's Driller has one more promotion than the Xbox save file, and so the Steam save file will be kept.

Granular save file comparison lets you see what's changed at a glance:

<p align="center"><img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/main/resources/images/example_comparison.png" /></p>

> Notice that the Driller on the left has one more promotion than the one on the right, which the comparison in the middle `>` confirms.

#### One-click operation by default
The big button at the bottom lets you know what's going to happen:

<p align="center"><img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/main/resources/images/example_button_steam_xbox.png" /></p>

> Clicking this will overwrite your Steam save file with your Xbox save file

<p align="center"><img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/main/resources/images/example_button_xbox_steam.png" /></p>

> Clicking this will overwrite your Xbox save file with your Steam save file

#### Override the defaults and choose the save you want to keep
Don't agree with DRGSS' choice? Click the save you'd like to keep to override it!

#### Quickly open your save file's directory in the explorer
Access the save files yourself with a single click! Each save file has a <img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/main/resources/icons_high_contrast/arrow_icon.png" width="24" /> button that'll open it for you.

#### Save files are backed up during the sync operation
Chose the wrong file accidentally? Just delete the invalid one, and rename the `*.backup` file back to the original name.

#### Refresh the save files
Has something changed? Hit the <img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/main/resources/icons_high_contrast/resupplier_icon.png" width="24" /> button in the top-left corner of the interface and see the latest save file changes.

#### Divergent save file detection
If you've made progress with both save files separately, DRGSS will alert you of this via the button at the bottom:

<p align="center"><img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/main/resources/images/example_button_divergent.png" /></p>

> The button is disabled, because DRGSS isn't sure of which save file should be overwritten

To fix this, simply pick a save file to keep, and the button will become enabled again.

### Caveats
You must have an existing save in both the Steam and Xbox versions of DRG. This will not work for transferring saves before starting the game up for the first time on a new platform.

### Kudos
This project depends on these neat projects:
- [Electron.NET](https://github.com/ElectronNET/Electron.NET)
- [Blazored.Modal](https://github.com/Blazored/Modal)
- [CssBuilder](https://github.com/justforfun-click/CssBuilder)
- [Gameloop.Vdf](https://github.com/Shravan2x/Gameloop.Vdf)

Assets:
- The DRG icons were pulled from the [wiki](https://deeprockgalactic.fandom.com/wiki/Deep_Rock_Galactic_Wiki)
- The DRG title font is called Danger Flight, and can be found [here](http://www.iconian.com/fonts2/dangerflight.zip)
- [Steam icons created by Hight Quality Icons - Flaticon](https://www.flaticon.com/free-icons/steam)
- [Xbox icons created by Freepik - Flaticon](https://www.flaticon.com/free-icons/xbox)
- [Document icons created by Freepik - Flaticon](https://www.flaticon.com/free-icons/document)
- [Navigation icons created by Smashicons - Flaticon](https://www.flaticon.com/free-icons/navigation)
