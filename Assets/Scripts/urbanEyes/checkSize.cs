using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class checkSize : MonoBehaviour {
    public GameObject obj; 

	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [ContextMenu("checkSize")]
    void checkSizzzzzz()
    {
        //Vector3 colliderSize =  obj.GetComponent<Collider>().bounds.size;
        Vector3 rendererSize = obj.GetComponent<Renderer>().bounds.size;
        //Debug.Log("ColliderSize: " + colliderSize);
        Debug.Log("RendererSize: " + rendererSize);
    }
}
