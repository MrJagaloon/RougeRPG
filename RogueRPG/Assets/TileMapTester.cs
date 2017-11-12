using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileMapLib.TileMaps;

public class TileMapTester : MonoBehaviour
{
    RougeCaveTileMapGenerator generator;

    public string seed;

	void Start ()
    {
        generator = GetComponent<RougeCaveTileMapGenerator>();

        if (string.IsNullOrEmpty(seed))
            seed = System.DateTime.Now.ToString();
        
        generator.Generate(seed.GetHashCode());
	}
}
