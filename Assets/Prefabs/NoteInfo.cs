using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInfo : MonoBehaviour
{    
    public Transform target;
    private void Update()
    {
        if (target == null) return;
        Vector3 p = transform.position;
        p.x = target.position.x;
        transform.position = p;
    }
}
