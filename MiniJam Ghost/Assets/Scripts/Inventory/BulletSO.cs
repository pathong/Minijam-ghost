using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Bullet", menuName = "ScriptableObject/Bullet")]
public class BulletSO : ScriptableObject 
{
    public BulletType Type;
    public GameObject Prefab;
    public Sprite sprite;
    public string BulletName;
}
