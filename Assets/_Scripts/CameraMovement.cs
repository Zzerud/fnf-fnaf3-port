using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraMovement : MonoBehaviour
{
    public PostProcessVolume volume;
    public static CameraMovement instance;
    public bool focusOnPlayerOne;
    public bool focusWhenAnimations = false;
    public bool focusWhenAnimationsPlayer = false;

    [Space] public Transform playerOne;
    public Vector2 playerOneOffset;
    public float orthographicSizePlayerOne;

    [Space] public Transform playerTwo;
    public Vector2 playerTwoOffset;
    public float orthographicSizePlayerTwo;

    [Space] public float speed;
    public bool enableMovement = true;
    
    private Vector3 _defaultPos;
    private float _defaultSize;
    private Camera _camera;



    public Vector2 _defaultPositionPlayer1;
    public Vector2 _defaultPositionPlayer2;
    public float _defaultOrthographicSizePlayer1;
    public float _defaultOrthographicSizePlayer2;

    public float yPosEnemy, yPosPlayer;
    public float speedY = 0.2f;
    public bool isFly = false;
    public bool startBob;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        _camera = GetComponent<Camera>();
        _defaultPos = _camera.transform.position;
        _defaultSize = _camera.orthographicSize;
        if (!OptionsV2.PostProcessing)
        {
            volume.enabled = false;
            gameObject.GetComponent<PostProcessLayer>().enabled = false;
        }
    }

    // Update is called once per frame


    void Update()
    {
        if(focusWhenAnimations) focusOnPlayerOne = false;
        else if(focusWhenAnimationsPlayer) focusOnPlayerOne = true;
        if (Song.instance == null) return;
        
        if (Song.instance.songStarted & !OptionsV2.LiteMode & enableMovement)
        {
            var newOffset = focusOnPlayerOne ? playerOne.position : playerTwo.position;
            newOffset = focusOnPlayerOne ? playerOneOffset : playerTwoOffset;
            newOffset.z = -50;
            float cameraSize = focusOnPlayerOne ? orthographicSizePlayerOne : orthographicSizePlayerTwo;
            if(isFly)
                newOffset.y = focusOnPlayerOne ? Mathf.Sin(Time.time * speedY) + yPosPlayer + .8f : Mathf.Sin(Time.time * speedY) + yPosEnemy + .8f;
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, newOffset, Time.deltaTime * 6);
            Song.instance.defaultGameZoom = cameraSize;
            float distance = Vector3.Distance(_camera.transform.position, newOffset);
            if(distance > 0.00001f && startBob)
            {
                if (Song.instance.enemy.isBobingWhileSing && Song.instance.isActiveShake)
                {
                    CameraShake.instance.StartShake(0.05f, 0.04f);
                }
                startBob = false;
            }

        }
        else
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _defaultPos, speed);
            Song.instance.defaultGameZoom = _defaultSize;
        }
        //playerTwoOffset.z = Mathf.Lerp(playerTwoOffset.z, zPos, Time.deltaTime * 2);
    }


}
