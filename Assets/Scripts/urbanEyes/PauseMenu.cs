using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	[Range(0.8f,1f)]
	public float occlusionBrightThreshold = 0.8f;
	public bool autoMove = false;
	public float timeScale = 1f;
	public Canvas pauseMenu;
	public Button generateButton;
	public Button brokenWindows;
	public Button graffitiButton;
	public Button buildingDirt;
	public Button greenery;
	public Button exitButton;

	MultiplyTexture textureScript;

	Slider[] mouseSlider;

	public Slider dirtSlider;
	public Slider windowSlider;
	public Slider greenerySlider;
	public Slider graffitiSlider;


	float timeSinceStart;
	float timer;
	bool timerBool = false;

	List<Button> buttonList = new List<Button>();
	ColorBlock originalColors;
	ColorBlock flippedColors;
	int activeButton;

	string ActiveChoice;

	MultiplyTexture mt;

	Text windowPercentage;
	Text dirtPercentage;
	Text greeneryPercentage;
	Text graffitiPercentage;

	List<GameObject> windowGOList = new List<GameObject>();
	GameObject[] windowGO;
	public Texture[] windowCrackTextures;
	public Texture[] dirtTextures;
	public Texture[] lowDirt; 
	public Texture[] graffitiTextures;

	GameObject[] lowGO;
	List<GameObject> lowGOList = new List<GameObject>();		 // For graffiti - adding on top of dirt

	GameObject[] wallGO;
	List<GameObject> wallGOList = new List<GameObject>();

	public GameObject[] trees;
	Vector3[] treePositions;
	List<Vector3> treePositionsList = new List<Vector3>();

	public GameObject[] bushes;
	Vector3[] bushPositions;
	List<Vector3> bushPositionsList = new List<Vector3>();

	public Texture2D grungeMap;
	public Texture2D perlin;

	public Material[] glassMaterials;
	// Use this for initialization
	void Start () {
		GetComponent<Canvas>().enabled = true;
		textureScript = GameObject.Find("TextureGO").GetComponent<MultiplyTexture>();

		//////// Add Treepositions //////////
		GameObject[] treeTemp;
		treeTemp = GameObject.FindGameObjectsWithTag("TreePosition");
		treePositions = new Vector3[treeTemp.Length];
		for (int i = 0; i < treeTemp.Length; i++){
			treePositions[i] = treeTemp[i].transform.position;
			treePositionsList.Add(treeTemp[i].transform.position);
		}
		//////// Add Treepositions //////////

		/////// Add bushpositions //////////

		GameObject[] bushTemp;
		bushTemp = GameObject.FindGameObjectsWithTag("BushPosition");
		bushPositions = new Vector3[bushTemp.Length];
		for (int i = 0; i < bushTemp.Length; i++){
			bushPositions[i] = bushTemp[i].transform.position;
			bushPositionsList.Add(bushTemp[i].transform.position);
		}

		/////// Add bushpositions //////////



		windowGO = GameObject.FindGameObjectsWithTag("Window");

		for (int w = 0; w < windowGO.Length; w++){
			windowGOList.Add(windowGO[w]);
		}

		wallGO = GameObject.FindGameObjectsWithTag("Wall");

		for (int w = 0; w < wallGO.Length; w++){
			wallGOList.Add(wallGO[w]);
		}

		lowGO = GameObject.FindGameObjectsWithTag("WallLow");
		for (int w = 0; w < lowGO.Length; w++){
			lowGOList.Add(lowGO[w]);
		}

		windowPercentage = transform.Find("BWNum").GetComponent<Text>();
		dirtPercentage = transform.Find("DirtNum").GetComponent<Text>();
		greeneryPercentage = transform.Find("GreeneryNum").GetComponent<Text>();
		graffitiPercentage = transform.Find("GraffitiNum").GetComponent<Text>();
			
		originalColors = buildingDirt.colors;
		flippedColors = buildingDirt.colors;
		flippedColors.normalColor = buildingDirt.colors.highlightedColor;
		flippedColors.highlightedColor = buildingDirt.colors.normalColor;

		mouseSlider = transform.GetComponentsInChildren<Slider>();

		windowSlider = mouseSlider[0];
		graffitiSlider = mouseSlider[1];
		dirtSlider = mouseSlider[2];
		greenerySlider = mouseSlider[3];
	


		buttonList.Add(generateButton);	// 0
		buttonList.Add(brokenWindows);	// 1
		buttonList.Add(graffitiButton); // 2
		buttonList.Add(buildingDirt);	// 3
		buttonList.Add(greenery);		// 4
		buttonList.Add(exitButton);		// 5


		timeSinceStart = Time.time;
		pauseMenu = GetComponent<Canvas>();
	
		mt = GameObject.Find("PauseMenu").GetComponent<MultiplyTexture>();



	}


	void ShowMenu(){
		pauseMenu.enabled = true;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;

		timer = Time.time - timeSinceStart;
		timerBool = true;
		activeButton = 1;
	}



	/*void Perlin(Renderer rend, Texture2D perlinMap){
		rend.sharedMaterial.SetTexture("_DetailAlbedoMap", perlinMap);
		rend.sharedMaterial.EnableKeyword("_DETAIL_MULX2");
	}	*/







	void Update(){
		if (Input.GetKeyDown(KeyCode.M)){

			int i = Random.Range(0,3);
		//	Debug.Log("Random range(0,3): " + i);

		}

		if (Input.GetKeyDown(KeyCode.Escape)){
			if (!pauseMenu.enabled){
				buttonList[activeButton].colors = flippedColors;

				ShowMenu();
				pauseMenu.planeDistance = 0.5f;
			
			}
			else if (pauseMenu.enabled){

				HideMenu();

			}
		}

		if (Input.GetKeyDown(KeyCode.R)){
			RestartLevel();	
		}

		if (timerBool){
			
			ContinuousRise();
		}
		timeSinceStart = Time.time;

	
		if (pauseMenu.enabled){

			if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0){

				if (Input.GetKeyDown(KeyCode.DownArrow)){
					buttonList[activeButton].colors = originalColors;
					activeButton = (activeButton + 1) % buttonList.Count;
					buttonList[activeButton].colors = flippedColors;

				}
				if (Input.GetKeyDown(KeyCode.UpArrow)){
					buttonList[activeButton].colors = originalColors;
					activeButton = Modulo(activeButton-1, buttonList.Count);//(activeButton - 1) % buttonList.Count;
					buttonList[activeButton].colors = flippedColors;

				}

			}

			if (Input.GetKeyDown(KeyCode.Return)){

				if (activeButton == 0){			// GENERATE //


	/////////////// Change Window Textures ////////////

					int windowsThatWillChange =  (int)(windowGO.Length * (windowSlider.value));

					//Debug.Log("WindowArray length: " + windowGO.Length + ", windowList: " + windowGOList.Count + "windowchanges: " + windowsThatWillChange);
					for (int w = 0; w < windowsThatWillChange; w++){			

						int changeWindowIndex = Random.Range(0, windowGOList.Count);
					
						Renderer rend = windowGOList[changeWindowIndex].GetComponent<Renderer>();
						
						Material mat = glassMaterials[Random.Range(0, glassMaterials.Length)];
						int crackIndex = Random.Range(0, windowCrackTextures.Length);


					Material[] matt = rend.materials;
					for (int z = 0; z < matt.Length; z++){
					//	Debug.Log("material name: " + matt[z].name);	
					}

					//Debug.Log("rend.materials[0].name" + rend.materials[0].name);
					//Debug.Log("rend.materials[1].name" + rend.materials[1].name);
					rend.materials[1] = mat;
					rend.materials[1].SetTexture("_DetailMask", windowCrackTextures[crackIndex]);
					rend.materials[1].SetTexture("_DetailAlbedoMap", windowCrackTextures[crackIndex]);
					rend.materials[1].EnableKeyword("_DETAIL_MULX2");

						windowGOList.RemoveAt(changeWindowIndex);
					}

	/////////////// Change Window Textures ////////////


	///////// Add DIRT LOW  ////////

				Debug.Log("Tot lowWalls: " + lowGO.Length);

				for (int d = 0; d < lowGO.Length; d++){

					Renderer lowRend = lowGO[d].GetComponent<Renderer>();

					Material matt = lowRend.sharedMaterial;
					int randomLowDirt = Random.Range(0, lowDirt.Length);
				
					Texture2D dirtOcclusioned = new Texture2D(lowDirt[randomLowDirt].width, lowDirt[randomLowDirt].height);
					//Debug.Log("asdfas");
					Texture2D occMapp = (Texture2D)matt.GetTexture("_OcclusionMap");
					//Debug.Log("occ name: " + occMap.name);
					Texture2D activeDirt = (Texture2D)lowDirt[randomLowDirt];

					for (int x = 0; x < occMapp.width; x++){

						for (int y = 0; y < occMapp.height; y++){

							if (occMapp.GetPixel(x,y).grayscale > occlusionBrightThreshold){

								dirtOcclusioned.SetPixel(x,y, Color.clear);

							}

							else {
								dirtOcclusioned.SetPixel(x,y, activeDirt.GetPixel(x,y));
							}

						}

					}
					//dirtOcclusioned.alphaIsTransparency = true;
					dirtOcclusioned.name = "DirtyOcclusion";
					dirtOcclusioned.Apply();

					lowRend.material = matt;
					lowRend.material.SetTexture("_DetailMask", dirtOcclusioned);
					lowRend.material.SetTexture("_DetailAlbedoMap", dirtOcclusioned);
					lowRend.material.EnableKeyword("_DETAIL_MULX2");

				}

	////// ADD DIRT LOW 

	////// ADD GRAFFITI 

				int lowWallsWithGraffiti = (int) (lowGO.Length* graffitiSlider.value);
				

				for (int g = 0; g < lowWallsWithGraffiti; g++){

					int lowWallThatWillGetGraffitiIndex = Random.Range(0, lowGOList.Count);

					Texture2D randomGraffiti = (Texture2D)graffitiTextures[Random.Range(0, graffitiTextures.Length)];

					Renderer lowWallRend = lowGOList[lowWallThatWillGetGraffitiIndex].GetComponent<Renderer>();
					Material lowWallMatBeforeGraffitMat = lowWallRend.material;

					Texture2D detailTexture = (Texture2D)lowWallRend.material.GetTexture("_DetailMask");
					
					for (int x = 0; x < detailTexture.width; x++){

						for (int y = 0; y < detailTexture.height; y++){

							if (randomGraffiti.GetPixel(x,y).a > 0.1f){			// om graffitin inte är genomskinlig där, lägg den ovanpå

								detailTexture.SetPixel(x,y, randomGraffiti.GetPixel(x,y));

							}

						}
							
					}
					//detailTexture.alphaIsTransparency = true;
					detailTexture.Apply();

					lowWallRend.material = lowWallMatBeforeGraffitMat;
					lowWallRend.material.SetTexture("_DetailMask", detailTexture);
					lowWallRend.material.SetTexture("_DetailAlbedoMap", detailTexture);
					lowWallRend.material.EnableKeyword("_DETAIL_MULX2");
					lowGOList.RemoveAt(lowWallThatWillGetGraffitiIndex);
				}



	////// ADD GRAFFITI



	/////////////// Place Trees ////////////
					int treesThatWillBePlaced =  (int)(treePositions.Length * (greenerySlider.value));
				//	Debug.Log("TreeArray length: " + trees.Length + ", treepositions list: " + treePositionsList.Count + ", treeplacements: " + treesThatWillBePlaced);
					for (int t = 0; t < treesThatWillBePlaced; t++){
						int placeTreeIndex = Random.Range(0, treePositionsList.Count);

						Instantiate(trees[Random.Range(0, trees.Length)], treePositionsList[placeTreeIndex], Quaternion.identity);

						treePositionsList.RemoveAt(placeTreeIndex);
					}
	/////////////// Place Trees ////////////




	/////////////// Place Bushes ////////////
				int bushesThatWillBePlaced =  (int)(bushPositions.Length * (greenerySlider.value));
				for (int t = 0; t < bushesThatWillBePlaced; t++){
					int placeBushIndex = Random.Range(0, bushPositionsList.Count);
					//GameObject bush = (GameObject)Instantiate(bushes[Random.Range(0, bushes.Length)], bushPositionsList[placeBushIndex], Quaternion.identity);
					int bushIndex = Random.Range(0, bushes.Length);
					GameObject bush = (GameObject)Instantiate(bushes[bushIndex], bushPositionsList[placeBushIndex], Quaternion.identity);
					bush.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					bush.transform.Translate(new Vector3(0f, -0.12f, 0f ));
					bushPositionsList.RemoveAt(placeBushIndex);

				}
	/////////////// Place Bushes ////////////



	/////////////// ADD MAIN DIRT ////////////
					int wallsThatWillChange =  (int)(wallGO.Length * Mathf.Round(((dirtSlider.value*100) - 50)*2)*0.01); //Mathf.Round(((dirtSlider.value*100) - 50)*2)/100);
				//	Debug.Log("WallArray length: " + wallGO.Length + ", wallList: " + wallGOList.Count + "wallChanges: " + wallsThatWillChange + ", when percentage was: " + dirtSlider.value);
				//	Debug.Log("dirtslider value: " + dirtSlider.value);
					//for (int w = 0; w < wallGOList.Count; w++){
					textureScript.maskThreshold = dirtSlider.value;//0.96f;
					textureScript.GrungeTexture();
						//Grunge(wallGOList[1], dirtSlider.value); 
							
					//}


					/*for (int w = 0; w < wallsThatWillChange; w++){			

						int changeWallIndex = Random.Range(0, wallGOList.Count);

						Renderer rend = wallGOList[changeWallIndex].GetComponent<Renderer>();
						Material mat = rend.sharedMaterial;

						int dirtIndex = Random.Range(0, dirtTextures.Length);

						rend.material.SetTexture("_DetailAlbedoMap", dirtTextures[dirtIndex]);
						rend.sharedMaterial.SetTexture("_DetailMask", dirtTextures[dirtIndex]);
						rend.sharedMaterial.EnableKeyword("_DETAIL_MULX2");

						wallGOList.RemoveAt(changeWallIndex);
					}*/
	/////////////// ADD MAIN DIRT ////////////



				HideMenu();

				if (autoMove){
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveOnPath>().enabled = true;
				}

				Time.timeScale = timeScale;

				}

				//else if (activeButton == 4){
				//	RestartLevel();				}

				else if (activeButton == 6){
					ExitGame();
				}
			}

			if (activeButton == 1){

				if (Input.GetKeyDown(KeyCode.LeftArrow)){
					
					windowSlider.value = (float)System.Math.Round((windowSlider.value - 0.1f), 2);
					windowPercentage.text = (windowSlider.value * 100) + " %";

				}
				if (Input.GetKeyDown(KeyCode.RightArrow)){

					windowSlider.value = (float)System.Math.Round((windowSlider.value + 0.1f), 2);
					windowPercentage.text = (windowSlider.value * 100) + " %";
			
				}

			}


		if (activeButton == 2){

			if (Input.GetKeyDown(KeyCode.LeftArrow)){

				graffitiSlider.value = (float)System.Math.Round((graffitiSlider.value - 0.1f), 2);
				graffitiPercentage.text = (graffitiSlider.value * 100) + " %";

			}
			if (Input.GetKeyDown(KeyCode.RightArrow)){

				graffitiSlider.value = (float)System.Math.Round((graffitiSlider.value + 0.1f), 2);
				graffitiPercentage.text = (graffitiSlider.value * 100) + " %";

			}

		}
			

			if (activeButton == 3){

				if (Input.GetKeyDown(KeyCode.LeftArrow)){
			
					dirtSlider.value = (float)System.Math.Round((dirtSlider.value - 0.05f), 2);
					//dirtSlider.value = (float)System.Math.Round((dirtSlider.value - 0.1f), 2);
					//dirtPercentage.text = (dirtSlider.value * 100) + " %";
					dirtPercentage.text = Mathf.Round(((dirtSlider.value*100) - 50)*2) + " %";
					//	Debug.Log("Dirtslider value: " + dirtSlider.value + ", text: " + Mathf.Round(((dirtSlider.value*100) - 50)*2));
					float dirtsliderPercent = Mathf.Round(((dirtSlider.value*100) - 50)*2) * 0.01f;	
					occlusionBrightThreshold = 0.8f + (1f-0.8f)*dirtsliderPercent;
					
				}
				if (Input.GetKeyDown(KeyCode.RightArrow)){

					dirtSlider.value = (float)System.Math.Round((dirtSlider.value + 0.05f), 2);
					//dirtSlider.value = (float)System.Math.Round((dirtSlider.value - 0.1f), 2);
					//dirtPercentage.text = (dirtSlider.value * 100) + " %";
					dirtPercentage.text = Mathf.Round(((dirtSlider.value*100) - 50)*2) + " %";
					//Debug.Log("Dirtslider value: " + dirtSlider.value  + ", text: " +  Mathf.Round(((dirtSlider.value*100) - 50)*2));
					float dirtsliderPercent = Mathf.Round(((dirtSlider.value*100) - 50)*2) * 0.01f;
					occlusionBrightThreshold = 0.8f + (1f-0.8f)*dirtsliderPercent;
					Debug.Log("occ threshold: " + occlusionBrightThreshold);

				}

			}

			if (activeButton == 4){

				if (Input.GetKeyDown(KeyCode.LeftArrow)){

					greenerySlider.value = (float)System.Math.Round((greenerySlider.value - 0.1f), 2);
					greeneryPercentage.text = (greenerySlider.value * 100) + " %";
				

				}
				if (Input.GetKeyDown(KeyCode.RightArrow)){
					
					greenerySlider.value = (float)System.Math.Round((greenerySlider.value + 0.1f), 2);
					greeneryPercentage.text = (greenerySlider.value * 100) + " %";
			
				}

			}


			if (activeButton == 5){

			}
				

			if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
				for (int c = 0; c < buttonList.Count; c++){
					buttonList[c].colors = originalColors;
				}
			}


		}

	}

	void HideMenu(){
		pauseMenu.enabled = false;
		ActiveChoice = "";
		Cursor.visible = false;
		//transform.GetComponentInParent<BlenderControl>().enabled = true;

		Cursor.lockState = CursorLockMode.Locked;
	
		timerBool = false;
	}


	int Modulo(int a, int b){
		return ((a %= b) < 0) ? a+b : a;
	}
	
	void ContinuousRise(){


		if (timer < 10f){
			int lastSec = (int)timer;
			timer += Time.deltaTime;
			if (lastSec < (int)timer){	// Gör nåt varje sekund

			}
				
		}

	}

	public void ResumePress(){
		
		pauseMenu.enabled = false;
		Cursor.visible = false;
		//transform.GetComponentInParent<BlenderControl>().enabled = true;
		Cursor.lockState = CursorLockMode.Locked;

	}

	public void RestartLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}

	public void ExitGame(){
		pauseMenu.enabled = false;
		Debug.Log("Exited Game!");
		Application.Quit();
	}





}
