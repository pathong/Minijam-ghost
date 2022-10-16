using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInventory : MonoBehaviour
{


    [SerializeField] private BulletSO normalBullet;
    [SerializeField] private BulletSO fireBullet;
    [SerializeField] private BulletSO lightBullet;

    [SerializeField] private AudioClip collectSound;


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
                    normalBullet.CurrentAmount += 10;
                    break;
                case (BulletType.Fire):
                    fireBullet.CurrentAmount += 10;
                    break;
                case (BulletType.Light):
                    lightBullet.CurrentAmount += 10;
                    break;
            }

            Destroy(collision.gameObject);
            SoundManager.PlaySound(collectSound, this.transform.position);
        }
    }



}
public enum BulletType
{
    Normal, Fire, Light, Empty
}
