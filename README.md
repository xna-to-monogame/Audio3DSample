
# Audio3D Sample
This sample shows how to position sounds in 3D space, implementing panning, Doppler, and distance attenuation effects.


## Sample Overview

3D audio is a bit more complicated than playing regular sound effects. It is not good enough to just "fire and forget" a 3D sound. As the listener moves around the world, or if the entity that created a sound moves, you must update the 3D audio settings to reflect these changes.

This sample demonstrates how to overcome this problem, implementing an **AudioManager** component that keeps track of sound effect instances and automatically updates their 3D settings.

For XNA Game Studio 4.0, this sample has been updated to use the SoundEffect API. Previous versions were implemented using XACT instead of SoundEffect.

## Sample Controls

This sample uses the following keyboard and gamepad controls.
| Action          | Keyboard control                                  | Gamepad control  |
| --------------- | ------------------------------------------------- | ---------------- |
| Move the camera | UP ARROW, DOWN ARROW, LEFT ARROW, and RIGHT ARROW | Left thumb stick |
| Exit            | ESC or ALT+F4                                     | BACK             |


## How the Sample Works

The **AudioManager** maintains a list of all the actively playing 3D sounds, along with an **IAudioEmitter** interface describing the entity that is creating each sound. When the manager is updated, it loops over all the active sounds, looks up the latest position of their emitter entities, and updates their 3D settings to reflect the new emitter and listener positions. It also checks for any sounds that have finished playing, calling **Dispose** to clean up their resources and removing them from the active list.

This system can be used for both looping sounds (as demonstrated by the Dog entity) and for one-shot sounds (as shown by the Cat entity).

As you move around the world, you will notice that sounds become quieter as you get farther away from them, and that the pitch changes to create a Doppler effect if you zoom quickly past them. The intensity of these effects can be adjusted by changing the values of the **SoundEffect.DistanceScale** and **SoundEffect.DopplerScale** constants, which are set by the **AudioManager.Initialize** method in this sample. If you want to reuse this code in a game that uses a different scale than the sample, you must update these values to match.

You may also notice that the Cat entity randomly switches among three slightly different sound effect variations, which can help to provide a more natural and realistic effect, reducing the machine-gun-like robotic quality of playing the exact same sound over and over again. This variation is implemented by the Cat.Update method, which uses a random number generator to choose which of the three sound effect variations should be played.

© 2010 Microsoft Corporation. All rights reserved.