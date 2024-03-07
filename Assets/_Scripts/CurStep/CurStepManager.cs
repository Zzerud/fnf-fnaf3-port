using UnityEngine;

public class CurStepManager : MonoBehaviour
{
    public static CurStepManager Instance { get; private set; }

    public AudioSource clip;
    public float stepCrochet;

    private float _songPos;
    private int _stepsPerformed;

    private StepBehaviour[] _behaviours;

    private void Start()
    {
        Instance = this;
        _songPos = 0;
        _stepsPerformed = 0;
    }

    public void NewSong()
    {
        _songPos = 0;
        _stepsPerformed = 0;
        _behaviours = (StepBehaviour[])FindObjectsByType(typeof(StepBehaviour), FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (clip.isPlaying)
        {
            _songPos = clip.time * 1000;
            
            int newStep = Mathf.FloorToInt((_songPos) / stepCrochet);

            while (_stepsPerformed < newStep)
            {
                for (int i = 0; i < _behaviours.Length; i++)
                {
                    if (_behaviours[i].Enabled)
                        _behaviours[i].StepHit(_stepsPerformed);
                }
                _stepsPerformed++;
            }
        }
    }
}
