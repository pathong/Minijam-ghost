using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineResetOnStart : MonoBehaviour
{
    public MagazineSO magazineSO;
    public BulletSO[] bulletSOs;

    void Start() {
        for (int i = 0; i < magazineSO.magazine.Length; i++) {
            BulletSO b = magazineSO.magazine[i];
            b = null;
        }

        foreach (BulletSO b in bulletSOs) {
            b.CurrentAmount = 0;
        }
    }
}
