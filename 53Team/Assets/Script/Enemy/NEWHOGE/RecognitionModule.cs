using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// 認識範囲の構造体
[Serializable]
public class ViewStructure
{
    public float distance;              // 認識距離
    public Vector2 angle;               // 認識角度(横)

    [Space(10)]
    public Sector sector;               // 索敵範囲描画用

    // コンストラクタ
    public ViewStructure(float aDistance, float aAngle)
    {
        distance = aDistance;
        angle.x = aAngle;
        angle.y = aAngle;
    }

    // コンストラクタ
    public ViewStructure(float aDistance, float aXAngle, float aYAngle)
    {
        distance = aDistance;
        angle.x = aXAngle;
        angle.y = aYAngle;
    }

    /// <summary>
    /// 索敵関数
    /// </summary>
    /// <param name="aOrigin">索敵開始地点</param>
    /// <param name="aDirection">向いている方向</param>
    /// <param name="aTarget">索敵したいポジション</param>
    /// <param name="view">索敵範囲</param>
    /// <returns>当たっかどうか</returns>
    public bool Search(Vector3 aOrigin, Vector3 aDirection, Vector3 aTarget)
    {
        bool hit = false;

        // 対象とのベクトルと距離(2乗)を取得
        var vec = aTarget - aOrigin;
        var dis = Vector3.SqrMagnitude(vec);

        // 対象との横方向と縦方向の角度を計算
        Vector2 angle;
        Vector3 v1, v2;

        v1 = new Vector3(aDirection.x, 0, aDirection.z);
        v2 = new Vector3(vec.x, 0, vec.z);
        angle.x = Vector3.Angle(v1, v2);

        v1 = new Vector3(0, aDirection.y, aDirection.z);
        v2 = new Vector3(0, vec.y, vec.z);
        angle.y = Vector3.Angle(v1, v2);


        // 対象と指定距離以内
        if (dis <= distance * distance)
        {

            // 対象と指定角度以内
            if (this.angle.x >= angle.x && this.angle.y >= angle.y)
            {

                // 対象へと向けてRayCastを飛ばす
                RaycastHit raycastHit;
                if (Physics.Raycast(aOrigin, vec.normalized, out raycastHit, distance))
                {
                    var pos = raycastHit.collider ? raycastHit.transform.position : raycastHit.point;
                    hit = pos == aTarget;
                    hit = true;
                }
            }
        }

        return hit;
    }
}


// 認識モジュール
public class RecognitionModule : MonoBehaviour{

    [SerializeField] ViewStructure m_mainView;      // メイン認識範囲の情報
    [SerializeField] ViewStructure m_subView;       // サブ認識範囲の情報

    public ViewStructure MainView
    {
        get { return m_mainView; }
    }

    public ViewStructure SubView
    {
        get { return m_subView; }
    }

    public void DrawArea()
    {
        if(m_mainView.sector != null)
        {
            m_mainView.sector.Show(m_mainView.distance, 90 - m_mainView.angle.x, 90 + m_mainView.angle.x);
        }
        if(m_subView.sector != null)
        {
            m_subView.sector.Show(m_subView.distance, 90 - m_subView.angle.x, 90 + m_subView.angle.x);
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(RecognitionModule))]
public class RecognitionModuleEx : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("DrawArea"))
        {
            Debug.Log("索敵範囲描画！！");
            RecognitionModule view = target as RecognitionModule;

            view.DrawArea();
        }
    }
}
#endif
