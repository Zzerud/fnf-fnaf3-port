using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestFireScene : MonoBehaviour
{
    public void Click()
    {
        SceneManager.LoadScene("TestFire");
    }
}
