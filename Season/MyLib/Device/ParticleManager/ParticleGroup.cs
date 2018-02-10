//作成日：　2017.10.11
//作成者：　柏
//クラス内容：  パーティクル管理
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Utility;
using MyLib.Utility.Action;
using System;
using System.Collections.Generic;

namespace MyLib.Device.ParticleManager
{
    public class ParticleGroup
    {
        private GraphicsDevice graphicsDevice;
        private GameDevice gameDevice;
        private static Random rand = new Random();

        private List<Particle> particles;

        public ParticleGroup(GraphicsDevice graphicsDevice, GameDevice gameDevice) {
            this.graphicsDevice = graphicsDevice;
            this.gameDevice = gameDevice;
            
            particles = new List<Particle>();
        }

        public void AddParticles(string name,
            int countMin, int countMax,
            Vector2 positionLT, Vector2 positionRB,
            float speedMin, float speedMax,
            float sizeMin, float sizeMax,
            float alphaMin, float alphaMax,
            int angleMin, int angleMax,
            float aliveMin, float aliveMax,
            IMoveAble move,
            IChangeAble change
            )
        {
            //gameDevice.GetSound.PlaySE(name);

            //パーティクルの色を決める（明るい色にする）
            float[] color = new float[4] {
                    (float)rand.NextDouble() + 0.5f,
                    (float)rand.NextDouble() + 0.5f,
                    (float)rand.NextDouble() + 0.5f,
                    1.0f };
            int count = rand.Next(countMin, countMax);
            for (int i = 0; i < count; i++) {
                Vector2 position = new Vector2(
                    (float)rand.Next((int)positionLT.X, (int)positionRB.X),
                    (float)rand.Next((int)positionLT.Y, (int)positionRB.Y));
                float speed = (float)rand.Next((int)(speedMin * 10), (int)(speedMax * 10)) / 10;
                Vector2 size = Vector2.One * (float)rand.Next((int)(sizeMin * 10), (int)(sizeMax * 10)) / 10;
                float alpha = (float)rand.Next((int)(alphaMin * 10), (int)(alphaMax * 10)) / 10;
                float aliveTime = (float)rand.Next((int)(aliveMin * 10), (int)(aliveMax * 10)) / 10;

                float angle = rand.Next(angleMin, angleMax);
                float radian = MathHelper.ToRadians(angle);
                Vector2 velocity = new Vector2(1, 0);
                velocity = Method.RotateVector2(velocity, (int)angle);

                particles.Add(new Particle(graphicsDevice, name, position, velocity, speed, size, alpha, radian, aliveTime, move, change, color));
            }
        }

        public void Clear() {
            particles.Clear();
        }

        public void Update() {
            particles.ForEach(p => p.Update());
            particles.RemoveAll(p => p.IsDead());
        }

        public void Draw() {
            particles.ForEach(p => p.Draw());
        }
        
        public int GetParticlesCount() {
            return particles.Count;
        }
    }
}
