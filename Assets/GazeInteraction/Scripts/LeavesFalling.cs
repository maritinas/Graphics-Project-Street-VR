using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesFalling : MonoBehaviour
{
    // Reference to the GameObject you want to activate
    public GameObject particleObject;

    // Method to set the GameObject as active
    public void LeavesSetActive()
    {
        if (particleObject != null)
        {
            particleObject.SetActive(true);
        }
    }

    public void LeavesDeactivate()
    {
        if (particleObject != null)
        {
            particleObject.SetActive(false);
        }
    }


}