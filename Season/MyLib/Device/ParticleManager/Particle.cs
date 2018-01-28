//作成日：　2017.10.11
//作成者：　柏
//クラス内容：  パーティクル
//修正内容リスト：
//名前：柏　　　日付：20171013　　　内容：GPU処理にため改造
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Utility;
using MyLib.Utility.Action;
using System;

namespace MyLib.Device.ParticleManager
{
    enum eParticleType {
        Circle,
        Bar,
        Cross,
        Square,
    }

    class Particle
    {
        private string name;
        private Vector2 position;
        private Vector2 velocity;
        private float speed;
        private Vector2 size;
        private float alpha;
        private float radian;
        private bool isDead;
        private Timer aliveTimer;
        private IMoveAble move;
        private IChangeAble change;
        private float[] color;

        GraphicsDevice graphicsDevice;

        private static Random rand = new Random();

        //現段階は既定サイズ
        private Vector2 imgSize = Vector2.One * 8;

        //現段階板ポリゴンの設定はデフォルトにする
        private BroadPolygon broadPolygon;

        public Particle(GraphicsDevice graphicsDevice, string name, Vector2 position, Vector2 velocity, 
            float speed, Vector2 size, float alpha, float radian, float aliveTime, IMoveAble move, IChangeAble change, float[] color)
        {
            this.graphicsDevice = graphicsDevice;
            this.name = name;
            this.position = position;
            this.velocity = velocity;
            this.speed = speed;
            this.size = size;
            this.alpha = alpha;
            this.radian = radian;
            this.move = move.Clone();
            this.change = change.Clone();
            this.color = color;

            aliveTimer = new Timer(aliveTime);
            isDead = false;
            
            InitializeBroadPolygon();
            UpdateBroad();
        }

        private void InitializeBroadPolygon() {
            Texture2D texture = ResouceManager.GetTexture(name);
            Effect effect = ResouceManager.GetEffect("PaticleShader");
            effect.Parameters["Color"].SetValue(color);
            broadPolygon = new BroadPolygon(graphicsDevice, effect, texture, alpha, size, aliveTimer, change);
            
            //色変更あり
            broadPolygon.SetTechnique(2);
        }

        public void Update() {
            move.Move(ref velocity, ref speed);
            position += velocity * speed;
            radian = (float)Math.Atan2(-velocity.Y, velocity.X);

            aliveTimer.Update();
            isDead = aliveTimer.IsTime;

            UpdateBroad();
        }

        private void UpdateBroad() {
            Vector3 position3 = new Vector3(position, 0);
            broadPolygon.Update(position3, radian);
        }

        public void Draw() { broadPolygon.Draw(); }
        public bool IsDead() { return isDead; }

    }
}
