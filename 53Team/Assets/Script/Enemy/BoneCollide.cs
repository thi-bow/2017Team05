using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BoneCollide : MonoBehaviour {

    // 弱点かどうか
    public bool m_weakPoint = false;

    // どこの部位か
    public CharaBase.Parts m_parts;

    // ダメージ受けたとき
    public Subject<Damageble> OnDamage = new Subject<Damageble>();

    public struct Damageble
    {
        public int value;
        public Weapon.Attack_State type;
    }

    /// <summary>
    /// ダメージ関数
    /// </summary>
    /// <param name="aDmg">攻撃側ダメージ</param>
    /// <param name="aExprosition">弱点無視かどうか</param>
    public void Damage(int aDmg, Weapon.Attack_State aType)
    {
        if(m_weakPoint)
        {
            aDmg *= 2;
        }

        Damageble dmg;
        dmg.value = aDmg;
        dmg.type = aType;
        
        OnDamage.OnNext(dmg);
    }
}
