using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing_bullet : Enemy_bullet {

    [Space(10)]
    public Transform m_target;
    public float m_rotateSpeed;

    void Update () {
        if (m_target == null)
            return;

        // ここら辺に誘導処理
    }
}
