using UnityEngine;
using UnityEngine.UI;



public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;
    public Sprite noImage;
    public static int weaponID;


    private void OnEnable()
    {
        PlayerShoot.OnPlayerReload += Toggle; 
    }

    private void OnDisable()
    {
        PlayerShoot.OnPlayerReload -= Toggle; 
        
    }

    // Update is called once per frame
    void Toggle()
    {
        weaponWheelSelected = !weaponWheelSelected;
        if (weaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
        }
    }
}
