using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenTracker : MonoBehaviour
{
    public GameObject target;
    [SerializeField] private float hideDistance = 1.5f;

    private void Update()
    {
        if(!target.activeSelf)
            SetIndicatorActive(false);
        else
        {
            var direction = target.transform.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (direction.magnitude < hideDistance)
            {
                SetIndicatorActive(false);
            }
            else
            {
                SetIndicatorActive(true);
            }

            if (target.GetComponent<ObjectBehavior>().used) gameObject.SetActive(false);
        }        
    }

    private void SetIndicatorActive(bool isActive)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
