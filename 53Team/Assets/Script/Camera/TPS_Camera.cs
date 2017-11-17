using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Camera : MonoBehaviour {

    [Header("でばっく")]
    public bool ray;

    [Space(10)]
    public bool m_run;                  // マウス追従フラグ

    [Range(1, 100)]
    public float m_value = 50.0f;       // マウス感度
    public float m_distance = 5.0f;     // カメラとTargetの目標距離

    public GameObject m_targetObj;
    public Vector3 m_offset;

    private Vector3     m_targetPos, m_currentPos;
    private Vector3     m_vec1, m_vec2;
    private RaycastHit  m_hit;

    private readonly float defRadius = 1.0f;

	void Start () {
        if(m_targetObj == null)
        {
            m_targetObj = GameObject.FindGameObjectWithTag("Player");
        }
        m_targetPos = m_targetObj.transform.position + m_offset;
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { m_run = !m_run; }

        if (m_run)
        {
            m_vec1 = (transform.position - m_targetObj.transform.position).normalized;

            UpdateCamMove();
        }
    }

    private void UpdateCamMove()
    {
        // targetの移動量分、カメラも移動する
        m_currentPos = m_targetObj.transform.position + m_offset;
        transform.position += m_currentPos - m_targetPos;
        m_targetPos = m_currentPos;

        float mouseInputX = Input.GetAxis("Mouse X");
        float mouseInputY = -Input.GetAxis("Mouse Y");

        // targetの位置のY軸を中心に、回転(公転)する
        transform.RotateAround(m_targetPos, Vector3.up, mouseInputX * Time.deltaTime * 10f * m_value);
        // カメラの垂直移動（角度制限なし）
        transform.RotateAround(m_targetPos, transform.right, mouseInputY * Time.deltaTime * 10f * m_value);

        if (IsRayCast())
        {
            transform.position = Vector3.Lerp(transform.position, m_hit.point, Time.deltaTime);
        }
        else
        {
            var p = m_currentPos + m_vec1 * m_distance;
            transform.position = Vector3.Lerp(transform.position, p, Time.deltaTime);
        }
    }

    private bool IsRayCast()
    {
        var hit = Physics.SphereCast(m_currentPos, defRadius, m_vec1, out m_hit, m_distance);
        ray = hit;
        return hit;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 p1, p2, v1;
        p1 = m_targetObj.transform.position + m_offset;
        v1 = (transform.position - p1).normalized;
        p2 = p1 + v1 * m_distance;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawWireSphere(p1, 0.2f);
        Gizmos.DrawWireSphere(p2, 0.5f);
    }
}
