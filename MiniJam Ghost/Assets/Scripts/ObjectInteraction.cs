using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class ObjectInteraction : MonoBehaviour
{

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public abstract void Interact();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerInteraction>().OpenInteractionIcon();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerInteraction>().CloseInteractionIcon();
    }
}
