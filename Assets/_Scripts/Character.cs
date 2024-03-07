using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SimpleSpriteAnimator;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "New Character", menuName = "Create New Character", order = 0),Serializable] 
public class Character : ScriptableObject
{
    public string characterName = string.Empty;
    [JsonIgnore] public List<SpriteAnimation> animations;
    public Character()
    {
        animations = new List<SpriteAnimation>();
    }
    public bool idleOnly = false;
    [Header("Floating")] public bool doesFloat;
    public float floatToOffset;
    public float floatSpeed;

    [Header("Size")] public float scale = 1;

    [FormerlySerializedAs("offset")]
    [Header("Camera")]
    public Vector2 cameraOffset = new Vector3(2, 6);
    public float orthographicSize = 5.5f;
    public float yToUp, yToDown, xToLeft, xToRight;

    [Header("Portrait"), JsonIgnore] public Sprite portrait;
    public Sprite portraitDead;
    public Sprite portraitWin;
    public Vector2 portraitSize;
    public bool isRotatedIcon = false;

    [Header("WithNewPos")]
    public bool isCustomPosition = false;
    public Vector3 newTransform;

    [Space]
    public bool changeColor;
    public Color healthColor = Color.red;
    public Color songDurationColor;

    [Space]
    public bool isRotated = false;
    public bool isBobingWhileSing = false;
}
