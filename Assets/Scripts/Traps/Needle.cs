using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : Trap
{
    private void Update()
    {
        if (Check())
        {
            Excute();
        }
    }
    public override void Excute()
    {
        Anim.Play();
    }
}
