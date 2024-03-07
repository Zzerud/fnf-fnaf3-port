using UnityEngine;

public abstract class StepBehaviour : MonoBehaviour
{
    public bool Enabled;
    public abstract void StepHit(int curStep);
}
