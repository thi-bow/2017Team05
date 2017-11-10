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
    public Subject<int> OnDamage = new Subject<int>();

    /// <summary>
    /// ダメージ関数
    /// </summary>
    /// <param name="aDmg">攻撃側ダメージ</param>
    /// <param name="aExprosition">弱点無視かどうか</param>
    public void Damage(int aDmg, bool aExprosition = false)
    {
        if(m_weakPoint && !aExprosition)
        {
            aDmg *= 2;
        }

        Debug.LogFormat("{0}パーツに{1}だめーじ", m_parts, aDmg);
        OnDamage.OnNext(aDmg);
    }
}
