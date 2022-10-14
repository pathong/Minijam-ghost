using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform _gunTip;
    [SerializeField] private Transform _gunPivot;
    [SerializeField] private int _bulletAmount;
    [SerializeField] private float _maxAngle;
    [SerializeField] private GameObject _bulletPrefab;


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

        for (int i = 0; i < _bulletAmount; i++)
        {
            float addedOffset = Random.Range(-_maxAngle, _maxAngle);


            newRot = Quaternion.Euler(_gunPivot.transform.localEulerAngles.x,
            _gunPivot.transform.localEulerAngles.y,
            _gunPivot.transform.localEulerAngles.z + addedOffset);

            GameObject bullet =  Instantiate(_bulletPrefab, _gunTip.position, newRot);

        }


        this.GetComponent<PlayerMovement>().Knockback(transform.position - _gunTip.position);

        currentBulletAmount -= 1;
        _shootCooldown = _maxShootCooldown;
    }

    public void Reload(InputAction.CallbackContext ctx)
    {
        if(currentBulletAmount > 0) { return; }
        isReloading = true;
        this.GetComponent<PlayerMovement>().MoveSpeed /= 2;

        StartCoroutine(nameof(Reloading));
    }

    IEnumerator Reloading()
    {
        float reloadTime = 1f;
        while(currentBulletAmount != maxBulletAmount)
        {
            yield return new WaitForSeconds(reloadTime);
            Debug.Log("reload");
            currentBulletAmount++;
        }
        isReloading = false;
        this.GetComponent<PlayerMovement>().MoveSpeed *= 2;
    }




    
}
