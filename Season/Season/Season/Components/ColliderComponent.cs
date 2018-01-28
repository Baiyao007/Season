using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components.ColliderComponents;
using Season.Def;
using Season.Entitys;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components
{
    public enum eCollitionType {
        Through,
        Jostle
    }

    public enum eCollitionForm {
        Circle,
        Square,
        Line,
    }

    public class ColliderComponent : Component
    {
        public int radius;
        public eCollitionType collisionType;
        public eCollitionForm collisionForm;
        public Vector2 offsetPosition;
        public bool isLocal;
        public string colliderName;
        public Vector2 centerPosition;

        public List<ColliderResult> results;

        protected Timer distroyTimer;
        protected Timer awakeTimer;
        protected bool isSleep;
        

        public ColliderComponent(
            string colliderName,
            Vector2 offsetPosition,
            int radius = 0,
            eCollitionType collisionType = eCollitionType.Through,
            eCollitionForm collisionForm = eCollitionForm.Circle,
            bool isLocal = true
            )
        {
            this.colliderName = colliderName;
            this.offsetPosition = offsetPosition;
            this.radius = radius;
            this.collisionType = collisionType;
            this.collisionForm = collisionForm;
            this.isLocal = isLocal;
            results = new List<ColliderResult>();

            InitializeCollision();
        }

        #region GetCollisionResult
        public bool ThroughStart(string colliderName) {
            ColliderResult target = results.Find(r => colliderName == r.otherCollider.colliderName);
            if (target == null) { return false; }
            return target.ThroughStart();
            }
        public bool IsThrough(string colliderName) {
            ColliderResult target = results.Find(r => colliderName == r.otherCollider.colliderName);
            if (target == null) { return false; }
            return target.IsThrough();
        }
        public bool ThroughEnd(string colliderName) {
            ColliderResult target = results.Find(r => colliderName == r.otherCollider.colliderName);
            if (target == null) { return false; }
            return target.ThroughEnd();
        }


        public bool JostleStart(string colliderName) {
            ColliderResult target = results.Find(r => colliderName == r.otherCollider.colliderName);
            if (target == null) { return false; }
            return target.JostleStart();
        }
        public bool IsJostle(string colliderName) {
            ColliderResult target = results.Find(r => colliderName == r.otherCollider.colliderName);
            if (target == null) { return false; }
            return target.IsJostle();
        }
        public bool JostleEnd(string colliderName) {
            ColliderResult target = results.Find(r => colliderName == r.otherCollider.colliderName);
            if (target == null) { return false; }
            return target.JostleEnd();
        }

        public Entity GetOtherEntity(string colliderName)
        {
            ColliderResult target = results.Find(r => colliderName == r.otherCollider.colliderName);
            if (target == null) { return null; }
            return target.otherCollider.GetEntity();
        }
        public ColliderComponent GetOtherCollider(string colliderName)
        {
            ColliderResult target = results.Find(r => colliderName == r.otherCollider.colliderName);
            if (target == null) { return null; }
            return target.otherCollider;
        }
        #endregion
        public void SetSleep() {
            results.Clear();
            isSleep = true;
            awakeTimer = null;
        }
        public void Awake(float second) {
            results.Clear();
            isSleep = true;
            awakeTimer = new Timer(second);
        }

        public void Destroy(float second) { distroyTimer = new Timer(second); }

        public virtual void Update() {
            AwakeAction();
            DestroyAction();
            results.ForEach(r => r.InitialzeResultPerFrame());

            if (isSleep) { return; }
            if (!IsAtive) { return; }

            if (isLocal) {
                float radian = MathHelper.ToRadians(entity.transform.Angle);
                Vector2 direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));
                centerPosition = entity.transform.Position + Methord.RightAngleMove(direction, offsetPosition.Length());
                centerPosition += IsRight()?  direction * offsetPosition.X : -direction * offsetPosition.X;
            }
        }

        private bool IsRight() {
            int area = (int)(entity.transform.Angle / 90) % 4;
            if (area == 0 || area == 3) { return true; }
            else { return false; }
        }


        public virtual void Collition(ColliderComponent otherComp)
        {
            if (!CheckCanCollision(otherComp)) { return; }
            
            if (collisionType == eCollitionType.Jostle && otherComp.collisionType == eCollitionType.Jostle) {
                DoJostleCollision(otherComp);
            } else {
                DoThroughCollision(otherComp);
            }
            results.RemoveAll(r => !r.IsCollide());
        }

        protected bool CheckCanCollision(ColliderComponent other) {
            //距離遠いところのColliderは当たらない
            if (Vector2.DistanceSquared(centerPosition, other.centerPosition) > Parameter.CollideAbleDistanceSquare) {
                return false;
            }
            if (other == this) { return false; }    //自分と当たらない
            if (other.entity == entity) { return false; }   //同じ親のColliderは当たらない
            if (isSleep || other.isSleep) { return false; }     //機能してないColliderは当たらない
            if (!IsAtive || !other.IsAtive) { return false; }   //無効になったColliderは当たらない

            return true;
        }

        protected virtual void DoJostleCollision(ColliderComponent otherComp) { }
        protected virtual void DoThroughCollision(ColliderComponent otherComp) { }

        protected void AwakeAction() {
            if (awakeTimer == null) { return; }
            awakeTimer.Update();
            if (awakeTimer.IsTime) {
                awakeTimer = null;
                isSleep = false;
            }
        }

        //削除カウントダウン
        protected void DestroyAction() {
            if (distroyTimer == null) { return; }
            distroyTimer.Update();
            if (distroyTimer.IsTime) { DeActive(); }
        }

        protected void InitializeCollision() {
            results.Clear();
            isSleep = false;
            awakeTimer = null;
        }

        #region 当たり判定処理
        protected void SetResultDataJostle(bool isJostle, ColliderComponent otherComp) {
            ColliderResult resultOther = results.Find(r => r.otherCollider == otherComp);
            if (resultOther == null) {
                if (isJostle) {
                    resultOther = new ColliderResult();
                    resultOther.otherCollider = otherComp;
                    resultOther.isJostleCurrent = true;
                    results.Add(resultOther);
                }
            }
            else {
                resultOther.isJostleCurrent = isJostle;
            }

            ColliderResult resultThis = otherComp.results.Find(r => r.otherCollider == this);
            if (resultThis == null) {
                if (isJostle) {
                    resultThis = new ColliderResult();
                    resultThis.otherCollider = this;
                    resultThis.isJostleCurrent = true;
                    otherComp.results.Add(resultThis);
                }
            }
            else {
                resultThis.isJostleCurrent = isJostle;
            }
        }
        protected void SetResultDataThrough(bool isThrough, ColliderComponent otherComp) {
            ColliderResult resultOther = results.Find(r => r.otherCollider == otherComp);
            if (resultOther == null) {
                if (isThrough) {
                    resultOther = new ColliderResult();
                    resultOther.otherCollider = otherComp;
                    resultOther.isThroughCurrent = isThrough;
                    results.Add(resultOther);
                }
            }
            else {
                resultOther.isThroughCurrent = isThrough;
            }

            ColliderResult resultThis = otherComp.results.Find(r => r.otherCollider == this);
            if (resultThis == null) {
                if (isThrough) {
                    resultThis = new ColliderResult();
                    resultThis.otherCollider = this;
                    resultThis.isThroughCurrent = true;
                    otherComp.results.Add(resultThis);
                }
            }
            else {
                resultThis.isThroughCurrent = isThrough;
            }
        }

        protected void Through_Circle_Circle(ColliderComponent otherComp) {
            bool isThroughThis = CollitionCheck.CollitionCheck_Circle(this, otherComp);
            SetResultDataThrough(isThroughThis, otherComp);
        }
        protected void Jostle_Circle_Circle(ColliderComponent otherComp) {
            bool isJostleThis = CollitionCheck.CollitionCheck_Circle(this, otherComp);
            SetResultDataJostle(isJostleThis, otherComp);
        }
        protected void Through_Circle_Line(ColliderComponent otherComp) {
            Vector2 normal = Vector2.Zero;
            bool isThroughThis = false;
            if (otherComp.collisionForm == eCollitionForm.Line) {
                isThroughThis = Methord.CircleSegment(
                                        ref centerPosition,
                                        radius,
                                        ((C_Collider_Line)otherComp).Position1,
                                        ((C_Collider_Line)otherComp).Position2,
                                        ref normal
                                    );
            }
            else if (collisionForm == eCollitionForm.Line) {
                isThroughThis = Methord.CircleSegment(
                                        ref otherComp.centerPosition,
                                        otherComp.radius,
                                        ((C_Collider_Line)this).Position1,
                                        ((C_Collider_Line)this).Position2,
                                        ref normal
                                    );
            }
            SetResultDataThrough(isThroughThis, otherComp);

        }
        protected void Jostle_Circle_Line(ColliderComponent otherComp) {
            Vector2 normal = Vector2.Zero;
            bool isJostleThis = false;
            if (otherComp.collisionForm == eCollitionForm.Line) {
                Vector2 check = centerPosition;
                isJostleThis = Methord.CircleSegment(
                                      ref check,
                                      radius,
                                      ((C_Collider_Line)otherComp).Position1,
                                      ((C_Collider_Line)otherComp).Position2,
                                      ref normal
                                  );
                //BezierStageのため押し出しをしない
                //if (isJostleThis) { entity.transform.Position = centerPosition - offsetPosition; }
            }
            else if (collisionForm == eCollitionForm.Line) {
                Vector2 check = otherComp.centerPosition;
                isJostleThis = Methord.CircleSegment(
                                      ref check,
                                      otherComp.radius,
                                      ((C_Collider_Line)this).Position1,
                                      ((C_Collider_Line)this).Position2,
                                      ref normal
                                  );
                //BezierStageのため押し出しをしない
                //if (isJostleThis) { otherComp.GetEntity().transform.Position = otherComp.centerPosition - offset - otherComp.offsetPosition; }
            }
            SetResultDataJostle(isJostleThis, otherComp);
        }

        #endregion

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            centerPosition = entity.transform.Position + offsetPosition;
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            InitializeCollision();
        }

    }
}
