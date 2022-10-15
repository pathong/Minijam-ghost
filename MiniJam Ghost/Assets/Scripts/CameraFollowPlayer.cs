using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    //* Basic script, maybe upgrade to cinemachine next times

    private void Update()
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null) {
            Transform playerTransform = _player.transform;
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
    }
}
