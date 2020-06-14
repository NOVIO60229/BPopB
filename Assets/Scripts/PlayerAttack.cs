using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ParticleSystem FireBall;

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack()
    {
        FireBall.Play();
    }
}
