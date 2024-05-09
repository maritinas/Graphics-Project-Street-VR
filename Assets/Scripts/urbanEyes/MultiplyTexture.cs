using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplyTexture : MonoBehaviour {


	public Texture2D dirtTexture;
	public Texture2D[] perlinNoise;
	Texture2D matMainText;
	Renderer rend;
	public Texture2D occlusionMap;
	public Texture2D grungeMap;
	public float dirtOpacity = 1f;
	public float dirtThreshold = 1f;
	// Use this for initialization
	public float maskThreshold = 0.98f;
	public Color dirtColor;


	public void ApplyPerlin(){

		rend.sharedMaterial.SetTexture("_DetailAlbedoMap", perlinNoise[Random.Range(0, perlinNoise.Length)]);
		rend.sharedMaterial.EnableKeyword("_DETAIL_MULX2");

	}

	public void GrungeTexture(){

		Renderer[] renders = Object.FindObjectsOfType<Renderer>();
	//	Debug.Log("Number of renderers: " + renders.Length);
		List<Renderer> rendList = new List<Renderer>();
	
		for (int r = 0; r < renders.Length; r++){
			if (rendList.Exists(x => x.sharedMaterial.name == renders[r].sharedMaterial.name)){


			}
			else {

				if (renders[r].gameObject.tag == "Wall"){
					rendList.Add(renders[r]);
				}
				//Debug.Log("Added Material: " + renders[r].sharedMaterial.name);
			}

		}

		for (int m = 0; m < rendList.Count; m++){
			Debug.Log("Main walls; " + rendList.Count);
			rend = rendList[m];
			occlusionMap = (Texture2D)rend.sharedMaterial.GetTexture("_OcclusionMap");
			//rend = GetComponent<Renderer>();

			matMainText = (Texture2D)rend.sharedMaterial.mainTexture;
			Texture2D grungeText = new Texture2D(matMainText.width, matMainText.height);
			Texture2D detailAlbedo = new Texture2D(matMainText.width, matMainText.height);

			for (int y = 0; y < matMainText.height; y++){

				for (int x = 0; x < matMainText.width; x++){

					Color grungedCol = dirtColor;

					float curGray = occlusionMap.GetPixel(x,y).grayscale;

					if (curGray > maskThreshold-0.08f){
						Color tempCol = grungeMap.GetPixel(x,y);
						grungeText.SetPixel(x,y,  dirtColor);
					}
					else {
						//grungeText.SetPixel(x,y, matMainText.GetPixel(x,y));
					}

					detailAlbedo.SetPixel(x, y, Color.black);

				}
			}
			grungeText.Apply();
			detailAlbedo.Apply();
			//rend.sharedMaterial.SetTexture("_BumpMap", grungeText);
			rend.sharedMaterial.EnableKeyword("_NORMALMAP");
			rend.sharedMaterial.SetTexture("_DetailMask", grungeText);

			rend.sharedMaterial.SetTexture("_DetailAlbedoMap", detailAlbedo);
			rend.sharedMaterial.EnableKeyword("_DETAIL_MULX2");

		}




		/*
		GameObject[] allWalls = GameObject.FindGameObjectsWithTag("Wall");

		for (int i = 0; i < allWalls.Length; i++){

			rend = allWalls[i].GetComponent<Renderer>();
			Material wallMat = rend.sharedMaterial;

			matMainText = (Texture2D)wallMat.mainTexture;
			grungeText = new Texture2D(matMainText.width, matMainText.height);
			detailAlbedo = new Texture2D(matMainText.width, matMainText.height);

			for (int y = 0; y < matMainText.height; y++){

				for (int x = 0; x < matMainText.width; x++){

					Color grungedCol = dirtColor;

					float curGray = occlusionMap.GetPixel(x,y).grayscale;

					if (curGray > maskThreshold){
						Color tempCol =  grungeMap.GetPixel(x,y);
						grungeText.SetPixel(x,y,  dirtColor);
					}
					else {
						//grungeText.SetPixel(x,y, matMainText.GetPixel(x,y));
					}

					detailAlbedo.SetPixel(x, y, Color.black);
	

				}
			}
			grungeText.Apply();
			detailAlbedo.Apply();
			//rend.sharedMaterial.SetTexture("_BumpMap", grungeText);
		
			rend.sharedMaterial.EnableKeyword("_NORMALMAP");
			rend.sharedMaterial.SetTexture("_DetailMask", grungeText);

			rend.sharedMaterial.SetTexture("_DetailAlbedoMap", detailAlbedo);
			rend.sharedMaterial.EnableKeyword("_DETAIL_MULX2");

			rend.sharedMaterial.SetTexture("_DetailAlbedoMap", perlinNoise[Random.Range(0, perlinNoise.Length)]);
			rend.sharedMaterial.EnableKeyword("_DETAIL_MULX2");
		}*/
		//rend.sharedMaterial.SetTexture("_MainTex", grungeText);



		rendList.Clear();
	}

	public void Multiply(){

		rend = GetComponent<Renderer>();
		matMainText = (Texture2D)rend.sharedMaterial.mainTexture;
	
		Texture2D multipliedTexture = new Texture2D(matMainText.width, matMainText.height);

		for (int y = 0; y < matMainText.height; y++){

			for (int x = 0; x < matMainText.width; x++){

				Color dirtPix = dirtTexture.GetPixel(x,y);
				Color mainPix = matMainText.GetPixel(x,y);
				Color multiplied = mainPix * dirtPix;

				Color difference = (multiplied - mainPix);
				Color newColor = mainPix + (difference * dirtOpacity);

				multipliedTexture.SetPixel(x,y, newColor);

			}
		}
					
		multipliedTexture.Apply();
		rend.sharedMaterial.SetTexture("_MainTex", multipliedTexture);

	}


	public void LinearBurn(){

		rend = GetComponent<Renderer>();
		matMainText = (Texture2D)rend.sharedMaterial.mainTexture;

		Debug.Log("Texture size; " + matMainText.width + ", " + matMainText.height);

		Texture2D multipliedTexture = new Texture2D(matMainText.width, matMainText.height);

		for (int y = 0; y < matMainText.height; y++){

			for (int x = 0; x < matMainText.width; x++){

				Color dirtPix = dirtTexture.GetPixel(x,y);
				Color mainPix = matMainText.GetPixel(x,y);

				Color difference = dirtPix - mainPix;
				difference = new Color(Mathf.Abs(difference.r), Mathf.Abs(difference.g), Mathf.Abs(difference.b), Mathf.Abs(difference.a));
				Color linear = (difference * dirtOpacity) + mainPix - (new Color(1,1,1,0) * dirtOpacity);
				multipliedTexture.SetPixel(x,y, linear);

			}
		}

		multipliedTexture.Apply();
		rend.sharedMaterial.SetTexture("_MainTex", multipliedTexture);

	}

	public void ThresholdDirt(){



		for (int y = 0; y < matMainText.height; y++){

			for (int x = 0; x < matMainText.width; x++){

				if (dirtTexture.GetPixel(x,y).grayscale < dirtThreshold){

					dirtTexture.SetPixel(x,y, Color.black);
				}

			}
		}
		dirtTexture.Apply();

	}



	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
