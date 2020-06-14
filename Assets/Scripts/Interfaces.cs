using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    int HP { get; set; }
    void Hurt(int damage);
}

public interface IBounce
{
    void Bounce();
}