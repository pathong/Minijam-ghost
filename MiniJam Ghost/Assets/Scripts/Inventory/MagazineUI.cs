using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagazineUI : MonoBehaviour
{

    [SerializeField] private Sprite border;
    [SerializeField] private Sprite fire;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite _light;

    [SerializeField] private Image borderImg;
    [SerializeField] private MagazineSO so;
    [SerializeField] private List<Image> slots;
    [SerializeField] private Transform slotParent;

    private void Awake()
    {
        InitializeSlot();
        SetSlot();
    }





    public void InitializeSlot()
    {
        foreach (var slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();

        for (int i = 0; i < so.capacity; i++)
        {
            Image img =  Instantiate(borderImg);
            slots.Add(img);
            img.transform.SetParent(slotParent, false);
            img.sprite = border;
        }
    }

    public void SetSlot()
    {
        for (int i = 0; i < so.capacity; i++)
        {
            slots[i].sprite = GetSprite(so.magazine[i]); 
        }
    }

    public Sprite GetSprite(BulletType type)
    {
        switch (type)
        {
            case (BulletType.Empty):
                return border;
            case (BulletType.Normal):
                return normal;
            case (BulletType.Fire):
                return fire;
            case (BulletType.Light):
                return _light;
        }
        return border;
            

    }



}
