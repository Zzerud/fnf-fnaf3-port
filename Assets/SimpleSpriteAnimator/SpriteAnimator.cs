using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SimpleSpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        public bool isCharacter = false;
        [SerializeField] public List<SpriteAnimation> spriteAnimations;

        [SerializeField] public bool playAutomatically = true;

        public SpriteAnimation DefaultAnimation
        {
            get { return spriteAnimations.Count > 0 ? spriteAnimations[0] : null; }
        }

        public SpriteAnimation CurrentAnimation
        {
            get { return spriteAnimationHelper.CurrentAnimation; }
        }

        public bool Playing
        {
            get { return state == SpriteAnimationState.Playing; }
        }

        public bool Paused
        {
            get { return state == SpriteAnimationState.Paused; }
        }

        [HideInInspector]
        public SpriteRenderer spriteRenderer;

        public Image img;
        public bool isNew = false;

        public SpriteAnimationHelper spriteAnimationHelper;

        private SpriteAnimationState state = SpriteAnimationState.Playing;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //img = GetComponent<Image>();
            spriteAnimationHelper = new SpriteAnimationHelper();
        }

        public void Start()
        {
            if (playAutomatically)
            {
                Play(DefaultAnimation);
            }
        }

        private void LateUpdate()
        {
            if (Playing)
            {
                SpriteAnimationFrame currentFrame = spriteAnimationHelper.UpdateAnimation(Time.deltaTime);

                if (currentFrame != null)
                {
                    spriteRenderer.sprite = currentFrame.Sprite;
                    if(isNew)
                    img.sprite = currentFrame.Sprite;
                    transform.localPosition = currentFrame.Offset;
                    if(isCharacter)
                        transform.localRotation = currentFrame.OffsetRotate;
                }
            }
        }

        public void Play()
        {
            if (CurrentAnimation == null)
            {
                spriteAnimationHelper.ChangeAnimation(DefaultAnimation);
            }

            Play(CurrentAnimation);
        }

        public void Play(string name)
        {

            SpriteAnimation animation = GetAnimationByName(name);
            if (animation != null)
            {
                Play(animation);
            }
            else
            {
                Debug.LogError("Animation with name " + name + " not found!");
            }
        }

        public void Play(SpriteAnimation animation)
        {
            state = SpriteAnimationState.Playing;
            spriteAnimationHelper.ChangeAnimation(animation);
        }
 
        private SpriteAnimation GetAnimationByName(string name)
        {
            foreach (var t in spriteAnimations)
            {
                if (t.Name == name)
                {
                    return t;
                }
            }

            
            return null;
        }
    }
}
