using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject interactionIcon;

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
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(0.1f, 1f), 0, Vector2.zero);

        if(hits.Length > 0)
        {
            foreach (RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<ObjectInteraction>())
                {
                    rc.transform.GetComponent<ObjectInteraction>().Interact();
                    return;
                }
            }
        }
    }
}
