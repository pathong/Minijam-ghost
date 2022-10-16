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


    public float distance;
    public float distanceFromPlayer;


    public Vector2 MinMaxIntervalSpawnTime;

    private void Awake()
    {
        if (waveSpawn == null) waveSpawn = this;
    }

    private void Start()
    {
        StartCoroutine(IntervalSpawn());
    }



    public void NormalSpawn()
    {
        Instantiate(monsTest, GetSpawnPoint(), Quaternion.identity);
    }

    IEnumerator IntervalSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinMaxIntervalSpawnTime.x, MinMaxIntervalSpawnTime.y));
            NormalSpawn();
        }
    }

    public Vector2 GetSpawnPoint()
    {
        while (true)
        {
            player =  Extension.GetPlayer();

            float x = Random.Range(-distance, distance);
            float y = Random.Range(-distance, distance);

            Vector2 pos = (Vector2)player.position + new Vector2(x, y);
            Vector3Int loc = groundTilemap.WorldToCell(pos);
            if (groundTilemap.GetTile(loc) && Vector2.Distance(player.position, pos) >= distanceFromPlayer)
            {
                return pos;
            }
        }
            
    }


    public static void SpawnWave(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            waveSpawn.NormalSpawn();
        }
        
    }

    [ContextMenu("SpawnInWave")]
    public void SpawnWaveTest()
    {
        SpawnWave(5);

    }



    






}
