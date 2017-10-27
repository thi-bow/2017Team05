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
    /// <param name="aAttckValue">攻撃側ダメージ</param>
    /// <param name="aExprosition">弱点無視かどうか</param>
    public void Damage(int aAttckValue, bool aExprosition = false)
    {
        if(m_weakPoint && !aExprosition)
        {
            aAttckValue *= 2;
        }

        OnDamage.OnNext(aAttckValue);
    }
}
