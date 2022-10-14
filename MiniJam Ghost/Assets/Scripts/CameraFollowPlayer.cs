using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    //* Basic script, maybe upgrade to cinemachine next times

    private void Update()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if(playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
    }
}
