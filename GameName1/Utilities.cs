﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameName1
{
    class Utilities
    {
        public static float DegreesToRadians(float degrees)
        {
            return degrees *  (float)Math.PI / 180.0f ;
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * (180.0f) / (float)Math.PI;
        }

        public static bool PointIntersectsRectangle(Vector2 point, Rectangle rec)
        {
            return (point.X >= rec.Left && point.Y >= rec.Top && point.X <= rec.Right && point.Y <= rec.Bottom);
        }

        //The amount of scaling that needs to be applied to a sprite with width and height of baseVec
        //so that it matches the width and height for result
        public static Vector2 GetSpriteScaling(Vector2 result, Vector2 baseVec)
        {
            Vector2 temp = new Vector2();
            temp.X = result.X / baseVec.X;
            temp.Y = result.Y / baseVec.Y;
            return temp;
        }
    }

    //0_o
    public class WidgetTree
    {
        private List<WidgetTree> Children;
        private List<Rectangle> HitableObjects;
        private Dictionary<Rectangle, Texture2D> DrawableAreas;
        private Rectangle BaseContainer;
        public WidgetTree(Rectangle baseArea)
        {
            Children = null;
            HitableObjects = new List<Rectangle>();
            DrawableAreas = new Dictionary<Rectangle, Texture2D>();
            BaseContainer = baseArea;
        }
        public void AddHitArea(Rectangle rec)
        {
            HitableObjects.Add(rec);
        }
        public void AddDrawArea(Rectangle area, Texture2D tex)
        {
            DrawableAreas.Add(area, tex);
        }
        public void UpdatePositions(Vector2 delta)
        {
            for (int x = 0; x < HitableObjects.Count; ++x)
            {
                Rectangle rec = HitableObjects[x];
                rec.X += (int)delta.X;
                rec.Y += (int)delta.Y;
                HitableObjects[x] = rec;
            }
            if (Children != null)
            {
                foreach (WidgetTree child in Children)
                {
                    child.UpdatePositions(delta);
                }
            }
        }
        public void AddWidgetTree(WidgetTree widgetTree)
        {
            Children.Add(widgetTree);
        }
        //returns an empty rectangle on false
        public Rectangle CheckCollision(Point p)
        {
            if (Children != null)
            {
                foreach (WidgetTree tree in Children)
                {
                    Rectangle temp;
                    temp = tree.CheckCollision(p);
                    if (temp.Width > 0)
                    {
                        return temp;
                    }
                }
            }
            foreach (Rectangle rec in HitableObjects)
            {
                if (rec.Contains(p))
                {
                    return rec;
                }
            }
            return new Rectangle();
        }
        //this color parameter needs to be removed in the future
        public void StartDrawWidgets(SpriteBatch _spriteBatch, Rectangle where, Color c)
        {
            Queue<WidgetTree> queue = new Queue<WidgetTree>();
            queue.Enqueue(this);
            while (queue.Count > 0) {
                WidgetTree child = queue.Dequeue();
                child.DrawWidgets(_spriteBatch, where, c);
                if (child.Children != null) 
                {
                    foreach (WidgetTree widgetTree in child.Children)
                    {
                        queue.Enqueue(widgetTree);
                    }
                }
            }
        }
        //TODO: Remove this color parameter
        private void DrawWidgets(SpriteBatch _spriteBatch, Rectangle where, Color c)
        {
            Rectangle temp = new Rectangle();
            foreach (KeyValuePair<Rectangle, Texture2D> entry in DrawableAreas)
            {
                temp = entry.Key;
                temp.X += (BaseContainer.X + where.X);
                temp.Y += (BaseContainer.Y + where.Y);
                _spriteBatch.Draw(entry.Value, temp, null, c, Utilities.DegreesToRadians(90f), new Vector2((temp.Width / 2), (temp.Height / 2)), SpriteEffects.None, 0);
                //_spriteBatch.Draw(entry.Value, temp, c);
            }
        }
    }
}
