using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public enum standard_State
    {
        wait,
        attack
    }

    public class Enemy_Standard : EnemyBase<Enemy_Standard, standard_State>
    {

    }
}
