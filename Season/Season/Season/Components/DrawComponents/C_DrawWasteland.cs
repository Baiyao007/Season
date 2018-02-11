using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;
using Season.Components.UpdateComponents;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawWasteland : DrawComponent
    {
        private C_UpdateWastelandState state;
        private Timer liveTimer;
        private GameDevice gameDevice;
        private string name;

        public C_DrawWasteland(GameDevice gameDevice, C_UpdateWastelandState state, float depth = 12, float alpha = 1f)
        {
            this.gameDevice = gameDevice;
            this.alpha = alpha;
            this.depth = depth;
            this.state = state;
            liveTimer = new Timer(10);
            name = "P_Ice_";
        }

        public void SetLiveTime(float second) { liveTimer = new Timer(second); }

        public override void Draw() {
            CreatBubble();
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
