using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBase : CharaParameter
{

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
    }

    /// <summary>
    /// Damageを受けたときの処理
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    public void Damage(int attackPower)
    {
        attackPower -= _defense;
        if (attackPower <= 1)
        {
            attackPower = 1;
        }

        _hp -= attackPower;
        if(_hp <= 0)
        {
            Dead();
        }
    }

    /// <summary>
    /// キャラクターの死亡処理
    /// </summary>
    void Dead()
    {
        Destroy(this.gameObject);
    }
}
