using Microsoft.Xna.Framework;
using Season.Components.ColliderComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.NormalComponents
{
    class C_PlayerState : Component
    {
        public bool IsHidden { get; set; }
        private List<Entity> children;  //蘇った子供をまとめるlist

        public C_PlayerState() {
            IsHidden = false;
            children = new List<Entity>();
        }

        public void AddChild(Entity child) { children.Add(child); }

        //Playerと一番近い子供を取る
        public Entity GetOneChild() {
            if (children.Count == 0) {
                return Entity.CreateEntity("Null", "Null", new Transform2D());
            }
            Entity child = children[0];
            children.ForEach(c => {
                    if ( Vector2.DistanceSquared(    c.transform.Position, entity.transform.Position) <
                         Vector2.DistanceSquared(child.transform.Position, entity.transform.Position) )
                    {
                        child = c;
                    }
                }
            );
            return child;
        }

        public void SetHidden(bool isHidden) {
            IsHidden = isHidden;
            if (isHidden) {
                entity.GetColliderComponent("Player").DeActive();
                ColliderComponent playerHiddenCollider = new C_Collider_Circle("PlayerHidden", new Vector2(0, -120), 100);
                entity.RegisterComponent(playerHiddenCollider);
            } else {
                entity.GetColliderComponent("PlayerHidden").DeActive();
                ColliderComponent playerCollider = new C_Collider_Circle("Player", new Vector2(0, -120), 80, eCollitionType.Jostle);
                entity.RegisterComponent(playerCollider);
            }
        }


        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }
    }
}
