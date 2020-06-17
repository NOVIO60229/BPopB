using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Walkable : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        transform.DOMoveY(transform.position.y - 20, 10f);
    }
}
