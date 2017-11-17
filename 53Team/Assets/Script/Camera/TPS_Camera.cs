using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Camera : MonoBehaviour {

    [Header("でばっく")]
    public bool ray;
    public Vector3 hoge;
    public Vector3 rotate;


    [Space(10)]
    public bool m_run;                  // マウス追従フラグ

    public Camera m_camera;
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
            UpdateCamMove();
            UpdateCamRotate();
        }
    }

    private void UpdateCamRotate()
    {
        float mouseInputX = Input.GetAxis("Mouse X");
        float mouseInputY = -Input.GetAxis("Mouse Y");

        rotate.x = mouseInputX * Time.deltaTime * 10f * m_value;
        rotate.y = mouseInputY * Time.deltaTime * 10f * m_value;

        // targetの位置のY軸を中心に、回転(公転)する
        transform.RotateAround(transform.position, Vector3.up, rotate.x);
        // カメラの垂直移動（角度制限なし）
        transform.RotateAround(transform.position, transform.right, rotate.y);



        m_camera.transform.LookAt(m_currentPos);
    }

    private void UpdateCamMove()
    {
        // targetの移動量分、カメラも移動する
        m_currentPos = m_targetObj.transform.position + m_offset;
        //transform.position += m_currentPos - m_targetPos;
        m_vec1 = (m_camera.transform.position - m_currentPos).normalized;
        //m_targetPos = m_currentPos;

        if (IsRayCast())
        {
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_hit.point, Time.deltaTime);
        }
        else
        {
            var p = m_currentPos + m_vec1 * m_distance;
            hoge = p;
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, p, Time.deltaTime);
        }
    }

    private bool IsRayCast()
    {
        var hit = Physics.SphereCast(m_currentPos, defRadius, m_vec1, out m_hit, m_distance);
        ray = hit;
        return hit;
    }

    public Camera GetTPSCamera()
    {
        return m_camera;
    }

    private void OnDrawGizmosSelected()
    {
        if (m_camera == null || m_targetObj == null) return;

        Gizmos.color = Color.red;
        Vector3 p1, p2, p3, v1, v2;
        p1 = m_targetObj.transform.position + m_offset;
        v1 = (m_camera.transform.position - p1).normalized;
        p2 = p1 + v1 * m_distance;
        p3 = m_camera.transform.position;
        v2 = m_camera.transform.forward;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawRay(new Ray(p3, v2));
        Gizmos.DrawWireSphere(p1, 0.2f);
        Gizmos.DrawWireSphere(p2, 0.5f);
    }
}
