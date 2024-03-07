using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditPeopleBtn : MonoBehaviour
{

    public GameObject[] CreditPeople;
    private int currentInt;
    public void ChangeImg(int id)
    {
        CreditPeople[currentInt].SetActive(false);
        currentInt = id;
        CreditPeople[id].SetActive(true);
    }
}
