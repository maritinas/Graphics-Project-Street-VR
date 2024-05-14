using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMoving : MonoBehaviour
{
    public GameObject shadowToMove; // The shadow to move
    public GameObject objectToObserve; // The object that triggers movement when hovered
    public Transform targetPosition; // The target position for the shadow
    public float moveSpeed = 6.0f; // Speed of movement

    public void MoveShadow()
    {
        // Move the shadow toward the target position
        shadowToMove.SetActive(true);
        shadowToMove.transform.position = Vector3.MoveTowards(
            shadowToMove.transform.position,
            targetPosition.position, // Use target's position
            moveSpeed * Time.deltaTime);
    }
}
