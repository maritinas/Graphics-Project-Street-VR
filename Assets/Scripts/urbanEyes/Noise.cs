using UnityEngine;
using System.Collections;

public static class Noise  {


	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset){

		float[,] noiseMap = new float[mapWidth, mapHeight];

		System.Random prng = new System.Random(seed); 
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++){
			float offsetX = prng.Next(-99999, 99999) + offset.x;
			float offsetY = prng.Next(-99999, 99999) + offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;		// to be able to normalize the values before returning the noise map
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth * 0.5f;
		float halfHeight = mapHeight * 0.5f;

		for (int y = 0; y < mapHeight; y++){
			for (int x = 0; x < mapWidth; x++){

				float amplitude = 1f;
				float frequency = 1f;
				float noiseHeight = 0f;

				for (int i = 0; i < octaves; i++){
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;		// higher frequency -> further apart sample points will be (height values change more rap)
					float sampleY = (y -halfHeight) / scale * frequency + octaveOffsets[i].y; 

					float perlinValue = Mathf.PerlinNoise( sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;	 
					frequency *= lacunarity; 
				}

				if (noiseHeight > maxNoiseHeight){
					maxNoiseHeight = noiseHeight;
				}
				else if (noiseHeight < minNoiseHeight){
					minNoiseHeight = noiseHeight;
				}
				noiseMap[x,y] = noiseHeight;

			}
		}

		for (int y = 0; y < mapHeight; y++){		// loop through the values and normalize them
			for (int x = 0; x < mapWidth; x++){
		
				noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);	// if noisemapval = min -> ret 0, if = max -> ret 1, middle -> 0.5 and all between 0 and 1

			}
		}
			
		return noiseMap;

	}


}
