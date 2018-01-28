//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　Entity親クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Season.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Entitys
{
    enum eEnemyType
    {
        E_Circle,
        E_Gear,
        E_Pinwheel,
        E_Square,
        E_Awl,
        None,
    }

    enum eTag
    {
        Player,
        Enemy,
        Effect,
        None,
    }

    public class Entity
    {
        public Transform2D transform { get; set; }
        private string tag;
        private string name;
        private bool isActive;
        private List<Component> componentList;
        private Entity parent;

        private Entity(string name, string tag,Transform2D transform) {
            this.tag = tag;
            this.name = name;
            this.transform = transform;
            isActive = true;
            componentList = new List<Component>();
        }

        public void SetParent(Entity parent) {
            this.parent = parent;
        }

        public Entity GetParent() { return parent; } 
        public static Entity CreateEntity(string name,string tag,Transform2D trans)
        {
            var entity = new Entity(name, tag, trans);
            EntityManager.Register(entity);
            return entity;
        }

        public void Active() { isActive = true; }
        public void DeActive() {
            isActive = false;
            componentList.ForEach(c => c.DeActive());
            componentList.Clear();
        }
        public bool GetIsActive() { return isActive; }


        #region Get
        public string GetName() { return name; }
        public string GetTag() { return tag; }
        public int GetComponentCount() { return componentList.Count; }
        
        public DrawComponent GetDrawComponent(string type) {
            return TaskManager.GetDrawComponent(this, type)[0];
        }
        public UpdateComponent GetUpdateComponent(string type) {
            return TaskManager.GetUpdateComponent(this, type)[0];
        }

        public Component GetNormalComponent(string type)
        {
            List<Component> list = TaskManager.GetNormalComponent(this, type);
            if (list.Count == 0) {
                return null;
            }
            else {
                return TaskManager.GetNormalComponent(this, type)[0];
            }
        }

        public ColliderComponent GetColliderComponent(string colliderName)
        {
            List<ColliderComponent> list = TaskManager.GetColliderComponent(this, colliderName);
            if (list.Count == 0)
            {
                return null;
            }
            else {
                return TaskManager.GetColliderComponent(this, colliderName)[0];
            }
        }

        #region Register
        public void RegisterComponent(ColliderComponent comp) {
            TaskManager.AddTask(comp);
            componentList.Add(comp);
            comp.Register(this);
            comp.Active();
        }

        public void RegisterComponent(UpdateComponent comp) {
            TaskManager.AddTask(comp);
            componentList.Add(comp);
            comp.Register(this);
            comp.Active();
        }

        public void RegisterComponent(DrawComponent comp) {
            TaskManager.AddTask(comp);
            componentList.Add(comp);
            comp.Register(this);
            comp.Active();
        }

        public void RegisterComponent(Component comp)
        {
            TaskManager.AddTask(comp);
            componentList.Add(comp);
            comp.Register(this);
            comp.Active();
        }
        #endregion


        public void RemoveComponent(UpdateComponent comp) {
            componentList.Remove(comp);
            TaskManager.RemoveTask(comp);
        }

        public void RemoveComponent(DrawComponent comp) {
            componentList.Remove(comp);
            TaskManager.RemoveTask(comp);
        }

        public void ClearVoidComponent() {
            componentList.RemoveAll(c => !c.IsAtive);
        }

        //public Type GetComponent<Type>(Func<Component,bool> isCondition)
        //{
        //     //cmponentList.FindAll(component=>isCondition(component));
        //}
        //{
        #endregion

    }
}
