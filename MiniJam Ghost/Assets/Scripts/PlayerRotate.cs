using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public Animator animator;

    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private GameObject sprite;
    

    private void FixedUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        transform.right = dir;

        if(dir.x < 0 && isFacingRight)
        {
            sprite.transform.localRotation = Quaternion.Euler(180, 0, 0);
            isFacingRight = false;
        }
        if(dir.x > 0 && !isFacingRight)
        {
            sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = true;
        }
    }

}
