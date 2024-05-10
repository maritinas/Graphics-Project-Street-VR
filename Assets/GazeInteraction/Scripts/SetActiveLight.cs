using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveLight : MonoBehaviour
{
    // Reference to the GameObject you want to activate
    public GameObject targetGameObject;
    public Material carLightOnMaterial;
    public Material carLightOffMaterial; 

    // Reference to the Renderer component you want to change the material of
    public Renderer carRendererR;
    public Renderer carRendererL;

    // Method to set the GameObject as active
    public void LightSetActive()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(true); // Set the GameObject as active
            if (carRendererL && carRendererR != null && carLightOnMaterial != null)
            {
                targetGameObject.SetActive(true);
                carRendererR.material = carLightOnMaterial; // Change the material of the renderer
                carRendererL.material = carLightOnMaterial;
            }
            else
            {
                Debug.LogWarning("Renderer or material not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }

    public void LightDeActive()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(false); // Set the GameObject as inactive
            carRendererR.material = carLightOffMaterial; // Change the material of the renderer
            carRendererL.material = carLightOffMaterial;
        }
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }
}

