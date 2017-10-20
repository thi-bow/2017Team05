using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BoneCollide : MonoBehaviour {

    public CharaBase.Parts m_parts;

    public Subject<int> OnDamage = new Subject<int>();

    public void Damage(int aAttckValue)
    {
        OnDamage.OnNext(aAttckValue);
    }
}
