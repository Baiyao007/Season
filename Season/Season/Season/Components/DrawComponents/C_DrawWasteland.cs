using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;
using Season.Components.NormalComponents;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawWasteland : DrawComponent
    {
        private C_WastelandState state;
        private Timer liveTimer;
        private GameDevice gameDevice;
        private string name;

        public C_DrawWasteland(GameDevice gameDevice, C_WastelandState state, float depth = 12, float alpha = 1f)
        {
            this.gameDevice = gameDevice;
            this.alpha = alpha;
            this.depth = depth;
            this.state = state;
            liveTimer = new Timer(10);
            name = "P_Ice_";
        }

        public void SetLiveTime(float second) { liveTimer = new Timer(second); }

        public override void Draw()
        {
            if (!state.IsPurify) { CreatBubble(); }
            else {
                liveTimer.Update();
                if (liveTimer.IsTime) {
                    for (int i = 0; i < state.moveAblePoints.Count; i++) {
                        BezierStage.UpdateBezierPoint(state.Pollute(), state.moveAblePoints[i]);
                    }
                    liveTimer.Initialize();
                }
            
                Renderer_2D.Begin(Camera2D.GetTransform());
                Vector2 imgSize = ResouceManager.GetTextureSize("P_Ice_0");
                Vector2 position = state.startPosition;
                Vector2 direction = state.endPosition - state.startPosition;
                direction.Normalize();
                int index = 0;
                while (position.X < state.endPosition.X) {
                    string imgName = name + index % 5;
                    Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
                    Renderer_2D.DrawTexture(
                        imgName,
                        position - new Vector2(0, 20),
                        Color.White,
                        alpha,
                        rect,
                        Vector2.One,
                        0,
                        imgSize / 2
                    );
                    index++;
                    position += direction * imgSize.X;
                }

                Renderer_2D.End();

            }
        }

        public void CreatBubble() {
            gameDevice.GetParticleGroup.AddParticles(
                "P_Ring",         //name
                1, 1,             //count
                state.startPosition - new Vector2(0, 50),
                state.endPosition,      //position
                1.0f, 3.0f,             //speed
                1f, 2.5f,               //size
                1f, 1,                  //alpha
                90, 90,                 //angle
                1, 1,                   //alive
                new MoveLine(),         //moveType
                new ChangeToLucency(new Timer(1))   //changeType
            );
        }

    }
}
