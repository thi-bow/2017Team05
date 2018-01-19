using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurret : weaponFire{

    [Space(10)]
    public Transform m_target;
    public float m_rotateSpeed;

	void Update () {
        if (m_target == null)
            return;

        // 回転速度に応じた角度を求めて計算

        // 差分が角度以内なら回転しない

        // 角度以上なら回転
	}
}
