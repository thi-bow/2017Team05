using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Camera : MonoBehaviour {

    [Header("でばっく")]
    public bool ray;
    public float wheel;
    public GameObject hoge;

    [Space(10)]
    public bool m_run;                  // マウス追従フラグ
    public bool m_aim;                  // エイムフラグ

    public Camera m_camera;
    [Range(1, 100)]
    public float m_value = 50.0f;       // マウス感度
    public float m_distance = 5.0f;     // カメラとTargetの目標距離

    public GameObject m_targetObj;
    public Transform m_camPosition;
    public Transform m_defViewPoint;
    public Transform m_aimViewPoint;

    private Vector3     m_vec1, m_vec2;
    private Transform   m_currentViewPoint;
    private RaycastHit  m_hit;

    private readonly int layerMask = ~(1 << 8 | 1 << 9);
    private readonly float clamp_angle = 60;

	void Start () {
        if(m_targetObj == null)
        {
            m_targetObj = GameObject.FindGameObjectWithTag("Player");
        }

        m_currentViewPoint = m_defViewPoint;
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { m_run = !m_run; }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Aim = !Aim; }
        m_distance -= Input.GetAxis("Mouse ScrollWheel");

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

        var x = mouseInputX * Time.deltaTime * 10f * m_value;
        var y = mouseInputY * Time.deltaTime * 10f * m_value;

        // targetの位置のY軸を中心に、回転(公転)する
        transform.RotateAround(transform.position, Vector3.up, x);
        // カメラの垂直移動（角度制限なし）
        transform.RotateAround(transform.position, transform.right, y);

        var angle = transform.rotation.eulerAngles;
        angle.x = angle.x > 180 ? angle.x - 360 : angle.x;
        angle.x = Mathf.Clamp(angle.x, -clamp_angle, clamp_angle);
        angle.z = 0;
        transform.rotation = Quaternion.Euler(angle);


        m_camera.transform.LookAt(m_currentViewPoint);
    }

    private void UpdateCamMove()
    {
        var pos = m_currentViewPoint.position;
        m_vec1 = (m_camera.transform.position - pos).normalized;

        if (IsRayCast())
        {
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_hit.point, Time.deltaTime * 6);
        }
        else
        {
            m_vec2 = pos + m_vec1 * m_distance;
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_vec2, Time.deltaTime * 6);
        }
    }

    private bool IsRayCast()
    {
        var hit = Physics.Raycast(m_currentViewPoint.position, m_vec1, out m_hit, m_distance, layerMask);
        ray = hit;
        hoge = hit ? m_hit.collider.gameObject : null;
        return hit;
    }

    public Camera GetTPSCamera()
    {
        return m_camera;
    }

    public bool Aim
    {
        get { return m_aim; }
        set
        {
            m_aim = value;
            if (m_aim)
            {
                Vector3 p1, p5, p6, p7, v1, v2;
                p1 = m_defViewPoint.position;
                v1 = (m_camPosition.position - p1).normalized;
                p5 = m_aimViewPoint.position;
                p6 = p1 + -v1 * m_distance;
                v2 = (p5 - p6).normalized;
                p7 = p5 + v2 * m_distance;

                m_camera.transform.position = p7;
                m_currentViewPoint = m_aimViewPoint;
                m_distance = 1;
            }
            else
            {
                m_camera.transform.localPosition = new Vector3(0, 0, 0);
                m_currentViewPoint = m_defViewPoint;
                m_distance = 2;
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (m_camera == null || m_targetObj == null || m_defViewPoint == null) return;

        Gizmos.color = Color.red;
        Vector3 p1, p2, p3, p4, p5, p6, p7, v1, v2;
        p1 = m_defViewPoint.position;
        v1 = (m_camPosition.position - p1).normalized;
        p2 = p1 + v1 * m_distance;
        p3 = m_camPosition.position;
        p4 = ray ? m_hit.point : p2;
        p5 = m_aimViewPoint.position;
        p6 = p1 + -v1 * m_distance;
        v2 = (p5 - p6).normalized;
        p7 = p5 + v2 * m_distance;
        Gizmos.DrawLine(p6, p3);
        Gizmos.DrawLine(p6, p7);
        Gizmos.DrawWireSphere(p1, 0.1f);
        Gizmos.DrawWireSphere(p6, 0.1f);
        Gizmos.DrawWireSphere(p7, 0.1f);
        Gizmos.DrawWireSphere(p5, 0.1f);
        Gizmos.DrawWireCube(p4, new Vector3(0.4f, 0.4f, 0.4f));
    }
}
