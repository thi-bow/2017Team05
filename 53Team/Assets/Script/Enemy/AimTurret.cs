using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTurret : MonoBehaviour {

    public Transform m_stand;           // 砲塔ユニット
    public Transform m_stand_pivot;     // 砲塔ユニットの回転点
    public Transform m_cannon;          // 主砲ユニット
    public Transform m_cannon_pivot;    // 主砲ユニットの回転点
    public Transform m_aimPoint;        // 主砲ユニットの基準点

    public void Aim(Vector3 target)
    {
        // ターゲットとのベクトルを取得
        var vec = target - m_aimPoint.position;

        // 台座の回転角度の計算
        var angle = Vector3.Angle(m_stand.right, vec);
        var deltaangle = Mathf.DeltaAngle(90, angle);
        RotateStand(deltaangle);

        // 砲身の俯仰角の計算
        angle = Vector3.Angle(-m_cannon.up, vec);       
        deltaangle = Mathf.DeltaAngle(90, angle);
        RotateCannon(deltaangle);
    }

    public void RotateStand(float angle)
    {
        m_stand.RotateAround(m_stand_pivot.position, -m_stand_pivot.up, angle);
    }

    public void RotateCannon(float angle)
    {
        m_cannon.RotateAround(m_cannon_pivot.position, -m_cannon_pivot.right, angle);
    }

    public Transform GetCannon()
    {
        return m_aimPoint;
    }
}
