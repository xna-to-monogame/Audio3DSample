#region File Description
//-----------------------------------------------------------------------------
// AudioManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Audio3D
{
    /// <summary>
    /// Audio manager keeps track of what 3D sounds are playing, updating
    /// their settings as the camera and entities move around the world, and
    /// automatically disposing sound effect instances after they finish playing.
    /// </summary>
    public class AudioManager : GameComponent
    {
        #region Fields


        // List of all the sound effects that will be loaded into this manager.
        private readonly static string[] SoundNames =
        {
            "CatSound0",
            "CatSound1",
            "CatSound2",
            "DogSound",
        };


        // The listener describes the ear which is hearing 3D sounds.
        // This is usually set to match the camera.
        public AudioListener Listener
        {
            get { return _listener; }
        }

        private readonly AudioListener _listener = new AudioListener();


        // The emitter describes an entity which is making a 3D sound.
        private readonly AudioEmitter _emitter = new AudioEmitter();


        // Store all the sound effects that are available to be played.
        private readonly Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();

        
        // Keep track of all the 3D sounds that are currently playing.
        private readonly List<ActiveSound> _activeSounds = new List<ActiveSound>();


        #endregion


        public AudioManager(Game game)
            : base(game)
        { }


        /// <summary>
        /// Initializes the audio manager.
        /// </summary>
        public override void Initialize()
        {
            // Set the scale for 3D audio so it matches the scale of our game world.
            // DistanceScale controls how much sounds change volume as you move further away.
            // DopplerScale controls how much sounds change pitch as you move past them.
            SoundEffect.DistanceScale = 2000;
            SoundEffect.DopplerScale = 0.1f;

            // Load all the sound effects.
            foreach (string soundName in SoundNames)
            {
                _soundEffects.Add(soundName, Game.Content.Load<SoundEffect>(soundName));
            }

            base.Initialize();
        }


        /// <summary>
        /// Unloads the sound effect data.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    foreach (SoundEffect soundEffect in _soundEffects.Values)
                    {
                        soundEffect.Dispose();
                    }

                    _soundEffects.Clear();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        
        /// <summary>
        /// Updates the state of the 3D audio system.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Loop over all the currently playing 3D sounds.
            int index = 0;

            while (index < _activeSounds.Count)
            {
                ActiveSound activeSound = _activeSounds[index];

                if (activeSound.Instance.State == SoundState.Stopped)
                {
                    // If the sound has stopped playing, dispose it.
                    activeSound.Instance.Dispose();

                    // Remove it from the active list.
                    _activeSounds.RemoveAt(index);
                }
                else
                {
                    // If the sound is still playing, update its 3D settings.
                    Apply3D(activeSound);

                    index++;
                }
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// Triggers a new 3D sound.
        /// </summary>
        public SoundEffectInstance Play3DSound(string soundName, bool isLooped, IAudioEmitter emitter)
        {
            ActiveSound activeSound = new ActiveSound();

            // Fill in the instance and emitter fields.
            activeSound.Instance = _soundEffects[soundName].CreateInstance();
            activeSound.Instance.IsLooped = isLooped;

            activeSound.Emitter = emitter;

            // Set the 3D position of this sound, and then play it.
            Apply3D(activeSound);

            activeSound.Instance.Play();

            // Remember that this sound is now active.
            _activeSounds.Add(activeSound);

            return activeSound.Instance;
        }


        /// <summary>
        /// Updates the position and velocity settings of a 3D sound.
        /// </summary>
        private void Apply3D(ActiveSound activeSound)
        {
            _emitter.Position = activeSound.Emitter.Position;
            _emitter.Forward = activeSound.Emitter.Forward;
            _emitter.Up = activeSound.Emitter.Up;
            _emitter.Velocity = activeSound.Emitter.Velocity;

            activeSound.Instance.Apply3D(_listener, _emitter);
        }


        /// <summary>
        /// Internal helper class for keeping track of an active 3D sound,
        /// and remembering which emitter object it is attached to.
        /// </summary>
        private class ActiveSound
        {
            public SoundEffectInstance Instance;
            public IAudioEmitter Emitter;
        }
    }
}
