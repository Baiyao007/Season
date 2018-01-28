//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　Entity管理クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Entitys
{
    public class EntityManager
    {
        private static List<Entity> entityList = new List<Entity>();


        #region Get
        public static List<Entity> FindWithName(string name) {
            List<Entity> targets = entityList.FindAll(e => e.GetName() == name);
            return targets;
        }
        public static List<Entity> FindWithTag(string tag) {
            List<Entity> targets = entityList.FindAll(e => e.GetTag() == tag);
            return targets;
        }
        public static int GetEntityCount() { return entityList.Count; }
        
        #endregion



        public static void Register(Entity entity) { entityList.Add(entity); }
        public static void Destroy() {
            ClearVoidComponent();
            entityList.RemoveAll(e => !e.GetIsActive());
        }
        public static void Clear() {
            entityList.ForEach(e => e.DeActive());
            entityList.Clear();
        }

        private static void ClearVoidComponent() { entityList.ForEach(e => e.ClearVoidComponent()); }

    }
}
