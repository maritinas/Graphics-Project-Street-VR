using UnityEngine;
using System.Collections;

public class MoveOnPath : MonoBehaviour {

	public PathMovement PathToFollow;

	public int CurrentWaypointID = 0;
	public float speed;
	public float reachDistance = 1.0f; //smoothing
	public float rotationSpeed = 5.0f;
	public string pathName;
	public bool returnToStartScene = false;
	Vector3 last_position;
	Vector3 current_position;

	// Use this for initialization
	void Start () {
		//PathToFollow = GameObject.Find(pathName).GetComponent<PathMovement>();
		last_position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(PathToFollow.pathObjects[CurrentWaypointID].position, transform.position);
		transform.position = Vector3.MoveTowards(transform.position, PathToFollow.pathObjects[CurrentWaypointID].position, Time.deltaTime * speed);

		var rotation = Quaternion.LookRotation(PathToFollow.pathObjects[CurrentWaypointID].position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

		if (distance <= reachDistance){
			CurrentWaypointID ++;
		}

		if (CurrentWaypointID >= PathToFollow.pathObjects.Count){

			if (returnToStartScene){
				UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");	
			}

			else{
				CurrentWaypointID = 0;
			}		
		}



	}
}
