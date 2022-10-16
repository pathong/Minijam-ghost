using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagazineUI : MonoBehaviour
{

    [SerializeField] private Image borderImg;
    [SerializeField] private MagazineSO so;
    [SerializeField] private List<Image> slots;
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject rToReload;

    private void Awake()
    {
        InitializeSlot();
        SetSlot();
    }

    private void Update()
    {
        SetSlot();
        if(so.GetFirstBullet() == -1)
        {
            rToReload.SetActive(true);
        }
        else
        {
            rToReload.SetActive(false);
        }
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
            img.sprite = so.emptyBullet.sprite;
        }
    }

    public void SetSlot()
    {
        for (int i = 0; i < so.capacity; i++)
        {
            slots[i].sprite = so.magazine[i].sprite; 
        }
    }




}
