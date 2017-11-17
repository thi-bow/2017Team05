using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Camera : MonoBehaviour {

    [Header("でばっく")]
    public bool ray;
    public GameObject hoge;

    [Space(10)]
    public bool m_run;                  // マウス追従フラグ

    public Camera m_camera;
    [Range(1, 100)]
    public float m_value = 50.0f;       // マウス感度
    public float m_distance = 5.0f;     // カメラとTargetの目標距離

    public GameObject m_targetObj;
    public Transform m_offsetPoint;

    private Vector3     m_currentPos;
    private Vector3     m_vec1, m_vec2;
    private RaycastHit  m_hit;

    private readonly float clamp_angle = 60;
    private readonly int layerMask = ~(1 << 8 | 1 << 9);
    private readonly float defRadius = 1.0f;

	void Start () {
        if(m_targetObj == null)
        {
            m_targetObj = GameObject.FindGameObjectWithTag("Player");
        }
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

        var x = mouseInputX * Time.deltaTime * 10f * m_value;
        var y = mouseInputY * Time.deltaTime * 10f * m_value;

        // targetの位置のY軸を中心に、回転(公転)する
        transform.RotateAround(transform.position, Vector3.up, x);
        // カメラの垂直移動（角度制限なし）
        transform.RotateAround(transform.position, transform.right, y);

        var angle = transform.rotation.eulerAngles;
        angle.x = angle.x > 180 ? angle.x - 360 : angle.x;
        angle.x = Mathf.Clamp(angle.x, -clamp_angle, clamp_angle);

        transform.rotation = Quaternion.Euler(angle);


        m_camera.transform.LookAt(m_offsetPoint.position);
    }

    private void UpdateCamMove()
    {
        // targetの移動量分、カメラも移動する
        m_currentPos = m_targetObj.transform.position + m_offsetPoint.localPosition + transform.localPosition;
        m_vec1 = (m_camera.transform.position - m_currentPos).normalized;

        if (IsRayCast())
        {
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_hit.point, Time.deltaTime * 6);
        }
        else
        {
            var p = m_currentPos + m_vec1 * m_distance;
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, p, Time.deltaTime * 6);
        }
    }

    private bool IsRayCast()
    {
        var hit = Physics.Raycast(m_currentPos, m_vec1, out m_hit, m_distance, layerMask);
        ray = hit;
        hoge = hit ? m_hit.collider.gameObject : null;
        return hit;
    }

    public Camera GetTPSCamera()
    {
        return m_camera;
    }

    private void OnDrawGizmos()
    {
        if (m_camera == null || m_targetObj == null || m_offsetPoint == null) return;

        Gizmos.color = Color.red;
        Vector3 p1, p2, p3, p4, v1, v2;
        p1 = m_offsetPoint.position;
        v1 = (m_camera.transform.position - p1).normalized;
        p2 = p1 + v1 * m_distance;
        p3 = m_camera.transform.position;
        v2 = m_camera.transform.forward;
        p4 = ray ? m_hit.point : p2;
        Gizmos.DrawLine(p1, p4);
        Gizmos.DrawRay(new Ray(p3, v2));
        Gizmos.DrawWireSphere(p1, 0.2f);
        Gizmos.DrawWireCube(p4, new Vector3(0.6f, 0.6f, 0.6f));
    }
}
