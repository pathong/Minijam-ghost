using UnityEngine;
using UnityEngine.UI;



public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponID;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            weaponWheelSelected = !weaponWheelSelected;
        }
        if (weaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
        }
        switch (weaponID)
        {
            case 0:
                selectedItem.sprite = noImage; 
                break;
            case 1:
                Debug.Log("Normal");
                break;
            case 2:
                Debug.Log("Fire");
                break;
            case 3:
                Debug.Log("Light");
                break;
            
        }
    }
}
