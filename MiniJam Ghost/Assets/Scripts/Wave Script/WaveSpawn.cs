using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaveSpawn : MonoBehaviour
{
    static WaveSpawn waveSpawn;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private GameObject monsTest;
    private Transform player;


    public Vector2 distance;

    private void Awake()
    {
        if (waveSpawn == null) waveSpawn = this;
    }

    private void Start()
    {

        NormalSpawn();
    }



    public void NormalSpawn()
    {
        Instantiate(monsTest, GetSpawnPoint(), Quaternion.identity);
        
    }



    public Vector2 GetSpawnPoint()
    {
        player =  Extension.GetPlayer();

        float x = Random.Range(distance.x, distance.y);
        float y = Random.Range(distance.x, distance.y);

        Vector2 pos = (Vector2)player.position + new Vector2(x, y);
        if(groundTilemap.GetTile((Vector3Int)Vector2Int.RoundToInt(pos)) == null)
        {
            Debug.Log("null");
            GetSpawnPoint();
        }


        return pos;



            
    }



    






}
