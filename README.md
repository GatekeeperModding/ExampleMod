# ExampleMod
Examplemod for Modding the Unity Game [Gatekeeper](https://store.steampowered.com/app/2106670/Gatekeeper/)
using the [GKAPI](https://github.com/Robocraft999/GKAPI) (Gatekeeper Modding API)

## Please Backup your saves before use!
you can probably find them under `C:\Users\<your user profile>\AppData\LocalLow\Gravity Lagoon\Gatekeeper\Saves`

## Using the Mod
To use the Mod or any Mods built with the API you have to do the following:

### **With** r2modman (or other thunderstore clients):
- Install the mods within the launcher and launch the modded game

### **Without** r2modman (or other thunderstore clients):
The following instructions use the term ``game root`` which is the folder where the executable is located.<br>
On Windows you probably find it under `C:\Program Files (x86)\Steam\steamapps\common\Gatekeeper`

1. Back up your game files in your ``game root``
2. Back up your save files, found in `C:\Users\<your user profile>\AppData\LocalLow\Gravity Lagoon\Gatekeeper\Saves`
3. Install BepInEx (the modloader) from [here](https://builds.bepinex.dev/projects/bepinex_be/725/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.725%2Be1974e2.zip) 
(This is the tested version)
4. Extract the contents into the ``game root``
5. Run the game executable to generate configuration files for the modloader
6. Download ``GKAPI`` and ``ExampleMod`` from [thunderstore](https://thunderstore.io/c/gatekeeper/)
7. Put them under your ``game root``/BepInEx/plugins

(Instructions inspired by the [BepInEx docs](https://docs.bepinex.dev/master/articles/user_guide/installation/unity_il2cpp.html?tabs=tabid-win))

## Current Content
The following categories are structured by the layout of the API

### Achievements

- simple achievement that handles all the [items](#Items)

### Items

- basic Item with Id ``TEST`` which is not unlocked by default
- more complex Item called ``Clover`` which boosts some Proc chances (inspired by RoR2)
- minimal Item called ``Min`` which is useless
- triad Item called ``TestTriad`` which uses the TestItem, Rune of Rebound and Triumph 
and has a custom Controller which does nothing

### Difficulties

- way harder difficulty than 200% called ``Sentinel`` 
which aims to be around 500% (uses custom [enemy templates](#templates))

### Enemies

#### Templates

- enemy spawn templates for the custom [difficulty](#difficulties) for each planet for loop 1, loop 2 and all after

## Credits

Thanks to [we1u](https://github.com/wewejay) for the Clover assets

