﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace GameName1
{
    //got new state setup on refreshed system, checking from refresh
    //a texture, how many frames the animation should play for, 
    public class AnimationInfo
    {
        public Texture2D Texture;
        public int NextFrame;
		
        public AnimationInfo(Texture2D tex, int frame)
        {
            Texture = tex;
            NextFrame = frame;
        }
    }
    public class SpriteInfo 
    {
        [DataMember]
        public Vector2 Position;
        [DataMember]
        public float Rotation;
        [DataMember]
        public int NumberOfBullets;
        [DataMember]
        public float LeftAngle; //left most of a spread shot
        [DataMember]
        public Vector2 PlayerVelocity;
        public SpriteInfo(Vector2 pos, Vector2 velocity, float rot, int numBul, float left)
        {
            Position = pos;
            PlayerVelocity = velocity;
            Rotation = rot;
            NumberOfBullets = numBul;
            LeftAngle = left;
        }
    }
    class AnimationManager
    {
        AnimationInfo[] AnimationArray;
        private SpriteInfo m_SpriteInfo;
        public SpriteInfo SpriteInfo { set   { m_SpriteInfo = value; m_CurrentSprite = 0; FrameCounter = 0;} }
        public bool Finished { get; set; }
        private int m_CurrentSprite;
        public int CurrentSprite { get { return m_CurrentSprite; } }
        public bool Animating { get; set; }
        public int MaxFrames {get;set;}
        public int FrameCounter {get;set;}
        public AnimationManager(AnimationInfo[] array, SpriteInfo shotInfo, int maxFrames)
        {
            m_SpriteInfo = shotInfo;
            AnimationArray = array;
            Finished = false;
            m_CurrentSprite = 0;
            MaxFrames = maxFrames;
        }

        public void SetAnimation(AnimationInfo[] array)
        {
            AnimationArray = array;
            m_CurrentSprite = 0;
        }
        public void SetSpriteInfo(SpriteInfo info)
        {
            m_SpriteInfo = info;
        }
        public void DrawAnimationFrame(SpriteBatch _spriteBatch)
        {
            if (FrameCounter < MaxFrames)
            {
                Animating = true;
                Finished = false;
                if (FrameCounter == AnimationArray[m_CurrentSprite].NextFrame)
                {
                    ++m_CurrentSprite;
                }
                _spriteBatch.Draw(AnimationArray[m_CurrentSprite].Texture, m_SpriteInfo.Position, null, Color.White, m_SpriteInfo.Rotation, new Vector2(0, AnimationArray[m_CurrentSprite].Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
                ++FrameCounter;
            }
            else
            {
                Animating = false;
                Finished = true;
            }
        }
            
        public bool CanStartAnimating()
        {
            return !Animating && Finished;
        }

        public bool CanDraw()
        {
            return Animating || !Finished;
        }
    }
}
