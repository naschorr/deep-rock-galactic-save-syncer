<p align="center"><img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/master/resources/icon.png" width="150"/></p>

# Deep Rock Galactic Save Syncer
Easy save syncing between Steam and Xbox Deep Rock Galactic installations!

<p align="center"><img src="https://raw.githubusercontent.com/naschorr/deep-rock-galactic-save-syncer/master/resources/example.png" width="800"/></p>

### What?
This application automatically discovers the appropriate save directories, and determines the newest one to keep, and updates any out of date saves with this latest one. It determines which save is newest by both analyzing the save file itself, as well as looking at the age of last modification. As a bonus, it also backs up the save being overwritten in case something goes wrong.

### How?
By default, the app automatically discover your save files and prompt you to sync the saves. In the screenshot above, you can see that the Steam save file's got a higher level driller, and was played more recently. As such, the sync button mentions that it will sync the Xbox save to the Steam save. At a glance you can see how the save files are different based on the comparison operators between them. If you'd like to override which file is overwritten, you can manually click on one of the save files. This newly selected save file will then overwrite the other when the sync button is clicked.

In the situation where DRG was played on one save file then the other without syncing, then the save files will become divergent. To fix this, you'll simply need to select the file that you'd like to keep then hit the sync button.

### Notes
You must have an existing save in both the Steam and Xbox versions of DRG. This will not work for transferring saves before starting the game up for the first time on a new platform.
