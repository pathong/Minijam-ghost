using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineResetOnStart : MonoBehaviour
{
    public MagazineSO magazineSO;
    public BulletSO[] bulletSOs;

    void Start() {
        magazineSO.RemoveBullet(0);
        magazineSO.RemoveBullet(1);
        magazineSO.RemoveBullet(2);
        magazineSO.RemoveBullet(3);
        magazineSO.RemoveBullet(4);

        magazineSO.AddNormal();
        magazineSO.RemoveBullet(1);
        magazineSO.RemoveBullet(2);
        magazineSO.RemoveBullet(3);
        magazineSO.RemoveBullet(4);

        foreach (BulletSO b in bulletSOs) {
            b.CurrentAmount = 1;
        }
    }
}
