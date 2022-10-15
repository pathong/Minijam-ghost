using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectBehavior : ObjectInteraction
{
    public Sprite active;
    public Sprite inactive;
    public GameObject countdown;

    private SpriteRenderer sr;
    private bool isActive;
    private GameObject playerCharacter;
    public bool used = false;
    public bool interactIcon = true;
    public bool task = true;

    [Header("4 Types of Interactables")]
    [SerializeField] private bool OnOff;
    [SerializeField] private bool OnOnly;
    [SerializeField] private bool Sequence;
    [SerializeField] private bool Timed;
    [SerializeField] private GameObject[] visits;
    [SerializeField] private GameObject[] lights;
    private bool tick1 = false;
    private bool tick2 = false;
    private bool noClick = false;

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
        else if (Timed & interactIcon & !used)
        {
            if (!isActive)
            {
                sr.sprite = active;
                playerCharacter.GetComponent<PlayerInteraction>().CloseInteractionIcon();
            }
            isActive = true;

            if (!tick1 & !tick2)
            {
                StartCoroutine(Wait(lights[0]));
                countdown.GetComponent<Countdown>().start = true;
                isActive = false;
                playerCharacter.GetComponent<PlayerInteraction>().CloseInteractionIcon();
                interactIcon = true;
                tick1 = true;
                noClick = true;
                return;
            }
            else if (tick1 & !tick2 & !noClick)
            {
                StartCoroutine(Wait(lights[1]));
                countdown.GetComponent<Countdown>().start = true;
                isActive = false;
                playerCharacter.GetComponent<PlayerInteraction>().CloseInteractionIcon();
                interactIcon = true;
                tick2 = true;
                noClick = true;
                return;
            }
            else if (tick1 & tick2 & !noClick)
            {
                StartCoroutine(Wait(lights[2]));
                countdown.GetComponent<Countdown>().start = true;
                isActive = false;
                playerCharacter.GetComponent<PlayerInteraction>().CloseInteractionIcon();
                interactIcon = false;
                used = true;
            }
            
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

        if (Timed)
        {
            for (int x = 0; x < 3; x++)
            {
                lights[x].SetActive(false);
            }
        }
    }

    public bool CheckUsed()
    {
        if (used) return true;
        else return false;
    }

    IEnumerator Wait(GameObject lightBulbSprite)
    {
        yield return new WaitForSeconds(countdown.GetComponent<Countdown>().setTime);
        lightBulbSprite.SetActive(true);
        noClick = false;
    }
}
