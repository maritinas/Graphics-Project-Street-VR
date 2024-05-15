using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RotateAndTranslateObject : MonoBehaviour
{
    public GameObject bikeToMove;
    public Transform targetPosition;
    public float moveSpeed;  // Speed of movement

    // Update is called once per frame
    public void SlideBike()
    {
        Vector3 targetDirection = targetPosition.position - bikeToMove.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Debug.Log("Bike is rotating");

        // Only consider rotation around the X axis
        Quaternion xRotation = Quaternion.Euler(targetRotation.eulerAngles.x, 0, 0);

        bikeToMove.transform.rotation = xRotation;

        bikeToMove.transform.position = Vector3.MoveTowards(
            bikeToMove.transform.position,
            targetPosition.position,
            moveSpeed * Time.deltaTime);

        // the second argument, upwards, defaults to Vector3.up
       /* Quaternion rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        bikeToMove.transform.rotation = rotation;*/

        //set the up direction
        /* bikeToMove.transform.rotation = Quaternion.RotateTowards(
             bikeToMove.transform.rotation,
             targetRotation,
             moveSpeed * Time.deltaTime);*/
        bikeToMove.transform.position = Vector3.MoveTowards(
       bikeToMove.transform.position,
       targetPosition.position,
       moveSpeed * Time.deltaTime);

    }

}

