using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacter : MonoBehaviour
{
    public string[] characters;

    private void Start()
    {
        GameObject prefab = Resources.Load<GameObject>(characters[Random.Range(0, characters.Length)]);
        GameObject go = Instantiate(prefab, transform);
    }
}
