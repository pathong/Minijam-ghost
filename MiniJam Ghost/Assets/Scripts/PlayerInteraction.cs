using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject interactionIcon;

    [SerializeField] private LayerMask objectLayer;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            CheckInteraction();
    }

    public void OpenInteractionIcon()
    {
        interactionIcon.SetActive(true);
    }

    public void CloseInteractionIcon()
    {
        interactionIcon.SetActive(false);
    }

    private void CheckInteraction()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1, objectLayer);
        foreach (var col in hits)
        {
            if (col.transform.GetComponent<ObjectInteraction>())
            {
                col.transform.GetComponent<ObjectInteraction>().Interact();
                return;
            }
        }
    }
}
