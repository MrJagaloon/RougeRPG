using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileMapLib.TileMaps.Generators;

public class TileMapTester : MonoBehaviour
{
    ITileMapGenerator generator;

    public string seed;

	void Start ()
    {
        generator = GetComponent<ITileMapGenerator>();

        if (string.IsNullOrEmpty(seed))
            seed = System.DateTime.Now.ToString();
        
        generator.Generate(seed.GetHashCode());
	}
}
