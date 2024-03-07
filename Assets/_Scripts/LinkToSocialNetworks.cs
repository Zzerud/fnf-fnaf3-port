using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkToSocialNetworks : MonoBehaviour
{
    public void OnClickToLink(string path)
    {
        Application.OpenURL(path);
    }
}
