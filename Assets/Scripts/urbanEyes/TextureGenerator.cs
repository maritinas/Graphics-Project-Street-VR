using UnityEngine;
using System.Collections;

public class TextureGenerator : MonoBehaviour {

	public int mapWidth;
	public int mapHeight; 

	[Range(1,200)]
	public float noiseScale;

	[Range(1,12)]
	public int octaves;

	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;

	public void GenerateTexture(){
		float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed,  noiseScale, octaves, persistance, lacunarity, offset);

		TextureDisplay display = FindObjectOfType<TextureDisplay>();
		display.DrawNoiseMap (noiseMap);

	}

	void OnValidate(){			// called auto whenever script variables are changed in inspector. keep from being minus

		if (mapWidth < 1){
			mapWidth = 1;
		}
		if (mapHeight < 1){
			mapHeight = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0){
			octaves = 0;
		}



	}


}
