using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public static CameraZoom Instance { get; private set; }



    float zFirstPlayer;
    float zFirstEnemy;

    float zEndPlayer;
    float zEndEnemy;

    float elapsedTime = 0;
    string timeToChange;
    float timeToChanges;

    private void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ChangeCameraZoom(object to, object time)
    {
        /*Debug.Log(time);
        float sto = float.Parse(to.ToString(), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        //float stime = float.Parse(time.ToString(), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);
        
            elapsedTime = 0;
            timeToChanges = 0;
            zFirstPlayer = CameraMovement.instance.playerOneOffset.z;
            zFirstEnemy = CameraMovement.instance.playerTwoOffset.z;

            zEndPlayer = CameraMovement.instance.playerOneOffset.z / sto;
            zEndEnemy = CameraMovement.instance.playerTwoOffset.z / sto;

            timeToChange = time.ToString();

        float stime = float.Parse(timeToChange, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture);*/

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(timeToChange.ToString());
        if (elapsedTime < timeToChanges)
        {
           /* CameraMovement.instance.zPosEnemy = Mathf.Lerp(zFirstEnemy, zEndEnemy, timeToChanges * Time.deltaTime);
            CameraMovement.instance.zPosPlayer = Mathf.Lerp(zFirstPlayer, zEndPlayer, timeToChanges * Time.deltaTime);
            elapsedTime += Time.deltaTime;*/
            
        }
        
        
    }
}
