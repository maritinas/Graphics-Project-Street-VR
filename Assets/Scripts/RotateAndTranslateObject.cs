using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RotateAndTranslateObject : MonoBehaviour
{
    public Transform bikeToMove;
    public Transform bikeOgPos;
    public Transform targetPosition;
    private int count = 0;
    public float moveSpeed;  // Speed of movement

    // Update is called once per frame
    public void SlideBike()
    {

        Debug.Log("Slidebike activated");
        // Check if the reference is assigned
        if (bikeToMove != null)
        {
            Debug.Log("BIKE detected");
            if(count==0)
            {
                // Rotate the specific object by 90 degrees on the Z-axis
                bikeToMove.Rotate(90, 0, 0);
                Vector3 translation = targetPosition.position - bikeToMove.position;
                // Move the bike to the target position
                bikeToMove.Translate(translation, Space.World);
                Debug.Log("turn around only once");
                count = count + 1;
            }
            
        }
    }

}

