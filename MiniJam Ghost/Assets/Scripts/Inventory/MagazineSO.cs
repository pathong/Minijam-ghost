using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magazine", menuName = "ScriptableObject/Magazine")]
public class MagazineSO : ScriptableObject 
{
    public BulletType[] magazine;
    public int capacity;


    [ContextMenu("Reset Magazine")]
    public void ResetMagazine()
    {
        magazine = new BulletType[capacity];

        for (int i = 0; i < capacity; i++)
        {
            magazine[i] = BulletType.Empty;
        }
    }

    public int GetEmptyIndex()
    {
        for (int i = 0; i < capacity; i++)
        {
            if (magazine[i] == BulletType.Empty)
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
            if (magazine[i] != BulletType.Empty)
            {
                return i;
            }

        }
        return -1;
    }


    public void AddBullet(BulletType bullet)
    {
        int index = GetEmptyIndex();
        if (index == -1) { return; }

        magazine[index] = bullet;
    }

    public void RemoveBullet(int index)
    {
        magazine[index] = BulletType.Empty;

    }
}
