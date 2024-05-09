/* A CarSpawn object should be placed on a GameObject that has a PathMovement component. 
 * The first pathObject in the PathMovement is used as the spawn point.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarSpawn : MonoBehaviour {

	[SerializeField] private PathMovement path; // Path to spawn cars on
	[SerializeField] private float spawnDelayMean = 3f; // Mean delay between spawn events
	[SerializeField] private float spawnDelayDeviationInterval = 1f; // The interval around the mean which spawn events are allowed, i.e. shortest spawnDelay is (mean-spawnDelayDevationInterval/2) and longest spawnDelay is (mean+spawnDelayDevationInterval/2)
	[SerializeField] private int randomSeed = 255; // The seed for the RNG
	[SerializeField] private bool randomizeSpawnOrder = true; // If false, spawns car prefabs in order
	[SerializeField] private Object[] prefabs = new Object[1]; // The car prefabs

	private Transform spawnPoint;
	private float spawnDelay = 0f;
	private float spawnDelayDeviation = 0f; 
	private int nextPrefab = 0;
	private Random.State oldState;

	private void Awake () {
		Random.InitState (randomSeed);
		oldState = Random.state;
		path = this.gameObject.GetComponent<PathMovement>();
		if (path != null) {
			spawnPoint = path.pathObjects [0];
			Spawn ();
		}
	}

	// Update is called once per frame
	private void FixedUpdate () {
		if (spawnDelay <= spawnDelayMean + spawnDelayDeviation * spawnDelayMean) // Delay next spawn event
			spawnDelay += Time.deltaTime;
		else { // Spawn event
			spawnDelay = 0f;
			Spawn ();
		}
	}

	// Spawn the next car
	private void Spawn() {
		if (path != null) {
			GameObject car = Instantiate (prefabs [nextPrefab], spawnPoint.position, spawnPoint.rotation) as GameObject;
			IncNextPrefab ();
			car.GetComponent<ModifiedCarAIControl> ().SetUpNewSpawn (path);	
		}
	}
		
	private void IncNextPrefab () {
		if (randomizeSpawnOrder) { // Randomize spawn order
			Random.state = oldState;
			spawnDelayDeviation = Random.Range (-spawnDelayDeviationInterval / 2, spawnDelayDeviationInterval / 2);
			nextPrefab = Mathf.FloorToInt (Random.Range (0, prefabs.Length)); // Range is inclusive, floor round for valid index
			oldState = Random.state;
		} else if (nextPrefab == prefabs.Length - 1) // In order spawn, end of prefab list
			nextPrefab = 0;
		else // In order spawn, next in prefab list
			nextPrefab++;
	}


}
