#region File Description
//-----------------------------------------------------------------------------
// SpriteEntity.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Audio3D
{
    /// <summary>
    /// Base class for game entities that are displayed as billboard sprites,
    /// and which can emit 3D sounds. The Cat and Dog classes both derive from this.
    /// </summary>
    abstract class SpriteEntity : IAudioEmitter
    {
        #region Properties


        /// <summary>
        /// Gets or sets the 3D position of the entity.
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Vector3 _position;


        /// <summary>
        /// Gets or sets which way the entity is facing.
        /// </summary>
        public Vector3 Forward
        {
            get { return _forward; }
            set { _forward = value; }
        }

        private Vector3 _forward;


        /// <summary>
        /// Gets or sets the orientation of this entity.
        /// </summary>
        public Vector3 Up
        {
            get { return _up; }
            set { _up = value; }
        }

        private  Vector3 _up;

        
        /// <summary>
        /// Gets or sets how fast this entity is moving.
        /// </summary>
        public Vector3 Velocity
        {
            get { return _velocity; }
            protected set { _velocity = value; }
        }

        private Vector3 _velocity;


        /// <summary>
        /// Gets or sets the texture used to display this entity.
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private Texture2D _texture;


        #endregion


        /// <summary>
        /// Updates the position of the entity, and allows it to play sounds.
        /// </summary>
        public abstract void Update(GameTime gameTime, AudioManager audioManager);


        /// <summary>
        /// Draws the entity as a billboard sprite.
        /// </summary>
        public void Draw(QuadDrawer quadDrawer, Vector3 cameraPosition,
                         Matrix view, Matrix projection)
        {
            Matrix world = Matrix.CreateTranslation(0, 1, 0) *
                           Matrix.CreateScale(800) *
                           Matrix.CreateConstrainedBillboard(Position, cameraPosition,
                                                             Up, null, null);

            quadDrawer.DrawQuad(Texture, 1, world, view, projection);
        }
    }
}
