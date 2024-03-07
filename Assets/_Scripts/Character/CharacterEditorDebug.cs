using SimpleSpriteAnimator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEditorDebug : MonoBehaviour
{
    public SpriteAnimator player;
    public Protagonist protogen;
    public Character character;

    public GameObject firstFrame;
    public GameObject previousFrame;

    // New Version

    [SerializeField] private GameObject buttons, sings_offsets;


    [Space]
    [SerializeField] private Button exit;
    [SerializeField] private TMP_Text NameOfCategories;
    [SerializeField] private TMP_Text NameOfFrame;
    [SerializeField] private Button back, next;
    [SerializeField] private TMP_InputField offsetX, offsetY, rotateZ, rotateY;
    [SerializeField] private Toggle startAnim, copy, copyToAll;

    private int choosenAnim = 0;
    private int currentFrame = 0;

    private bool isAnimate, isLoop, firstF, previousF;

    private void Start()
    {
        //BoyfriendPlayAnimation("Idle");

    }

    public void OpenOffsets(int chooseAnimations)
    {
        buttons.SetActive(false);
        sings_offsets.SetActive(true);
        switch (chooseAnimations)
        {
            case 0:
                // Idle
                NameOfCategories.text = "Idle";
                
                break;
            case 1:
                // Down
                NameOfCategories.text = "Down";
                
                break;
            case 2:
                // Left
                NameOfCategories.text = "Left";
                break;
            case 3:
                // Right
                NameOfCategories.text = "Right";
                break;
            case 4:
                // Up
                NameOfCategories.text = "Up";
                break;
        }
        NameOfFrame.text = character.animations[chooseAnimations].Frames[0].Sprite.name;
        offsetX.text = character.animations[chooseAnimations].Frames[0].Offset.x.ToString();
        offsetY.text = character.animations[chooseAnimations].Frames[0].Offset.y.ToString();
        player.spriteRenderer.sprite = character.animations[chooseAnimations].Frames[0].Sprite;
        player.transform.localPosition = character.animations[chooseAnimations].Frames[0].Offset;
        choosenAnim = chooseAnimations;


        rotateZ.text = character.animations[choosenAnim].Frames[currentFrame].OffsetRotate.eulerAngles.z.ToString();
        rotateY.text = character.animations[choosenAnim].Frames[currentFrame].OffsetRotate.eulerAngles.y.ToString();
        player.transform.localRotation = character.animations[choosenAnim].Frames[currentFrame].OffsetRotate;


        for (int i = 0; i < character.animations.Count; i++)
        {
            player.spriteAnimations[i] = character.animations[i];
        }
        

        currentFrame = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) NextFrames(1);
        if (Input.GetKeyDown(KeyCode.A)) NextFrames(0);
    }
    public void NextFrames(int leftRight)
    {
        if(leftRight == 0 && currentFrame > 0)
        {
            currentFrame--;
        }
        else if(leftRight == 1 && (currentFrame < character.animations[choosenAnim].Frames.Count-1))
        {
            currentFrame++;
        }
        NameOfFrame.text = character.animations[choosenAnim].Frames[currentFrame].Sprite.name;
        offsetX.text = character.animations[choosenAnim].Frames[currentFrame].Offset.x.ToString();
        offsetY.text = character.animations[choosenAnim].Frames[currentFrame].Offset.y.ToString();
        rotateZ.text = character.animations[choosenAnim].Frames[currentFrame].OffsetRotate.eulerAngles.z.ToString();
        rotateY.text = character.animations[choosenAnim].Frames[currentFrame].OffsetRotate.eulerAngles.y.ToString();
        player.spriteRenderer.sprite = character.animations[choosenAnim].Frames[currentFrame].Sprite;

        player.transform.localPosition = character.animations[choosenAnim].Frames[currentFrame].Offset;
        player.transform.localRotation = character.animations[choosenAnim].Frames[currentFrame].OffsetRotate;
    }

    public void ChangeOffsetX(string finalOffset)
    {
        ChangeOffsetX(finalOffset, currentFrame);
    }
    public void ChangeOffsetY(string finalOffset)
    {
        ChangeOffsetY(finalOffset, currentFrame);
    }

    public void ChangeRotateX(string finalOffset)
    {
        ChangeOffsetX(finalOffset, currentFrame, true, true);
    }
    public void ChangeRotateY(string finalOffset)
    {
        ChangeOffsetY(finalOffset, currentFrame, true, true);
    }

    public void ChangeOffsetX(string finalOffset, int curFrame, bool save = true, bool isRotate = false)
    {
        float result;
        if (!float.TryParse(finalOffset, out result)) return;
        if (!isRotate)
        {
            character.animations[choosenAnim].Frames[curFrame].Offset = new Vector2(result, character.animations[choosenAnim].Frames[curFrame].Offset.y);
            offsetX.text = character.animations[choosenAnim].Frames[curFrame].Offset.x.ToString();

            player.transform.localPosition = character.animations[choosenAnim].Frames[curFrame].Offset;
        }
        else
        {
            Quaternion w;
            w = Quaternion.Euler(character.animations[choosenAnim].Frames[curFrame].OffsetRotate.x, character.animations[choosenAnim].Frames[curFrame].OffsetRotate.y, result);
            character.animations[choosenAnim].Frames[curFrame].OffsetRotate = w;
            rotateZ.text = character.animations[choosenAnim].Frames[curFrame].OffsetRotate.eulerAngles.z.ToString();

            player.transform.localRotation = character.animations[choosenAnim].Frames[curFrame].OffsetRotate;
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(character.animations[choosenAnim]);
        if (save)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
    public void ChangeOffsetY(string finalOffset, int curFrame, bool save = true, bool isRotate = false)
    {
        float result;
        if (!float.TryParse(finalOffset, out result)) return;
        if (!isRotate)
        {
            character.animations[choosenAnim].Frames[curFrame].Offset = new Vector2(character.animations[choosenAnim].Frames[curFrame].Offset.x, result);
            offsetY.text = character.animations[choosenAnim].Frames[curFrame].Offset.y.ToString();

            player.transform.localPosition = character.animations[choosenAnim].Frames[curFrame].Offset;
        }
        else
        {
            Quaternion w;
            w = Quaternion.Euler(character.animations[choosenAnim].Frames[curFrame].OffsetRotate.x, result, character.animations[choosenAnim].Frames[curFrame].OffsetRotate.z);
            character.animations[choosenAnim].Frames[curFrame].OffsetRotate = w;
            rotateY.text = character.animations[choosenAnim].Frames[curFrame].OffsetRotate.eulerAngles.y.ToString();

            player.transform.localRotation = character.animations[choosenAnim].Frames[curFrame].OffsetRotate;
        }
        

#if UNITY_EDITOR
        EditorUtility.SetDirty(character.animations[choosenAnim]);
        if(save)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }   
#endif
    }

    public void OnToggleAnimations()
    {
        if (isLoop)
        {
            isAnimate = !isAnimate;
            if (isAnimate)
            {
                switch (choosenAnim)
                {
                    case 0:
                        CharacterPlayAnimation("Idle");
                        break;
                    case 1:
                        CharacterPlayAnimation("Sing Down");
                        break;
                    case 2:
                        CharacterPlayAnimation("Sing Left");
                        break;
                    case 3:
                        CharacterPlayAnimation("Sing Right");
                        break;
                    case 4:
                        CharacterPlayAnimation("Sing Up");
                        break;
                }
            }
        }
        else
        {
            startAnim.isOn = false;
            isAnimate = false;
            Debug.Log(choosenAnim);
            switch (choosenAnim)
            {
                case 0:
                    CharacterPlayAnimation("Idle");
                    break;
                case 1:
                    CharacterPlayAnimation("Sing Down");
                    break;
                case 2:
                    CharacterPlayAnimation("Sing Left");
                    break;
                case 3:
                    CharacterPlayAnimation("Sing Right");
                    break;
                case 4:
                    CharacterPlayAnimation("Sing Up");
                    break;
            }

        }

    }

    public void OnToggleLoop()
    {
        isLoop = !isLoop;
        if(isLoop)
            character.animations[choosenAnim].SpriteAnimationType = SpriteAnimationType.Looping;
        else
            character.animations[choosenAnim].SpriteAnimationType = SpriteAnimationType.PlayOnce;

    }

    public void OnToggleFirstFrame()
    {
        firstF = !firstF;
        firstFrame.SetActive(firstF);
        firstFrame.GetComponent<SpriteRenderer>().sprite = character.animations[0].Frames[0].Sprite;
    }

    public void OnTogglePreviousFrame()
    {
        previousF = !previousF;
        previousFrame.SetActive(previousF);
        if(currentFrame > 0)
            previousFrame.GetComponent<SpriteRenderer>().sprite = character.animations[choosenAnim].Frames[currentFrame-1].Sprite;
        else
            previousFrame.GetComponent<SpriteRenderer>().sprite = character.animations[choosenAnim].Frames[currentFrame].Sprite;
    }

    public void OnToggleCopy()
    {
        copy.isOn = false;
        if(currentFrame > 0)
        {
            ChangeOffsetX(character.animations[choosenAnim].Frames[currentFrame - 1].Offset.x.ToString());
            ChangeOffsetY(character.animations[choosenAnim].Frames[currentFrame - 1].Offset.y.ToString());
        }
        
    }
    public void OnToggleCopyAll()
    {
        copyToAll.isOn = false;
        for (int i = currentFrame; i < character.animations[choosenAnim].Frames.Count; i++)
        {

            ChangeOffsetX(character.animations[choosenAnim].Frames[currentFrame].Offset.x.ToString(), i, false);
            ChangeOffsetY(character.animations[choosenAnim].Frames[currentFrame].Offset.y.ToString(), i, false);
            /*Debug.Log(character.animations[choosenAnim].Frames[currentFrame].OffsetRotate.eulerAngles.z.ToString());
            ChangeOffsetX(character.animations[choosenAnim].Frames[currentFrame].OffsetRotate.eulerAngles.z.ToString(), i, false, true);
            ChangeOffsetY(character.animations[choosenAnim].Frames[currentFrame].OffsetRotate.eulerAngles.y.ToString(), i, false, true);*/
        }

#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif


    }


    private void BoyfriendPlayAnimation(string animationName)
    {
        player.Play("BF " + animationName);
    }
    private void CharacterPlayAnimation(string animationName)
    {
        Debug.Log(animationName);
        player.Play(animationName);
    }

}
