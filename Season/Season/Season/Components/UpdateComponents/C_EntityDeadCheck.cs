using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Def;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    class C_EntityDeadCheck : UpdateComponent
    {
        private bool isDead;

        public C_EntityDeadCheck()
        {
            isDead = false;
        }

        public override void Update()
        {
            isDead = (!Methord.IsInScale(entity.transform.Position, Vector2.Zero, Parameter.StageSize));
        }

        public bool IsDead() { return isDead; }

    }
}