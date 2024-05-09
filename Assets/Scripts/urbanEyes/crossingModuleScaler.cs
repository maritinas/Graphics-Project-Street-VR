using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]                   // Makes the script runnable in edit mode
public class crossingModuleScaler : MonoBehaviour
{
    public GameObject asphalt;        // The following five variables needs to be reachable induvidually, since they are adjustable.   
    public GameObject paralellStreet1;
    public GameObject paralellStreet2;
    public GameObject[] objectsCloseToRoad1;
    public GameObject[] objectsCloseToRoad2;
    public GameObject[] objectsCloseToApts1; // Add ALL gameobjects that has to move in direction "1" after scaling to here.
    public GameObject[] objectsCloseToApts2; // Add ALL gameobjects that has to move in direction "2" after scaling to here.
    public GameObject sidewalk;
    public float newRoadSize;         // In the inspector for each module, type in desired road width.
    public float newSidewalkSize;     // In the inspector for each module, type in desired sidewalk width.
    private Transform rt;             // The following four variables are just to decrease the amount of writing in the code.
    private Transform sw1t;
    private Transform sw2t;
    private Transform cot;            // Current object tranform
    private Vector3 prevRoadScale;    // The following two variables are needed for calculations.
    private Vector3 prevSWScale;
    private Vector3 temp;             // A temporary Vector3 needed for setting new scales and positions of GameObjects (GOs)


