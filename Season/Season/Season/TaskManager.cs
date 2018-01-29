//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　更新用Component親クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Season.Components;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season
{
    public static class TaskManager
    {
        private static List<Component> NormalContainer = new List<Component>();
        private static List<UpdateComponent> UpdateContainer = new List<UpdateComponent>();
        private static List<DrawComponent> DrawContainer = new List<DrawComponent>();
        private static List<ColliderComponent> ColliderContainer = new List<ColliderComponent>();

        private static bool isPause = false;

        public static void ChangePause() {
            isPause = !isPause;
        }


        #region GetComponent
        public static List<Component> GetNormalComponent(Entity entity, string type) {
            List<Component> result = NormalContainer.FindAll(c => c.GetEntity() == entity);
            result = result.FindAll(c => c.GetType().Name == type);
            return result;
        }

        public static List<UpdateComponent> GetUpdateComponent(Entity entity, string type) {
            List<UpdateComponent> result = UpdateContainer.FindAll(c => c.GetEntity() == entity);
            result = result.FindAll(c => c.GetType().Name == type);
            return result;
        }

        public static List<DrawComponent> GetDrawComponent(Entity entity, string type) {
            List<DrawComponent> result = DrawContainer.FindAll(c => c.GetEntity() == entity);
            result = result.FindAll(c => c.GetType().Name == type);
            return result;
        }

        public static List<ColliderComponent> GetColliderComponent(Entity entity, string colliderName) {
            List<ColliderComponent> result = ColliderContainer.FindAll(c => c.GetEntity() == entity);
            result = result.FindAll(c => c.colliderName == colliderName);
            return result;
        }

        #endregion

        #region NormalContainer
        public static void AddTask(Component comp) { NormalContainer.Add(comp); }
        #endregion

        #region UpdateContainer
        public static void AddTask(UpdateComponent comp) { UpdateContainer.Add(comp); }
        public static void RemoveTask(UpdateComponent comp) {
            comp.DeActive();
            UpdateContainer.Remove(comp);
        }
        #endregion

        #region DrawContainer
        public static void AddTask(DrawComponent comp) {
            DrawContainer.Add(comp);
            SordDepth();
        }

        public static void SordDepth() {
            DrawContainer.Sort((x, y) => x.depth.CompareTo(y.depth));
        }

        public static void RemoveTask(DrawComponent comp) {
            comp.DeActive();
            DrawContainer.Remove(comp);
        }
        #endregion

        #region ColliderContainer
        public static void AddTask(ColliderComponent comp) { ColliderContainer.Add(comp); }
        public static void RemoveTask(ColliderComponent comp) {
            comp.DeActive();
            ColliderContainer.Remove(comp);
        }
        #endregion

        public static void Draw() { DrawContainer.ForEach(c => c.Draw()); }

        public static void Update()
        {
            if (isPause) { return; }
            UpdateContainer.ForEach(c => c.Update());

            UpdateContainer.RemoveAll(c => !c.IsAtive);
            NormalContainer.RemoveAll(c => !c.IsAtive);
            DrawContainer.RemoveAll(c => !c.IsAtive);
            EntityManager.Destroy();

            object syncobject = new object();
            lock (syncobject) { Collition(); }
        }

        public static void CloseAllTask()
        {
            NormalContainer.ForEach(c => c.DeActive());
            UpdateContainer.ForEach(c => c.DeActive());
            DrawContainer.ForEach(c => c.DeActive());
            ColliderContainer.ForEach(c => c.DeActive());
        }

        private static void Collition() {
            ColliderContainer.ForEach(c => c.Update());

            for (int i = 0; i < ColliderContainer.Count; i++) {
                for (int j = 0; j < ColliderContainer.Count; j++) {
                    ColliderContainer[i].Collition(ColliderContainer[j]);
                }
            }

            ColliderContainer.RemoveAll(c => !c.IsAtive);
        }

    }
}
