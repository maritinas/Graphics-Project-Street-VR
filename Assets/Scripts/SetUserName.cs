using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUserName : MonoBehaviour
{
   public string USERNAME;
   
    public void Start()
    {
        if(USERNAME != "")
            {
                Debug.Log("Username" + USERNAME);
            }
            else {
    Debug.LogWarning("Set username");
            }
    
    }

    
}
