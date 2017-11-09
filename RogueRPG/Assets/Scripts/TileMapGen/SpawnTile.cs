using UnityEngine;

namespace Board
{
    public class SpawnTile : Tile
    {
        public GameObject toSpawn;

        public GameObject Spawn()
        {
            GameObject spawnInstance = Instantiate(toSpawn, transform.position, Quaternion.identity);
            return spawnInstance;
        }
    }
}