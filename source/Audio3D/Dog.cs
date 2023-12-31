#region File Description
//-----------------------------------------------------------------------------
// Dog.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Audio3D
{
    /// Entity class which sits in one place and plays dog sounds.
    /// This uses a looping sound, which must be explicitly stopped
    /// to prevent it going on forever. See the Cat class for an
    /// example of using a single-shot sound.
    class Dog : SpriteEntity
    {
        #region Fields

        // How long until we should start or stop the sound.
        private TimeSpan _timeDelay = TimeSpan.Zero;

        // The sound which is currently playing, if any.
        private SoundEffectInstance _activeSound = null;

        #endregion


        /// <summary>
        /// Updates the position of the dog, and plays sounds.
        /// </summary>
        public override void Update(GameTime gameTime, AudioManager audioManager)
        {
            // Set the entity to a fixed position.
            Position = new Vector3(0, 0, -4000);
            Forward = Vector3.Forward;
            Up = Vector3.Up;
            Velocity = Vector3.Zero;

            // If the time delay has run out, start or stop the looping sound.
            // This would normally go on forever, but we stop it after a six
            // second delay, then start it up again after four more seconds.
            _timeDelay -= gameTime.ElapsedGameTime;

            if (_timeDelay < TimeSpan.Zero)
            {
                if (_activeSound == null)
                {
                    // If no sound is currently playing, trigger one.
                    _activeSound = audioManager.Play3DSound("DogSound", true, this);

                    _timeDelay += TimeSpan.FromSeconds(6);
                }
                else
                {
                    // Otherwise stop the current sound.
                    _activeSound.Stop(false);
                    _activeSound = null;

                    _timeDelay += TimeSpan.FromSeconds(4);
                }
            }
        }
    }
}
