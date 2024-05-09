using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class AssembleBlocks : MonoBehaviour {





	private List<Transform> corner_LOW = new List<Transform>();
	private List<Transform> center_LOW = new List<Transform>();

	private List<Transform> corner_MID = new List<Transform>();
	private List<Transform> center_MID = new List<Transform>();

	private List<Transform> corner_TOP = new List<Transform>();
	private List<Transform> center_TOP = new List<Transform>();

	public int scalingFactor;

	private string buildingPart;
//	private Dictionary<Transform, string> partDic = new Dictionary<Transform, string>();

	private Vector3 currentPosition = new Vector3(0,0,0);
	private int houseWidth;
	private int houseDepth;

	Transform tempTrans;

	private int currentLevel = 0;
	int lastLevel = 0;
	int buildingLevels;
	public GameObject ScaleRotTranslateObject;

	float levelHeight;
	Vector3 boundingBox;
	// Use this for initialization
	void Start () {
		buildingLevels = Random.Range(3,12);
		//buildingLevels = 6;
		AddPartsToList();

		houseWidth = ChooseRandomLength();
		houseDepth = ChooseRandomLength();
		houseWidth = 5;
		houseDepth = 4;
		buildingLevels = 2;
		Debug.Log("CurrentLevel: " + currentLevel);
		for (int i = 0; i < buildingLevels; i++){
			InstantiateFront();
			InstantiateLeft();
			InstantiateBack();
			InstantiateRight();
			currentLevel += 1;
			Debug.Log("CurrentLevel: " + currentLevel);
		}

	}

	float CalculateBuildingBlockHeight(GameObject blockPart){

		Renderer rend;
		rend = blockPart.GetComponent<Renderer>();
		Vector3 max = rend.bounds.max;
		Vector3 min = rend.bounds.min;
		levelHeight = max.y - min.y;
		float mag = rend.bounds.extents.magnitude;
		Debug.Log("Max: " + max + " , min: " + min);
		Debug.Log("Mag: " + mag + " höjd: " + levelHeight);

//		Vector3 boundBox = max - min;
		return levelHeight;






	}


		
	void AddPartsToList(){

		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Corner/Low", typeof(GameObject))){
			corner_LOW.Add(g.transform);
		}
		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Corner/Mid", typeof(GameObject))){
			corner_MID.Add(g.transform);
		}
		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Corner/Top", typeof(GameObject))){
			corner_TOP.Add(g.transform);
		}
		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Center/Low", typeof(GameObject))){
			center_LOW.Add(g.transform);
		}
		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Center/Mid", typeof(GameObject))){
			center_MID.Add(g.transform);
		}
		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Center/Top", typeof(GameObject))){
			center_TOP.Add(g.transform);
		}
	
	}

	void InstantiateFront(){




		// Instantiate First Corner
		GameObject firstCorner;
		if (currentLevel == 0){						// Första våningen
			
			firstCorner = (GameObject)Instantiate(corner_LOW.First().gameObject, currentPosition, corner_LOW.First().gameObject.transform.rotation);

			levelHeight = CalculateBuildingBlockHeight(corner_LOW.First().gameObject);
			//levelHeight = boundingBox.y;
			}
		else if (currentLevel == buildingLevels - 1){ // Sista våningen
				firstCorner = (GameObject)Instantiate(corner_TOP.First().gameObject, currentPosition, corner_TOP.First().gameObject.transform.rotation);
			}
		else{
				firstCorner = (GameObject)Instantiate(corner_MID.First().gameObject, currentPosition, corner_LOW.First().gameObject.transform.rotation);
			}

		firstCorner.transform.localScale *= scalingFactor;
		currentPosition = firstCorner.transform.position; //+ new Vector3(0f,0f,1f)*scalingFactor;
		firstCorner.transform.Translate(firstCorner.transform.up*(scalingFactor*0.5f));
		//firstCorner.transform.Translate(firstCorner.transform.up*(levelHeight*0.5f*currentLevel));
		firstCorner.transform.Translate(firstCorner.transform.up*(levelHeight*2*currentLevel));
		firstCorner.gameObject.name = "FirstCorner";

		// Take random house length and instantiate middle blocks
		for (int i = 0; i < houseWidth; i++){
			Transform part;

			if (currentLevel == 0){						// Första våningen
				part = center_LOW.First();
			}
			else if (currentLevel == buildingLevels - 1){ // Sista våningen
				part = center_TOP.First();
			}
			else{
				part = center_MID.First();
			}
				
			GameObject test1 = (GameObject)Instantiate(part.gameObject, currentPosition, center_TOP.First().gameObject.transform.rotation);
//			Debug.Log("Cur. Pos.:" + currentPosition);
			test1.transform.localScale *= scalingFactor;
			test1.transform.position += test1.transform.right * scalingFactor; // right = + i x-axis
			currentPosition = test1.transform.position;
			test1.gameObject.name = "middleBlock" + currentLevel.ToString();
			//test1.transform.Translate(test1.transform.up*(scalingFactor*0.5f));
			//test1.transform.Translate(test1.transform.up*(scalingFactor*(currentLevel-lastLevel)*0.5f));
			//test1.transform.Translate(test1.transform.up*(scalingFactor*(levelHeight)*0.5f));
			test1.transform.Translate(firstCorner.transform.up*(scalingFactor*0.5f));
			//firstCorner.transform.Translate(firstCorner.transform.up*(levelHeight*0.5f*currentLevel));
			test1.transform.Translate(firstCorner.transform.up*(levelHeight*2*currentLevel));

		}

		// When done, instantiate second corner

		Transform cornerPart;

		if (currentLevel == 0){						// Första våningen
			cornerPart = corner_LOW.First();
		}
		else if (currentLevel == buildingLevels - 1){ // Sista våningen
			cornerPart = corner_TOP.First();
		}
		else{
			cornerPart = corner_MID.First();
		}


		GameObject secondCorner = (GameObject)Instantiate(cornerPart.gameObject, currentPosition, corner_TOP.First().gameObject.transform.rotation);
		secondCorner.transform.localScale *= scalingFactor;
		secondCorner.transform.position += secondCorner.transform.right*scalingFactor;
		currentPosition = secondCorner.transform.position;
		secondCorner.transform.Translate(secondCorner.transform.up*(scalingFactor*0.5f));
		secondCorner.transform.Translate(secondCorner.transform.up*(scalingFactor*(currentLevel-lastLevel)*0.5f));
		secondCorner.gameObject.name = "Second Corner";

	}


	void InstantiateLeft(){
		

	
		// Take random house length and instantiate middle blocks
		for (int i = 0; i < houseDepth; i++){

			Transform midPart;

			if (currentLevel == 0){						// Första våningen
				
				midPart = center_LOW.First();
			}
			else if (currentLevel == buildingLevels - 1){ // Sista våningen
				midPart = center_TOP.First();
			}
			else{
				midPart = center_MID.First();
			}
				

			GameObject test1 = (GameObject)Instantiate(midPart.gameObject,  currentPosition, center_TOP.First().gameObject.transform.rotation);
			test1.transform.localScale *= scalingFactor;
			test1.transform.position += -test1.transform.forward*scalingFactor;
			currentPosition = test1.transform.position;
			test1.transform.Translate(test1.transform.up*(scalingFactor*0.5f));
			test1.transform.Translate(test1.transform.up*(scalingFactor*(currentLevel-lastLevel)*0.5f));
		}

		// When done, instantiate third corner
		Transform cornerPart;

		if (currentLevel == 0){						// Första våningen
			cornerPart = corner_LOW.First();
		}
		else if (currentLevel == buildingLevels - 1){ // Sista våningen
			cornerPart = corner_TOP.First();
		}
		else{
			cornerPart = corner_MID.First();
		}

		GameObject thirdCorner = (GameObject)Instantiate(cornerPart.gameObject, currentPosition, corner_TOP.First().gameObject.transform.rotation);
		thirdCorner.transform.localScale *= scalingFactor;
		thirdCorner.transform.position += -thirdCorner.transform.forward*scalingFactor;
		currentPosition = thirdCorner.transform.position;
		thirdCorner.transform.Translate(thirdCorner.transform.up*(scalingFactor*0.5f));
		thirdCorner.transform.Translate(thirdCorner.transform.up*(scalingFactor*(currentLevel-lastLevel)*0.5f));
	}



	void InstantiateBack(){

		// Take random house length and instantiate middle blocks
		for (int i = 0; i < houseWidth; i++){

			Transform midPart;

			if (currentLevel == 0){						// Första våningen
				midPart = center_LOW.First();
			}
			else if (currentLevel == buildingLevels - 1){ // Sista våningen
				midPart = center_TOP.First();
			}
			else{
				midPart = center_MID.First();
			}
				
			GameObject test1 = (GameObject)Instantiate(midPart.gameObject,  currentPosition, center_TOP.First().gameObject.transform.rotation);
			test1.transform.localScale *= scalingFactor;
			test1.transform.position += -test1.transform.right*scalingFactor;
			currentPosition = test1.transform.position;
			test1.gameObject.name = "Backcenter";
			test1.transform.Translate(test1.transform.up*(scalingFactor*0.5f));
			test1.transform.Translate(test1.transform.up*(scalingFactor*(currentLevel-lastLevel)*0.5f));
		}

		// When done, instantiate third corner

		Transform cornerPart;

		if (currentLevel == 0){						// Första våningen
			cornerPart = corner_LOW.First();
		}
		else if (currentLevel == buildingLevels - 1){ // Sista våningen
			cornerPart = corner_TOP.First();
		}
		else{
			cornerPart = corner_MID.First();
		}

		GameObject fourthCorner = (GameObject)Instantiate(cornerPart.gameObject, currentPosition, corner_TOP.First().gameObject.transform.rotation);
		fourthCorner.transform.localScale *= scalingFactor;
		fourthCorner.transform.position += -fourthCorner.transform.right*scalingFactor;
		fourthCorner.gameObject.name = "FourthCorner";
		currentPosition = fourthCorner.transform.position;
		fourthCorner.transform.Translate(fourthCorner.transform.up*(scalingFactor*0.5f));
		fourthCorner.transform.Translate(fourthCorner.transform.up*(scalingFactor*(currentLevel-lastLevel)*0.5f));

	}

	void InstantiateRight(){
		
		// Take random house length and instantiate middle blocks
		for (int i = 0; i < houseDepth; i++){

			Transform midPart;

			if (currentLevel == 0){						// Första våningen
				midPart = center_LOW.First();
			}
			else if (currentLevel == buildingLevels - 1){ // Sista våningen
				midPart = center_TOP.First();
			}
			else{
				midPart = center_MID.First();
			}
			Debug.Log("Curr: " + currentLevel + ", buildinglevels: " + buildingLevels);
			GameObject test1 = (GameObject)Instantiate(midPart.gameObject,  currentPosition, center_TOP.First().gameObject.transform.rotation);
			test1.transform.localScale *= scalingFactor;
			test1.transform.position += test1.transform.forward*scalingFactor;
			currentPosition = test1.transform.position;
			test1.transform.Translate(test1.transform.up*(scalingFactor*0.5f));
			test1.transform.Translate(test1.transform.up*(scalingFactor*(currentLevel-lastLevel)*0.5f));
			tempTrans = test1.transform;
		}
		currentPosition = tempTrans.transform.position + tempTrans.transform.forward*scalingFactor;
		lastLevel = currentLevel;
	}



	int ChooseRandomLength(){
		int length = Random.Range(5,15);
		return length;
	}


	// Update is called once per frame
	void Update () {
	 

		if (Input.GetKeyDown(KeyCode.S)){
			ScaleRotTranslateObject.transform.localScale += new Vector3(1,1,1);
		}


		if (Input.GetKeyDown(KeyCode.R)){
			ScaleRotTranslateObject.transform.Rotate(new Vector3(0,90,0));
		}

		if (Input.GetKeyDown(KeyCode.T)){
			ScaleRotTranslateObject.transform.Translate(1,0,0);
		}

		


		if (Input.GetKeyUp(KeyCode.O)){
			Scene scene = SceneManager.GetActiveScene(); 
			SceneManager.LoadScene(scene.name);
		}

	}
}
