using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HurtArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Hurt(10);
        }
    }
}
