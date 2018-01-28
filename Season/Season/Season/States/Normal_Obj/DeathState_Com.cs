//作成日：　2017.10.10
//作成者：　柏
//クラス内容：  ステート実装（死亡パータン）
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;
using Season.Components;
using Season.Def;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.States.Normal_Obj
{
    class DeathState_Com : IState<Entity>
    {
        private string name;
        private GameDevice gameDevice;
        private Vector2 effectPosition;
        private static Random rand = new Random();

        private Dictionary<string, Action> effects;

        public DeathState_Com(string name, GameDevice gameDevice)
        {
            this.name = name;
            this.gameDevice = gameDevice;

            effects = new Dictionary<string, Action>() {
                { "Squirrel",  CreatFireCircle },
                { "Enemy",  CreatFireSquare },
                { "E_Pinwheel",  CreatEddy },
                { "E_Square",  CreatFlash },
                { "Boar",  CreatFlash },
                { "Player",  CreatRay },
            };
        }

        protected override void Initialize(Entity entity)
        {
            Sound.PlaySE(name);
            effectPosition = entity.transform.Position;
            effects[entity.GetName()]();
        }


        #region CreatEffect
        private void CreatEddy()
        {
            gameDevice.GetParticleGroup.AddParticles(
                "P_Bar",        //name
                50, 80,         //count
                effectPosition, effectPosition,     //position
                3.0f, 8.0f,           //speed
                0.8f, 1,           //size
                0.5f, 1,           //alpha
                0, 360,         //angle
                1, 1,           //alive
                new MoveCircle(rand.Next(5, 10) / 10f, true),  //moveType
                new ChangeToLucency(new Timer(1))   //changeType
            );
        }

        private void CreatFireCircle()
        {
            gameDevice.GetParticleGroup.AddParticles(
                "P_Circle",        //name
                50, 80,         //count
                effectPosition, effectPosition,     //position
                1, 8,           //speed
                0.8f, 1,           //size
                0.5f, 1,           //alpha
                0, 360,         //angle
                1, 1,           //alive
                new MoveAccelerate(false),  //moveType
                new ChangeToLucency(new Timer(1))   //changeType
            );
        }

        private void CreatFireSquare()
        {
            gameDevice.GetParticleGroup.AddParticles(
                "P_Square",        //name
                50, 80,         //count
                effectPosition, effectPosition,     //position
                1, 8,           //speed
                0.8f, 1,           //size
                0.5f, 1,           //alpha
                0, 360,         //angle
                1, 1,           //alive
                new MoveAccelerate(false),  //moveType
                new ChangeToLucency(new Timer(1))   //changeType
            );
        }

        private void CreatFlash()
        {
            gameDevice.GetParticleGroup.AddParticles(
                "P_Cross",        //name
                50, 80,         //count
                effectPosition, effectPosition,     //position
                2, 6,           //speed
                0.8f, 1,           //size
                0.5f, 1,           //alpha
                0, 360,         //angle
                1, 1,           //alive
                new MoveTeleport(true),  //moveType
                new ChangeToLucency(new Timer(1))   //changeType
            );

        }

        private void CreatRay()
        {
            gameDevice.GetParticleGroup.AddParticles(
                "P_Cross",        //name
                80, 130,         //count
                effectPosition, effectPosition,     //position
                2, 7,           //speed
                1.0f, 1.2f,           //size
                0.5f, 1,           //alpha
                0, 360,         //angle
                1, 1,           //alive
                new MoveAccelerate(true),  //moveType
                new ChangeToLucency(new Timer(1))   //changeType
            );
        }
        #endregion

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            entity.GetDrawComponent("C_DrawAnimetion").alpha -= 0.05f;
            if (entity.GetDrawComponent("C_DrawAnimetion").alpha <= 0)
            {
                if (entity.GetName() == "Player") { GameConst.SetEnding(); }
                entity.DeActive();
            }
            nextState = this;
            return eStateTrans.ToThis;
        }

        protected override void ExitAction(Entity UpdateComp) { }
    }


}
