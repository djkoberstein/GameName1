﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameName1
{
    [DataContract]
    class Plasma : Weapon
    {
        [DataMember]
        public string shotString1 { get; set; }
        [DataMember]
        public string shotString2 { get; set; }
        
        [IgnoreDataMember]
        private SpriteInfo m_SavedShotInfo;
        [IgnoreDataMember]
        private SpriteInfo m_CurrentShotInfo;

        [DataMember]
        public SpriteInfo SavedShotInfo { get { return m_SavedShotInfo; } set { m_SavedShotInfo = value; } }
        [DataMember]
        public SpriteInfo CurrentShotInfo { get { return m_CurrentShotInfo; } set { m_CurrentShotInfo = value; } }

        private AnimationManager m_FireAnimation;
        public Plasma()
        {
            Spread = (float)Math.PI / 6;
            NumberOfBullets = 1;
            FireRate = 15;
            shotString1 = "rifle1";
            shotString2 = "rifle2";
            m_SightRange = 400;
            Knockback = 250f;
            CanMoveWhileShooting = true;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            LoadTextures(content);
        }
        //foreach line of the shotgun i need to update the lines based on the player center,
        //and rotate it and give it length, then update the graphical lines
        public override void Update(float elapsedTime, Vector2 playerCenter, float rotationAngle, int accuracy, bool shotFired)
        {
            base.Update(elapsedTime, playerCenter, rotationAngle, accuracy, shotFired);
            if (!Firing)
            {
                //float accuracyInRadians = WEAPON_RANDOM.Next(0, accuracy) * ((float)Math.PI / 180);
                //TODO: add a random so its either plus or minus accuracy
                float centerVector = rotationAngle;
                if (NumberOfBullets > 1)
                {
                    float leftAngle = centerVector - (Spread / (NumberOfBullets - 1));
                    LeftAngle = leftAngle;
                }
                else
                {
                    LeftAngle = centerVector;
                }
                
                //foreach (Line line in m_BulletLines)
                //{
                //    line.Update(playerCenter, LeftAngle, SightRange);
                //}
                m_CurrentShotInfo = new SpriteInfo(playerCenter, rotationAngle, NumberOfBullets, LeftAngle);
            }
            //firing a shot, save the state
            if (!Firing && shotFired && CanFire())
            {
                Firing = true;
                m_FireAnimation.SpriteInfo = m_CurrentShotInfo;
                CanDamage = false;
                if (m_FireAnimation.CanStartAnimating())
                    m_FireAnimation.Finished = false;
            }
        }
        public override bool CheckCollision(GameObject ob)
        {
            if (!CanDamage)
            {
                return false;
            }
            //foreach (Line line in m_BulletLines)
            //{
            //    Vector2 check = line.Intersects(ob.m_Bounds);
            //    if (check.X != -1)
            //    {
            //        Vector2 intersectingAngle = new Vector2(line.P2.X - line.P1.X, line.P2.Y - line.P1.Y);
            //        IEnemy enemy = ob as IEnemy;
            //        enemy.ApplyLinearForce(intersectingAngle, Knockback);
            //        return true;
            //    }
            //}
            return false;
        }
        public override void DrawWeapon(SpriteBatch _spriteBatch, Vector2 position, float rot)
        {

        }

        public override void DrawBlast(SpriteBatch _spriteBatch, Vector2 position, float rot)
        {
            if (m_FireAnimation.CanDraw())
            {
                m_FireAnimation.DrawAnimationFrame(_spriteBatch);
                //if frame is at 5
                if (m_FireAnimation.FrameCounter == 20)
                {
                    CanDamage = true;
                }
                //foreach (Line line in m_BulletLines)
                //{
                //    line.Draw(_spriteBatch);
                //}
                if (m_FireAnimation.FrameCounter == 40)
                {
                    CanDamage = false;
                }
            }
            else if (Firing)
            {
                Firing = false;
                m_ElapsedFrames = FireRate;
            }
        }
        public override void LoadWeapon(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            LoadTextures(content);

            //m_BulletLines = new List<Line>();
            //for (int i = 0; i < NumberOfBullets; ++i)
            //{
            //    m_BulletLines.Add(new Line(content));
            //}
        }
        protected override void LoadTextures(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Bullet s = new Bullet("PlasmaBullet", content);
            AnimationInfo[] array = new AnimationInfo[2];
            array[0] = new AnimationInfo(TextureBank.GetTexture(shotString1, content), 20);
            array[1] = new AnimationInfo(TextureBank.GetTexture(shotString2, content), -1);
            m_FireAnimation = new AnimationManager(array, m_SavedShotInfo, 60);
        }   
    }
    public class Bullet : GameObject
    {
        public Bullet(string s, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Texture = TextureBank.GetTexture(s, content);
        }
    }
}