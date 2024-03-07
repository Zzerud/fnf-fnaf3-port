using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SimpleSpriteAnimator
{
    [Serializable]
    [CreateAssetMenu]
    public class SpriteAnimation : ScriptableObject
    {
        [SerializeField]
        private string animationName = "animation";

        public string Name
        {
            get => animationName;
            set => animationName = value;
        }

        [SerializeField]
        private int fps = 24;

        public int FPS
        {
            get => fps;
            set => fps = value;
        }

        [SerializeField]
        private List<SpriteAnimationFrame> frames = new List<SpriteAnimationFrame>();

        public List<SpriteAnimationFrame> Frames
        {
            get => frames;
            set => frames = value;
        }

        [SerializeField]
        private Sprite[] sprites = new Sprite[0];
        [SerializeField]

        private void OnValidate()
        {
            if (sprites.Length > 0)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    SpriteAnimationFrame f = new SpriteAnimationFrame();
                    f.Sprite = sprites[i];
                    frames.Add(f);
                }
                sprites = new Sprite[0];
            }
        }

        [SerializeField]
        private SpriteAnimationType spriteAnimationType = SpriteAnimationType.Looping;
        public SpriteAnimationType SpriteAnimationType
        {
            get => spriteAnimationType;
            set => spriteAnimationType = value;
        }
    }
}
