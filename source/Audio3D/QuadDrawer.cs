#region File Description
//-----------------------------------------------------------------------------
// QuadDrawer.cs
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
    /// Helper for drawing 3D quadrilaterals. This is used to draw the cat
    /// and dog billboard sprites, and also the checkered ground polygon.
    /// </summary>
    class QuadDrawer
    {
        #region Fields

        private readonly GraphicsDevice _graphicsDevice;
        private readonly AlphaTestEffect _effect;
        private readonly VertexPositionTexture[] _vertices;

        #endregion


        /// <summary>
        /// Constructs a new quadrilateral drawing worker.
        /// </summary>
        public QuadDrawer(GraphicsDevice device)
        {
            _graphicsDevice = device;

            _effect = new AlphaTestEffect(device);

            _effect.AlphaFunction = CompareFunction.Greater;
            _effect.ReferenceAlpha = 128;

            // Preallocate an array of four vertices.
            _vertices = new VertexPositionTexture[4];

            _vertices[0].Position = new Vector3(1, 1, 0);
            _vertices[1].Position = new Vector3(-1, 1, 0);
            _vertices[2].Position = new Vector3(1, -1, 0);
            _vertices[3].Position = new Vector3(-1, -1, 0);
        }


        /// <summary>
        /// Draws a quadrilateral as part of the 3D world.
        /// </summary>
        public void DrawQuad(Texture2D texture, float textureRepeats,
                             Matrix world, Matrix view, Matrix projection)
        {
            // Set our effect to use the specified texture and camera matrices.
            _effect.Texture = texture;

            _effect.World = world;
            _effect.View = view;
            _effect.Projection = projection;

            // Update our vertex array to use the specified number of texture repeats.
            _vertices[0].TextureCoordinate = new Vector2(0, 0);
            _vertices[1].TextureCoordinate = new Vector2(textureRepeats, 0);
            _vertices[2].TextureCoordinate = new Vector2(0, textureRepeats);
            _vertices[3].TextureCoordinate = new Vector2(textureRepeats, textureRepeats);

            // Draw the quad.
            _effect.CurrentTechnique.Passes[0].Apply();

            _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, 2);
        }
    }
}
