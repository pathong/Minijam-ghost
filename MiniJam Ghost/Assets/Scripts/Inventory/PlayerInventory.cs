using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInventory : MonoBehaviour
{


    [SerializeField] private BulletSO normalBullet;
    [SerializeField] private BulletSO fireBullet;
    [SerializeField] private BulletSO lightBullet;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ProgressManager.i.IncreaseProgress();
        //WaveSpawn.SpawnNormal();
        if(collision.tag == "Item")
        {
            ItemOnGround item =  collision.GetComponent<ItemOnGround>();
            switch (item.BulletType)
            {
                case (BulletType.Normal):
                    normalBullet.CurrentAmount++;
                    break;
                case (BulletType.Fire):
                    fireBullet.CurrentAmount++;
                    break;
                case (BulletType.Light):
                    lightBullet.CurrentAmount++;
                    break;
            }

            Destroy(collision.gameObject);
        }
    }



}
public enum BulletType
{
    Normal, Fire, Light, Empty
}
