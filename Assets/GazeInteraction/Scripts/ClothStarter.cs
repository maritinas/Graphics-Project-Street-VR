using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothStarter : MonoBehaviour
{
    public GameObject stillCloth; // The object to hover over
    public GameObject objectWithMovingCloth;

    public void MoveCloth()
    {
        stillCloth.SetActive(false);
        objectWithMovingCloth.SetActive(true);
    }
}

