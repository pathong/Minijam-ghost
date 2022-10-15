using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInventory : MonoBehaviour
{


    [SerializeField] private int amountNormalBullet;
    [SerializeField] private int amountFireBullet;
    [SerializeField] private int amountLightBullet;

    private void Awake()
    {
        amountFireBullet = 0;
        amountNormalBullet = 0;
        amountLightBullet = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            ItemOnGround item =  collision.GetComponent<ItemOnGround>();
            switch (item.BulletType)
            {
                case (BulletType.Normal):
                    amountNormalBullet++;
                    break;
                case (BulletType.Fire):
                    amountLightBullet++;
                    break;
                case (BulletType.Light):
                    amountFireBullet++;
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
