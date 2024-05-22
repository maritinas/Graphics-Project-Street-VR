using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothStarter : MonoBehaviour
{
    public Cloth stillCloth; // The object to hover over
    public GameObject objectWithMovingCloth;

    public void MoveCloth()
    {
        stillCloth.externalAcceleration = new Vector3(3, 1, 1);
        //objectWithMovingCloth.SetActive(true);
    }

    public void DeactiveCloth()
    {
        stillCloth.externalAcceleration = new Vector3(0, 0, -0.5f);
        //objectWithMovingCloth.SetActive(false);
    }
}

