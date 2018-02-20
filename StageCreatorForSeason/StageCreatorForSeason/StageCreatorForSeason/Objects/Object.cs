using Microsoft.Xna.Framework;
using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Objects
{
    enum eObjectType {
        BezierStage = 0,
        Wall,
        Boar,
        Eagle,
        Shrub,
        Squirrel,
        Wasteland,
        F = 10,
        X,
        Z,
        Jump,
        Right,
        Left,
        HintNoLeft,
        HintNoRight,
        HintJump,
        HintDown,
        None,
    }

    class Object
    {
        protected Vector2 position;
        protected float radius;
        protected string name;
        protected bool isMouseIn;
        protected eObjectType type;
        protected int teamNo;
        protected bool isSelected;

        public string GetName() { return name; }

        public Vector2 Position {
            set { position = value; }
            get { return position; }
        }

        public float PostionX {
            set { position = new Vector2(value, position.Y); }
            get { return position.X; }
        }
        public float PostionY {
            set { position = new Vector2(value, position.Y); }
            get { return position.Y; }
        }

        public int GetTeamNo() { return teamNo; }

        public Object(string name, Vector2 position, float radius, eObjectType type, int teamNo = 0) {
            this.position = position;
            this.radius = radius;
            this.name = name;
            this.type = type;
            this.teamNo = teamNo;
            isMouseIn = false;
        }

        public void CheckSelected(int index) {
            isSelected = teamNo == index;
        }

        private Object(Object other, Vector2 position)
                : this(other.name, position, other.radius, other.type)
        { }

        public virtual Object Clone(Vector2 position) {
            return new Object(this, position);
        }

        public eObjectType GetType() {
            return type;
        }

        public void dragging(Vector2 position) { this.position = position; }
        public bool IsMouseIn {
            get { return isMouseIn; }
            set { isMouseIn = value; }
        }

        public void Update(Vector2 mousePosition) {
            isMouseIn = (position - mousePosition).LengthSquared() <= radius * radius;
        }

        public virtual void Draw() {
            DrawCollition();
            DrawObject();
        }

        protected virtual void DrawObject() {
            Renderer_2D.Begin(Camera2D.GetTransform());

            Vector2 imgSize = ResouceManager.GetTextureSize(name);
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Renderer_2D.DrawTexture(name, position, Color.White, 1, rect, Vector2.One, 0, imgSize / 2);

            Renderer_2D.End();
        }
        protected virtual void DrawCollition() {
            Renderer_2D.Begin(Camera2D.GetTransform());

            Color color = isMouseIn ? Color.Red : Color.Yellow;

            Vector2 imgSize = ResouceManager.GetTextureSize("CollisionArea");
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Vector2 drawSize = new Vector2(radius / imgSize.X, radius / imgSize.Y) * 2;
            Renderer_2D.DrawTexture("CollisionArea", position, color, 0.5f, rect, drawSize, 0, imgSize / 2);

            Renderer_2D.End();
        }

        public virtual string Print() {
            return "";
        }
    }
}
