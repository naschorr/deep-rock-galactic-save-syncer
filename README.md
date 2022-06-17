<p align="center"><img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/master/resources/icon.png" width="150"/></p>

# Deep Rock Galactic Save Syncer
One-click save syncing between Steam and Xbox (Windows) Deep Rock Galactic installations!

### What?
This application automatically discovers the appropriate save directories, and determines the newest one to keep, and updates any out of date saves with this latest one. It determines which save is newest by both analyzing the save file itself, as well as looking at the age of last modification. This application also backs up the save being overwritten in case something goes wrong. Output is sent to the Windows [Action Center](https://support.microsoft.com/en-us/windows/how-to-open-notification-center-and-quick-settings-f8dc196e-82db-5d67-f55e-ba5586fbb038).

### Configuration
Configuration is usually never needed, but the option is available if necessary.

The available configurables are:
- `steamSavesDir` - path to the directory containing the Steam saves for DRG
- `xboxSavesDir` - Path to the directory containing the Xbox (Windows) saves for DRG

#### How to configure
Configuration is fairly simple
- Create a json file (`config.json`), and put one or both of the configurables into it, like so:
    ```
    {
        "steamSavesDir": "path\\to\\steam\\deep rock galactic\\saves\\directory",
        "xboxSavesDir": "path\\to\\xbox\\deep rock galactic\\saves\\directory"
    }
    ```
- Now create a shortcut to the executable.
- Right-click the shortcut and open the Properties.
- Go to the Shortcut tab.
- In the text box next to the Target field, add `--configPath="\path\to\your\config.json"` to the end of it, replacing the path information with the path to your own custom `config.json` that you just created. Note that if your `config.json` is located alongside the executable then you can simply use `--configPath="config.json"`. Also, make sure that the `--configPath` has a space before it.

This tells the executable where to look to load the config data, and in turn allows it to load your custom options.

### Notes
You must have an existing save in both the Steam and Xbox (Windows) versions of DRG. This will not work for transferring saves before starting the game up for the first time on a new platform.
