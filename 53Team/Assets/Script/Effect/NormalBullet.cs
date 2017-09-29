using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;

public class NormalBullet : MonoBehaviour {

    private Transform m_startTransform;
    private Transform m_targetTransform;

    public IObservable<Unit> Shot(Transform aPos, Transform aTarget)
    {
        m_startTransform = aPos;
        m_targetTransform = aTarget;

        return Observable.ReturnUnit();
    }
}

public class NormalBulletPool : ObjectPool<NormalBullet>
{
    private readonly NormalBullet _prefab;
    private readonly Transform _parentTransform;

    public NormalBulletPool(NormalBullet prefab, Transform parent = null)
    {
        _prefab = prefab;
        _parentTransform = parent;
    }

    protected override NormalBullet CreateInstance()
    {
        var go = GameObject.Instantiate(_prefab, _parentTransform);

        return go;
    }

    protected override void OnBeforeRent(NormalBullet instance)
    {
        base.OnBeforeRent(instance);
    }

    protected override void OnBeforeReturn(NormalBullet instance)
    {
        base.OnBeforeReturn(instance);
    }
}
