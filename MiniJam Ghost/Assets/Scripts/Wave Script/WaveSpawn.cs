using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaveSpawn : MonoBehaviour
{
    static WaveSpawn waveSpawn;
    private Tilemap groundTilemap;

    public GameObject enemyLv1;
    public GameObject enemyLv2;

    private Transform player;


    public float distance;
    public float distanceFromPlayer;


    public Vector2 MinMaxIntervalSpawnTime;

    private void Awake()
    {
        if (waveSpawn == null) waveSpawn = this;
        groundTilemap = GameObject.FindGameObjectWithTag("GroundTileMap").GetComponent<Tilemap>();
    }




    void NormalSpawn(GameObject monst = null)
    {
        if(monst == null) { monst = enemyLv1; }
        Instantiate(monst, GetSpawnPoint(), Quaternion.identity);
    }

    IEnumerator IntervalSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinMaxIntervalSpawnTime.x, MinMaxIntervalSpawnTime.y));
            NormalSpawn();
        }
    }

    Vector2 GetSpawnPoint()
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





    /// <summary>
    /// Spawn monster in wave
    /// </summary>
    /// <param name="amount">default value for 1 wave is 5 monster</param>
    public static void SpawnWave(int amount = 5)
    {
        Debug.Log("SpawnWave");
        for (int i = 0; i < amount; i++)
        {
            waveSpawn.NormalSpawn();
        }
        
    }
     
    /// <summary>
    /// spawn monster once
    /// </summary>
    public static void SpawnNormal(GameObject monst = null)
    {
        Debug.Log("Spawn Normal");
        waveSpawn.NormalSpawn(monst);
    }
    
    /// <summary>
    /// Start Interval Spawn
    /// </summary>
    public static void StartIntervalSpawn()
    {
        Debug.Log("Start Interval");
        waveSpawn.StartCoroutine(waveSpawn.IntervalSpawn());
    }
    /// <summary>
    /// Stop Interval Spawn
    /// </summary>
    public static void StopIntervalSpawn()
    {
        Debug.Log("Stop Interval");
        waveSpawn.StopCoroutine(waveSpawn.IntervalSpawn());
    }

    
    
    [ContextMenu("Test SpawnInWave")]
    public void SpawnWaveTest()
    {
        SpawnWave();

    }



    






}
