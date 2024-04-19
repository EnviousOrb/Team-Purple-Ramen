using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void takeDamage(int amount, int type);//1 = water, 2 = fire, 3 = lightning, 4 = plant
}
