using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectBehavior : ObjectInteraction
{
    public Sprite active;
    public Sprite inactive;

    private SpriteRenderer sr;
    private bool isActive;
    private GameObject playerCharacter;
    public bool interactIcon = true;

    [SerializeField] private bool OnOff;
    [SerializeField] private bool OnOnly;
    [SerializeField] private bool Sequence;
    [SerializeField] private GameObject[] visits;

    public override void Interact()
    {
        if (OnOff)
        {
            if (isActive)
                sr.sprite = inactive;
            else
                sr.sprite = active;
            isActive = !isActive;
        }
        else if (OnOnly & interactIcon)
        {
            if (!isActive)
                sr.sprite = active;
            isActive = !isActive;
            playerCharacter.GetComponent<PlayerInteraction>().CloseInteractionIcon();
            interactIcon = !interactIcon;
        }
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = inactive;
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
    }
}
