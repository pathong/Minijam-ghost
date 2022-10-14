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

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        float spread = 1;
        Quaternion newRot = _gunPivot.rotation;

        for (int i = 0; i < _bulletAmount; i++)
        {
            float addedOffset = Random.Range(-_maxAngle, _maxAngle);


            // Then add "addedOffset" to whatever rotation axis the player must rotate on
            newRot = Quaternion.Euler(_gunPivot.transform.localEulerAngles.x,
            _gunPivot.transform.localEulerAngles.y,
            _gunPivot.transform.localEulerAngles.z + addedOffset);

            GameObject bullet =  Instantiate(_bulletPrefab, _gunTip.position, newRot);


        }
    }





    
}
