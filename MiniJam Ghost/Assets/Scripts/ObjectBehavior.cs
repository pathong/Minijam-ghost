using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectBehavior : ObjectInteraction
{
    public Sprite active;
    public Sprite inactive;

    private SpriteRenderer sr;
    private bool isActive;
    private GameObject playerCharacter;
    private bool used = false;
    public bool interactIcon = true;

    [SerializeField] private bool OnOff;
    [SerializeField] private bool OnOnly;
    [SerializeField] private bool Sequence;
    [SerializeField] private GameObject[] visits;

    public bool task = true;

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
        else if (OnOnly & interactIcon & !used)
        {
            if (!isActive)
                sr.sprite = active;
            isActive = !isActive;
            playerCharacter.GetComponent<PlayerInteraction>().CloseInteractionIcon();
            interactIcon = !interactIcon;
            used = true;
        }
        else if (Sequence & interactIcon & !used)
        {
            if(!isActive)
                sr.sprite = active;
            isActive = !isActive;
            playerCharacter.GetComponent<PlayerInteraction>().CloseInteractionIcon();
            interactIcon = !interactIcon;
            task = false;
            if (!task)
            {
                for (int x = 0; x < 2; x++)
                {
                    visits[x].SetActive(true);
                }
                task = true;
            }
            used = true;
        }
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = inactive;
        playerCharacter = GameObject.FindGameObjectWithTag("Player");

        if (Sequence)
        {
            for (int x = 0; x < 2; x++)
            {
                visits[x].SetActive(false);
            }
        }
    }

    public bool CheckUsed()
    {
        if (used) return true;
        else return false;
    }
}
