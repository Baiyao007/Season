using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Utility
{
    class ImageEffect
    {
        public string name { get; }
        public float angle { get; set; }
        public float alpha { get; private set; }
        public Vector2 position { get; set; }
        public Vector2 imageSize { get; }
        public Rectangle rect { get; }

        public Vector2 size;

        //private Timer transTimer;
        private bool isBreak;
        public bool isDisappear { get; private set; }

        private Timer aliveTimer;


        public Effect effect;
        public VertexBuffer vertexBuffer;
        public VertexPositionTexture[] vertexPositions;
        private GraphicsDevice graphicsDevice;
        

        public ImageEffect(string name, Vector2 position, float aliveSecond) {
            this.name = name;
            this.position = position;
            alpha = 0;
            //transTimer = new Timer(1f);
            aliveTimer = new Timer(aliveSecond);
            isBreak = false;
            isDisappear = false;
            imageSize = ResouceManager.GetTextureSize(name);
            rect = new Rectangle(0, 0, (int)imageSize.X, (int)imageSize.Y);
            size = Vector2.One;

            graphicsDevice = Renderer_2D.GetGraphicsDevice();

            effect = ResouceManager.GetEffect("MaskShader").Clone();
            effect.Parameters["theTexture"].SetValue(ResouceManager.GetTexture(name));
            effect.Parameters["theMask"].SetValue(ResouceManager.GetTexture("ShadeMask_LR"));
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["Color"].SetValue(new float[4] { 0.5f, 0, 0, 0.5f });
            effect.Parameters["WorldViewProjection"].SetValue(Camera2D.GetView() * Camera2D.GetProjection());

            vertexPositions = new VertexPositionTexture[4];
        }



        public void VertexUpdate(Vector3 drawPosition)
        {
            float rotateAngle = 0;

            vertexPositions[0] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f * size.X, -0.5f * size.Y, 0) * Camera2D.GetZoom(), rotateAngle) * imageSize.X, new Vector2(0, 0));
            vertexPositions[1] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f * size.X, 0.5f * size.Y, 0) * Camera2D.GetZoom(), rotateAngle) * imageSize.X, new Vector2(0, 1));
            vertexPositions[2] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(0.5f * size.X, -0.5f * size.Y, 0) * Camera2D.GetZoom(), rotateAngle) * imageSize.X, new Vector2(1, 0));
            vertexPositions[3] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(0.5f * size.X, 0.5f * size.Y, 0) * Camera2D.GetZoom(), rotateAngle) * imageSize.X, new Vector2(1, 1));

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), vertexPositions.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(vertexPositions);
        }



        public void Update() {
            //transTimer.Update();
            aliveTimer.Update();

            //alpha = isBreak ? transTimer.Rate() : 1 - transTimer.Rate();
            effect.Parameters["Rate"].SetValue(1 - aliveTimer.InterpoRate());
            isDisappear = aliveTimer.IsTime;

            //if (isBreak && transTimer.IsTime) { isDisappear = true; }
            //if (isBreak) { return; }
            //if (aliveTimer.IsTime) { GoToDestroy(); }
        }

        private void GoToDestroy() {
            //transTimer.Initialize();
            isBreak = true;
        }

    }
}
