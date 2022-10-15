using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Animator animator;

    [SerializeField] private Transform _gunTip;
    [SerializeField] private Transform _gunPivot;
    [SerializeField] private int _bulletAmount;
    [SerializeField] private float _maxAngle;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Shoot sound Test")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;


    [SerializeField] private float _maxShootCooldown;
    private float _shootCooldown;

    private PlayerAction playerAction;

    [SerializeField] private int maxBulletAmount;
    private int currentBulletAmount;


    public bool isReloading;



    private void OnEnable()
    {
        playerAction = new PlayerAction();
        playerAction.Enable();
        playerAction.Movement.Shoot.performed += Shoot;
        playerAction.Movement.Reload.performed += Reload;
    }

    private void OnDisable()
    {
        playerAction.Disable();
        playerAction.Movement.Shoot.performed -= Shoot;
    }


    private void Awake()
    {
        _shootCooldown = _maxShootCooldown;
        currentBulletAmount = maxBulletAmount;
        isReloading = false;
    }

    private void Update()
    {
        if(_shootCooldown >= 0)
        {
            _shootCooldown -= Time.deltaTime;
        }
    }


    public void Shoot(InputAction.CallbackContext ctx)
    {
        if(_shootCooldown >= 0) { return; }
        if(currentBulletAmount <= 0) { return; }
        if (isReloading) { return; }
        Quaternion newRot = _gunPivot.rotation;

        // test sound
        SoundManager.PlaySound(shootSound, transform.position);

        if (SoundGraphManager.soundGraphManager != null) { SoundGraphManager.TriggerSoundGraph(transform.position); } 
        // trigger flash
        Flash.Trigger();
        // trigger gun animation
        animator.SetTrigger("Shoot");

        for (int i = 0; i < _bulletAmount; i++)
        {
            float addedOffset = Random.Range(-_maxAngle, _maxAngle);


            newRot = Quaternion.Euler(_gunPivot.transform.localEulerAngles.x,
            _gunPivot.transform.localEulerAngles.y,
            _gunPivot.transform.localEulerAngles.z + addedOffset);


            GameObject bullet =  Instantiate(_bulletPrefab, _gunTip.position, newRot);

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bullet.GetComponent<BulletBehaviour>().SetDistance(Vector2.Distance(mousePos, transform.position));

        }




        currentBulletAmount -= 1;
        _shootCooldown = _maxShootCooldown;
    }

    public void Reload(InputAction.CallbackContext ctx)
    {
        if(currentBulletAmount > 0) { return; }
        if (isReloading) { return; }
        isReloading = true;
        this.GetComponent<PlayerMovement>().MoveSpeed /= 2;

        // trigger gun animation
        animator.SetBool("isReloading", true);

        StartCoroutine(nameof(Reloading));
    }

    IEnumerator Reloading()
    {
        float reloadTime = 1f;
        while(currentBulletAmount != maxBulletAmount)
        {
            yield return new WaitForSeconds(reloadTime);
            SoundManager.PlaySound(reloadSound);
            currentBulletAmount++;
        }
        isReloading = false;
        animator.SetBool("isReloading", false);

        this.GetComponent<PlayerMovement>().MoveSpeed *= 2;
    }




    
}
