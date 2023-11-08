#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Audio3D
{
    /// <summary>
    /// Sample showing how to implement 3D audio.
    /// </summary>
    public class Audio3DGame : Game
    {
        #region Fields

        private readonly GraphicsDeviceManager _graphics;

        private readonly AudioManager _audioManager;

        private readonly SpriteEntity _cat;
        private readonly SpriteEntity _dog;

        private Texture2D _checkerTexture;

        private QuadDrawer _quadDrawer;

        private Vector3 _cameraPosition = new Vector3(0, 512, 0);
        private Vector3 _cameraForward = Vector3.Forward;
        private Vector3 _cameraUp = Vector3.Up;
        private Vector3 _cameraVelocity = Vector3.Zero;

        private KeyboardState _currentKeyboardState;
        private GamePadState _currentGamePadState;

        #endregion

        #region Initialization


        public Audio3DGame()
        {
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this);

            _audioManager = new AudioManager(this);

            Components.Add(_audioManager);

            _cat = new Cat();
            _dog = new Dog();
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            _cat.Texture = Content.Load<Texture2D>("CatTexture");
            _dog.Texture = Content.Load<Texture2D>("DogTexture");

            _checkerTexture = Content.Load<Texture2D>("checker");

            _quadDrawer = new QuadDrawer(_graphics.GraphicsDevice);
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the game to run logic.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            UpdateCamera();

            // Tell the AudioManager about the new camera position.
            _audioManager.Listener.Position = _cameraPosition;
            _audioManager.Listener.Forward = _cameraForward;
            _audioManager.Listener.Up = _cameraUp;
            _audioManager.Listener.Velocity = _cameraVelocity;

            // Tell our game entities to move around and play sounds.
            _cat.Update(gameTime, _audioManager);
            _dog.Update(gameTime, _audioManager);

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = _graphics.GraphicsDevice;

            device.Clear(Color.CornflowerBlue);

            device.BlendState = BlendState.AlphaBlend;

            // Compute camera matrices.
            Matrix view = Matrix.CreateLookAt(_cameraPosition,
                                              _cameraPosition + _cameraForward,
                                              _cameraUp);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, device.Viewport.AspectRatio,
                                                                    1, 100000);

            // Draw the checkered ground polygon.
            Matrix groundTransform = Matrix.CreateScale(20000) *
                                     Matrix.CreateRotationX(MathHelper.PiOver2);

            _quadDrawer.DrawQuad(_checkerTexture, 32, groundTransform, view, projection);

            // Draw the game entities.
            _cat.Draw(_quadDrawer, _cameraPosition, view, projection);
            _dog.Draw(_quadDrawer, _cameraPosition, view, projection);

            base.Draw(gameTime);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Handles input for quitting the game.
        /// </summary>
        void HandleInput()
        {
            _currentKeyboardState = Keyboard.GetState();
            _currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // Check for exit.
            if (_currentKeyboardState.IsKeyDown(Keys.Escape) ||
                _currentGamePadState.Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }
        }


        /// <summary>
        /// Handles input for moving the camera.
        /// </summary>
        void UpdateCamera()
        {
            const float turnSpeed = 0.05f;
            const float accelerationSpeed = 4;
            const float frictionAmount = 0.98f;

            // Turn left or right.
            float turn = -_currentGamePadState.ThumbSticks.Left.X * turnSpeed;

            if (_currentKeyboardState.IsKeyDown(Keys.Left))
                turn += turnSpeed;

            if (_currentKeyboardState.IsKeyDown(Keys.Right))
                turn -= turnSpeed;

            _cameraForward = Vector3.TransformNormal(_cameraForward,
                                                    Matrix.CreateRotationY(turn));

            // Accelerate forward or backward.
            float accel = _currentGamePadState.ThumbSticks.Left.Y * accelerationSpeed;

            if (_currentKeyboardState.IsKeyDown(Keys.Up))
                accel += accelerationSpeed;

            if (_currentKeyboardState.IsKeyDown(Keys.Down))
                accel -= accelerationSpeed;

            _cameraVelocity += _cameraForward * accel;

            // Add velocity to the current position.
            _cameraPosition += _cameraVelocity;

            // Apply the friction force.
            _cameraVelocity *= frictionAmount;
        }


        #endregion
    }
}
