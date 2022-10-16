using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour
{

    public int Id;
    //private Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    public Image Icon;
    public TextMeshProUGUI bulletAmount;


    public BulletSO bullet;
    public MagazineSO magazine;


    public AudioClip reloadShell;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        Icon.sprite = bullet.sprite;
        itemName = bullet.name;
    }

    // Update is called once per frame
    void Update()
    {
        bulletAmount.text = bullet.CurrentAmount.ToString();
        if(bullet.CurrentAmount <= 0)
        {
            this.GetComponent<Button>().interactable = false;
        }
        else
        {
            this.GetComponent<Button>().interactable = true;
        }

    }

    public void HoverEnter()
    {
        //anim.SetBool("Hover", true);
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        //anim.SetBool("Hover", false);
        itemText.text = "";
    }


    public void OnClick()
    {
        if(magazine.GetEmptyIndex() == -1) { return; }
        if(bullet.CurrentAmount <= 0) { return; }

        SoundManager.PlaySound(reloadShell, transform.position);
        magazine.AddBullet(bullet);
        bullet.CurrentAmount--;

    }
}
