using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileMapLib;
using TileMapLib.Generators;

public class TileMapTester : MonoBehaviour
{
    public int rows;
    public int cols;

    public GameObject[] emptyTileSet;
    public GameObject[] filledTileSet;

    RandomFillGeneratorStep generatorStep;

	void Start ()
    {
        generatorStep = new RandomFillGeneratorStep(emptyTileSet, filledTileSet);

        bool[][] fillMap = WhiteNoise.GenerateBitMap(rows, cols);

        string s = "Generated white noise:\n";
        for (int x = 0; x < cols; ++x)
        {
            for (int y = 0; y < rows; ++y)
            {
                s += fillMap[x][y] ? "1" : "0";
            }
            s += "\n";
        }
        Debug.Log(s);

        TileMap map = new TileMap(rows, cols);

        map.baseMaps.Add(fillMap);

        generatorStep.ProcessTileMap(map, null, 0);
	}
}