    // Use this for initialization
    void Start()
    {
        rt = asphalt.transform;
        sw1t = paralellStreet1.transform;
        sw2t = paralellStreet2.transform;
        prevRoadScale = rt.localScale;
        prevSWScale = sw1t.localScale;
        cot = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Resize Road")]           // Makes function available in the drop-down menu in the inspector.
    void ScaleRoad()
    {
        if (newRoadSize >= 0)              // The scale must be positive.  
        {
            temp = rt.localScale;  // Change width of road.  
            temp.z = newRoadSize;
            rt.localScale = temp;
            move1(prevRoadScale, rt.localScale.z, objectsCloseToApts1, 0);  // Move GOs in direction "1"
            move2(prevRoadScale, rt.localScale.z, objectsCloseToApts2, 0);  // Move GOs in direction "2"
            move1(prevRoadScale, rt.localScale.z, objectsCloseToRoad1, 0);  // Move GOs in direction "1"
            move2(prevRoadScale, rt.localScale.z, objectsCloseToRoad2, 0);  // Move GOs in direction "2"


        }
        else Debug.Log("Z-Scale must be greater or equal to 0");
        prevRoadScale = rt.localScale;     // Set new previous ZScale-value for next scaling.
    }


    [ContextMenu("Resize Sidewalk")]
    void ScaleSidewalk()
    {
        if (newSidewalkSize >= 0)           // The scale must be positive.
        {
            Vector3 sizeBeforeScale = sidewalk.GetComponent<Renderer>().bounds.size;
            temp = sw1t.localScale; // Change width of sidewalks. 
            temp.x = newSidewalkSize;
            sw1t.localScale = temp;

            temp = sw2t.localScale;
            temp.x = newSidewalkSize;
            sw2t.localScale = temp;
            Vector3 sizeAfterScale = sidewalk.GetComponent<Renderer>().bounds.size;
            float sizeDiference = sizeBeforeScale.z-sizeAfterScale.z;
            Debug.Log(sizeDiference);

            moveSWCrossing1(prevSWScale, sw1t.localScale.x, objectsCloseToApts1, sizeDiference, 0);  // Move GOs in direction "1"
            moveSWCrossing2(prevSWScale, sw1t.localScale.x, objectsCloseToApts2, sizeDiference, 0);  // Move GOs in direction "2"

        }
        else Debug.Log("Rotation Y can only be set to 0 or 90 degrees.");
        prevSWScale = sw1t.localScale;   // Set new previous SWScale-value for next scaling.
    }

    // Takes previous and new scale of either road or sidewalk
    // and moves the GOs on side "1" in direction "1" 
    void move1(Vector3 previousScale, float newScale, GameObject[] gos, int moveExtra)
    {
        if (cot.localEulerAngles.y == 0) // Get and move position of sidewalks when rotation y = 0.
        {
            foreach (GameObject go in gos)
            {
                temp = go.transform.position;
                temp.z = temp.z + (((10 * previousScale.z) - (10 * newScale)) / 2); //reason why prevRoadScale is still .z because the scale is local and the position is global
                if (moveExtra == 1 && go.tag != "sidewalk")
                {
                    temp.z = temp.z + (((10 * previousScale.z) - (10 * newScale)) / 2);
                }
                go.transform.position = temp;
            }

        }
        else if (cot.localEulerAngles.y == 90) //Get and move position of sidewalks when rotation y = 90.
        {
            foreach (GameObject go in gos)
            {
                temp = go.transform.position;
                temp.x = temp.x + (((10 * previousScale.z) - (10 * newScale)) / 2);
                if (moveExtra == 1 && go.tag != "sidewalk")
                {
                    temp.x = temp.x + (((10 * previousScale.z) - (10 * newScale)) / 2);
                }
                go.transform.position = temp;
            }
        }
    }

    // Takes previous and new scale of either road or sidewalk
    // and moves the GOs on side "2" in direction "2" 
    void move2(Vector3 previousScale, float newScale, GameObject[] gos, int moveExtra)
    {
        if (cot.localEulerAngles.y == 0)  //Get and move position of sidewalks when rotation y = 0.
        {
 
           foreach (GameObject go in gos)
            {
                temp = go.transform.position;
                temp.z = temp.z - (((10 * previousScale.z) - (10 * newScale)) / 2); //reason why prevRoadScale is still .z because the scale is local and the position is global.
                if (moveExtra == 1 && go.tag != "sidewalk")
                {
                    temp.z = temp.z - (((10 * previousScale.z) - (10 * newScale)) / 2);
                }
                go.transform.position = temp;

            }
        }
        else if (cot.localEulerAngles.y == 90)  //Get and move position of sidewalks when rotation y = 90.
        {
            foreach (GameObject go in gos)
            {
                temp = go.transform.position;
                temp.x = temp.x - (((10 * previousScale.z) - (10 * newScale)) / 2); //reason why prevRoadScale is still .z because the scale is local and the position is global.
                if (moveExtra == 1 && go.tag != "sidewalk")
                {
                    temp.x = temp.x - (((10 * previousScale.z) - (10 * newScale)) / 2);
                }
                go.transform.position = temp;
            }
        }
    }


    void moveSWCrossing1(Vector3 previousScale, float newScale, GameObject[] gos, float difference, int moveExtra)
    {
        if (cot.localEulerAngles.y == 0) // Get and move position of sidewalks when rotation y = 0.
        {
            Debug.Log(difference);

            foreach (GameObject go in gos)
            {

                temp = go.transform.position;
                Debug.Log(temp.z);
                Debug.Log(temp.z - (difference / 2));
                temp.z = temp.z + (difference / 2); //reason why prevRoadScale is still .z because the scale is local and the position is global
                if (moveExtra == 1 && go.tag != "sidewalk")
                {
                    temp.z = temp.z + (difference / 2);
                }
                go.transform.position = temp;
                Debug.Log(go.transform.position.z);

            }

        }
        else if (cot.localEulerAngles.y == 90) //Get and move position of sidewalks when rotation y = 90.
        {
            foreach (GameObject go in gos)
            {
                temp = go.transform.position;
                temp.x = temp.x + (((10 * previousScale.z) - (10 * newScale)) / 2);
                if (moveExtra == 1 && go.tag != "sidewalk")
                {
                    temp.x = temp.x + (((10 * previousScale.z) - (10 * newScale)) / 2);
                }
                go.transform.position = temp;
            }
        }
    }
    void moveSWCrossing2(Vector3 previousScale, float newScale, GameObject[] gos, float difference, int moveExtra)
    {
        if (cot.localEulerAngles.y == 0)  //Get and move position of sidewalks when rotation y = 0.
        {
            foreach (GameObject go in gos)
            {
                
                temp = go.transform.position;
                temp.z = temp.z - (difference / 2); //reason why prevRoadScale is still .z because the scale is local and the position is global.
                if (moveExtra == 1 && go.tag != "sidewalk")
                {
                    temp.z = temp.z - (difference / 2);
                }
                go.transform.position = temp;
            }
        }
        else if (cot.localEulerAngles.y == 90)  //Get and move position of sidewalks when rotation y = 90.
        {
            foreach (GameObject go in gos)
            {
                temp = go.transform.position;
                temp.x = temp.x - (((10 * previousScale.z) - (10 * newScale)) / 2); //reason why prevRoadScale is still .z because the scale is local and the position is global.
                if (moveExtra == 1 && go.tag != "sidewalk")
                {
                    temp.x = temp.x - (((10 * previousScale.z) - (10 * newScale)) / 2);
                }
                go.transform.position = temp;
            }
        }
    }

    [ContextMenu("checkSize")]
    void checkSizzzzzz()
    {
        //Vector3 colliderSize =  obj.GetComponent<Collider>().bounds.size;
        Vector3 rendererSize = sidewalk.GetComponent<Renderer>().bounds.size;
        //Debug.Log("ColliderSize: " + colliderSize);
        Debug.Log("RendererSize: " + rendererSize);
    }
}