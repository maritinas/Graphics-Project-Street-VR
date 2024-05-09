using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class TestPut : MonoBehaviour {


	Vector3 currentPos = new Vector3(0f,0f,0f);
	Vector3 boundsCurrent;
	Vector3 boundsLastFloor;
	Vector3 boundsLastPart;
	Vector3 boxBelow;

	GameObject part1;
	GameObject part2;
	GameObject part3;


	public class buildingBlock {
		public Transform part;
		public int apartmentIndex;
	}

	List<buildingBlock> bottomLeft = new List<buildingBlock>();
	List<buildingBlock> bottomRight = new List<buildingBlock>();
	List<buildingBlock> bottomCenter = new List<buildingBlock>();

	List<buildingBlock> middleLeft = new List<buildingBlock>();
	List<buildingBlock> middleRight = new List<buildingBlock>();
	List<buildingBlock> middleCenter = new List<buildingBlock>();

	List<buildingBlock> topLeft = new List<buildingBlock>();
	List<buildingBlock> topRight = new List<buildingBlock>();
	List<buildingBlock> topCenter = new List<buildingBlock>();

	int buildingLevels;
	int buildingWidth;

	public int buildingLevelsMin = 3;
	public int buildingLevelsMax = 5;
	public int buildingWidthMin = 3;
	public int buildingWidthMax = 5;



	GameObject firstCorner;


	public class apartmentBoxes {
		public Vector3 partBox;
		public int level;
		public int apartmentNumber;
	}
	List<apartmentBoxes> apartmentInfo = new List<apartmentBoxes>(); 

	int activeLevel = 0;
	int activeAptNr = 0;
	// Use this for initialization
	void Start () {
		

		buildingLevels = UnityEngine.Random.Range(buildingLevelsMin,buildingLevelsMax);
		buildingWidth = UnityEngine.Random.Range(buildingWidthMin,buildingWidthMax);

		AddPartsToList();

		Debug.Log(buildingLevels);


		for (int i = 0; i < buildingLevels; i++){
			CreateFloor();
			activeLevel += 1;
		}


			
	}

	void CreateFloor(){

		/*Vector3 boundsCurrent;
		Vector3 boundsLastFloor;
		Vector3 boundsLastPart;
		*/

		string levelType; 
		int partIndex;

		if (activeLevel == 0){
			levelType = "Bottom";
		}
		else if (activeLevel == buildingLevels - 1){
			levelType = "Top";
		}
		else {
			levelType = "Middle";
		}
			
	
		//////// First Corner /////////
		firstCorner = null;
		if (levelType == "Bottom"){
			partIndex = UnityEngine.Random.Range(0,bottomRight.Count);
			firstCorner = (GameObject)Instantiate(bottomRight[partIndex].part.gameObject, currentPos, Quaternion.identity);
		
		}

		else if (levelType == "Middle"){
			partIndex = UnityEngine.Random.Range(0,middleRight.Count);
			firstCorner = (GameObject)Instantiate(middleRight[partIndex].part.gameObject, currentPos, Quaternion.identity);
		}

		else if (levelType == "Top"){
			partIndex = UnityEngine.Random.Range(0,topRight.Count);
			firstCorner = (GameObject)Instantiate(topRight[partIndex].part.gameObject, currentPos, Quaternion.identity);
		}

		string partName = firstCorner.gameObject.name;
		if (partName.Contains("R")){

			firstCorner.transform.localScale = new Vector3(1,1,1);
		}
		else if (partName.Contains("L")){
			firstCorner.transform.localScale = new Vector3(-1,1,1);
		}


		boundsCurrent = CalculateBounds(firstCorner);
		currentPos = firstCorner.transform.position;




		if (levelType != "Bottom"){
			
			boxBelow = apartmentInfo.Find(a => a.apartmentNumber == 1 && a.level == activeLevel-1).partBox;
			ScaleToBounds("XZ", boxBelow, firstCorner);
		}

		boundsCurrent = CalculateBounds(firstCorner);
		firstCorner.transform.Translate(new Vector3(0, boundsCurrent.y*0.5f, 0));

		boundsCurrent = CalculateBounds(firstCorner);
		boundsLastFloor = boundsCurrent;
		boundsLastPart = boundsCurrent;
		currentPos = firstCorner.transform.position;
		currentPos += new Vector3(boundsCurrent.x*0.5f, 0, 0);


		Vector3 temp = CalculateBounds(firstCorner);
		firstCorner.gameObject.name += temp.x + ":" + temp.y + ":" + temp.z;

		AddBoundingBoxToList(firstCorner, activeLevel, activeAptNr+1);

		CreateMid(levelType);
	}
		

	void CreateMid(string levelType){

		int partIndex;
		//////// First Middle /////////
		GameObject middlePart = null;
		for (int i = 0; i < buildingWidth; i++){

			middlePart = null;
			if (levelType == "Bottom"){
				partIndex = UnityEngine.Random.Range(0,bottomCenter.Count);
				middlePart = (GameObject)Instantiate(bottomCenter[partIndex].part.gameObject, currentPos, Quaternion.identity);
			}

			else if (levelType == "Middle"){
				partIndex = UnityEngine.Random.Range(0,middleCenter.Count);
				middlePart = (GameObject)Instantiate(middleCenter[partIndex].part.gameObject, currentPos, Quaternion.identity);
			}

			else if (levelType == "Top"){
				partIndex = UnityEngine.Random.Range(0,topCenter.Count);
				middlePart = (GameObject)Instantiate(topCenter[partIndex].part.gameObject, currentPos, Quaternion.identity);
			}

											
			ScaleToBounds("YZ", boundsLastPart, middlePart);	
		
			boundsCurrent = CalculateBounds(middlePart);	
			boundsLastPart = boundsCurrent;



			Vector3 temp = CalculateBounds(middlePart);
			middlePart.gameObject.name += temp.x + ":" + temp.y + ":" + temp.z;

			if (levelType != "Bottom"){

				boxBelow = apartmentInfo.Find(a => a.apartmentNumber == apartmentInfo.Last().apartmentNumber + 1 && a.level == activeLevel-1).partBox;
				ScaleToBounds("XZ", boxBelow, middlePart);

			}

			boundsCurrent = CalculateBounds(middlePart);											///// FLOOR BOUNDS /////
			middlePart.transform.Translate(new Vector3(boundsCurrent.x/2, 0, 0));
			AddBoundingBoxToList(middlePart, activeLevel, 2+i);

			boundsCurrent = CalculateBounds(middlePart);	
			boundsLastPart = boundsCurrent;

			currentPos = middlePart.transform.position;
			currentPos += new Vector3(boundsCurrent.x*0.5f, 0 ,0);

		}
		boundsCurrent = CalculateBounds(middlePart);
		boundsLastPart = boundsCurrent;
		CreateSecondCorner(levelType);
	}


	void CreateSecondCorner(string levelType){

	
		int partIndex;
		//////// SECOND CORNER /////////

		GameObject secondCorner = null;
		if (levelType == "Bottom"){
			partIndex = UnityEngine.Random.Range(0,bottomLeft.Count);
			secondCorner = (GameObject)Instantiate(bottomLeft[partIndex].part.gameObject, currentPos, Quaternion.identity);
		}

		else if (levelType == "Middle"){
			partIndex = UnityEngine.Random.Range(0,middleLeft.Count);
			secondCorner = (GameObject)Instantiate(middleLeft[partIndex].part.gameObject, currentPos, Quaternion.identity);
		}

		else if (levelType == "Top"){
			partIndex = UnityEngine.Random.Range(0,topLeft.Count);
			secondCorner = (GameObject)Instantiate(topLeft[partIndex].part.gameObject, currentPos, Quaternion.identity);
		}

	
		string partName = secondCorner.gameObject.name;

		if (partName.Contains("R")){

			secondCorner.transform.localScale = new Vector3(-1,1,1);
		}
		else if (partName.Contains("L")){
			secondCorner.transform.localScale = new Vector3(1,1,1);
		}
		Debug.Log("Before bounds: " + CalculateBounds(secondCorner).y);
		ScaleToBounds("YZ", boundsLastPart, secondCorner);
		Debug.Log("After bounds: " + CalculateBounds(secondCorner).y);
		Debug.Log("Name: " + secondCorner.gameObject.name);
		//currentPos = firstCorner.transform.position;


		if (levelType != "Bottom"){
			boxBelow = apartmentInfo.Find(a => a.apartmentNumber == apartmentInfo.Last().apartmentNumber + 1 && a.level == activeLevel-1).partBox;
			ScaleToBounds("XZ", boxBelow, secondCorner);

		}
		boundsCurrent = CalculateBounds(secondCorner);
		secondCorner.transform.Translate(new Vector3(boundsCurrent.x*0.5f, 0,0));

		AddBoundingBoxToList(secondCorner, activeLevel, apartmentInfo.Last().apartmentNumber + 1);

		currentPos = firstCorner.transform.position + new Vector3(0, boundsLastFloor.y*0.5f, 0);

		Vector3 temp = CalculateBounds(secondCorner);
		secondCorner.gameObject.name += temp.x + ":" + temp.y + ":" + temp.z;

		activeAptNr = 0;	

	}







	void AddBoundingBoxToList(GameObject go, int floor, int aptNr){

		Vector3 tempVec = CalculateBounds(go);
		apartmentBoxes tempApt = new apartmentBoxes();
		tempApt.partBox = tempVec;
		tempApt.level = floor;
		tempApt.apartmentNumber = aptNr;

		apartmentInfo.Add(tempApt);
	} 


	void AddPartsToList(){
		//buildingBlock tempo = null;
		buildingBlock tempo = new buildingBlock();

		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Apartment2/Corner/Low", typeof(GameObject))){

			if (g.name.EndsWith("L")){
				tempo = new buildingBlock();
				tempo.part = g.transform;
				tempo.apartmentIndex = 1;

				bottomLeft.Add(tempo);
				bottomRight.Add(tempo);
	
			}
			
			else if (g.name.EndsWith("R")){
				tempo = new buildingBlock();
				tempo.part = g.transform;
				tempo.apartmentIndex = 1;

				bottomRight.Add(tempo);
				bottomLeft.Add(tempo);

			}
	
		}
			
	
		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Apartment2/Corner/Mid", typeof(GameObject))){

			if (g.name.EndsWith("L")){
				tempo = new buildingBlock();
				tempo.part = g.transform;
				tempo.apartmentIndex = 1;
				middleLeft.Add(tempo);
				middleRight.Add(tempo);

			}

			else if (g.name.EndsWith("R")){
				tempo = new buildingBlock();
				tempo.part = g.transform;
				tempo.apartmentIndex = 1;
				middleRight.Add(tempo);
				middleLeft.Add(tempo);
			}
				
		}

		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Apartment2/Corner/Top", typeof(GameObject))){

			if (g.name.EndsWith("L")){
				tempo = new buildingBlock();
				tempo.part = g.transform;
				tempo.apartmentIndex = 1;
				topLeft.Add(tempo);
				topRight.Add(tempo);

			}

			else if (g.name.EndsWith("R")){
				tempo = new buildingBlock();
				tempo.part = g.transform;
				tempo.apartmentIndex  = 1;

				topRight.Add(tempo);
				topLeft.Add(tempo);
			}

		}

		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Apartment2/Center/Low", typeof(GameObject))){
			tempo = new buildingBlock();
			tempo.part = g.transform;
			tempo.apartmentIndex = 1;
			bottomCenter.Add(tempo);

		}
	
		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Apartment2/Center/Mid", typeof(GameObject))){
			tempo = new buildingBlock();
			tempo.part = g.transform;
			tempo.apartmentIndex = 1;
			middleCenter.Add(tempo);

		}

		foreach(GameObject g in Resources.LoadAll("HousePrefabs/Apartment2/Center/Top", typeof(GameObject))){
			tempo = new buildingBlock();
			tempo.part = g.transform;
			tempo.apartmentIndex = 1;
			topCenter.Add(tempo);

		}


	}


	void ScaleToBounds(string axis, Vector3 boundsRef, GameObject objectToBeScaled){

		Vector3 currentSize = objectToBeScaled.GetComponent<Renderer>().bounds.size; 
		Vector3 scale = objectToBeScaled.transform.localScale;

		if (axis.Contains("X")){
			scale.x = boundsRef.x * scale.x / currentSize.x;
			//scale.x = (float)Math.Round(scale.x,2);
	
		}
		if (axis.Contains("Y")){
			scale.y = boundsRef.y * scale.y / currentSize.y;
			//scale.y = (float)Math.Round(scale.y,2);
		}
		if (axis.Contains("Z")){
			scale.z = boundsRef.z * scale.z / currentSize.z;

			//scale.z = (float)Math.Round(scale.z,2);

		}


		objectToBeScaled.transform.localScale = scale;
	
	}



	Vector3 CalculateBounds(GameObject blockPart){

		Renderer rend;
		rend = blockPart.GetComponent<Renderer>();
		Vector3 max = rend.bounds.max;
		Vector3 min = rend.bounds.min;
	
		//float mag = rend.bounds.extents.magnitude;
		Vector3 boundBox = max - min;
		return boundBox;

	}
		
	// Update is called once per frame
	void Update () {


		if (Input.GetKeyUp(KeyCode.R)){
			Scene scene = SceneManager.GetActiveScene(); 
			SceneManager.LoadScene(scene.name);
		}


	}
}



/* 
		 * 	Om fönster o "extrasaker" är parentade i blender
		 * hamnar de i underhierarkin, och bara föräldern räknas med 
		 * i bounds! woop yeah :) 
		 * 
		 * Anv. foreach(Bounds b in Getcomponentsinchildren<Renderer>()){
		 * bounds.Encapsulate(b.bounds) }
		 * 
		 * För att omsluta hela hierarkin 
		*/