using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magazine", menuName = "ScriptableObject/Magazine")]
public class MagazineSO : ScriptableObject 
{
    public BulletSO[] magazine;
    public BulletSO emptyBullet;
    [SerializeField] private BulletSO NormalBullet;
    [SerializeField] private BulletSO FireBullet;
    [SerializeField] private BulletSO LightBullet;
    public int capacity;


    [ContextMenu("Reset Magazine")]
    public void ResetMagazine()
    {
        magazine = new BulletSO[capacity];

        for (int i = 0; i < capacity; i++)
        {
            magazine[i] = emptyBullet;
        }
    }

    public int GetEmptyIndex()
    {
        for (int i = 0; i < capacity; i++)
        {
            if (magazine[i].Type == BulletType.Empty)
            {
                return i;
            }

        }
        return -1;
    }


    public int GetFirstBullet()
    {
        for (int i = 0; i < capacity; i++)
        {
            if (magazine[i].Type != BulletType.Empty)
            {
                return i;
            }

        }
        return -1;
    }


    public void AddBullet(BulletSO bullet)
    {
        int index = GetEmptyIndex();
        if (index == -1) { return; }

        magazine[index] = bullet;
    }

    public void RemoveBullet(int index)
    {
        magazine[index] = emptyBullet;

    }

    public GameObject GetandShoot()
    {
        int index = GetFirstBullet();
        if(index == -1) { return null; }
        GameObject bulletToShoot = magazine[index].Prefab;
        RemoveBullet(index);
        return bulletToShoot;

    }


    [ContextMenu("Add Normal")]
    public void AddNormal()
    {
        for (int i = 0; i < capacity; i++)
        {
            AddBullet(NormalBullet);
        }
    }
    [ContextMenu("Add Fire")]
    public void AddFire()
    {
        for (int i = 0; i < capacity; i++)
        {
            AddBullet(FireBullet);
        }
    }
    [ContextMenu("Add Light")]
    public void Light()
    {
        for (int i = 0; i < capacity; i++)
        {
            AddBullet(LightBullet);
        }
    }

}


