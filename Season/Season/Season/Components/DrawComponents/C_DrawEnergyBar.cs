using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using Season.Components.NormalComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawEnergyBar : DrawComponent
    {
        private C_Energy energyBar;
        private float offsetY;

        public C_DrawEnergyBar(C_Energy energyBar, float offsetY, float depth = 100, float alpha = 0.5f)
        {
            this.alpha = alpha;
            this.depth = depth;
            this.energyBar = energyBar;
            this.offsetY = offsetY;
        }
        public override void Draw()
        {
            Renderer_2D.Begin(Camera2D.GetTransform());

            string name = energyBar.GetImgName();
            Vector2 imgSize = ResouceManager.GetTextureSize(name);
            Rectangle rect = new Rectangle(0, 0, (int)(imgSize.X * energyBar.GetRate()), (int)imgSize.Y);

            Vector2 position = entity.transform.Position;
            while (entity.transform.Angle < 0) { entity.transform.Angle += 360; }
            float angle = entity.transform.Angle;
            if (angle % 180 == 0) { angle--; }
            float radian = MathHelper.ToRadians(angle);
            Vector2 direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));
            Vector2 offsetVert = Method.RightAngleMove(direction, 270);

            angle += 90;
            int area = (int)(angle / 90);
            radian = MathHelper.ToRadians(angle);
            direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));

            radian = MathHelper.ToRadians(entity.transform.Angle);

            Vector2 offsetHori = Vector2.Zero;
            if (area % 4 == 0 || area % 4 == 2)
            {
                offsetHori = Method.RightAngleMove(direction, 40);
            }
            else {
                offsetHori = Method.RightAngleMove(direction, -40);
            }
            Renderer_2D.DrawTexture(
                name, 
                position + offsetVert + offsetHori,
                Color.LightGreen,
                alpha,
                rect, 
                Vector2.One,
                radian, 
                imgSize / 2
            );

            Renderer_2D.End();
        }
    }
}
