using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HogeTwo : MonoBehaviour {

    public bool hit;

    private RecognitionModule m_recognition;

    public Transform m_target;

	// Use this for initialization
	void Start () {
        m_recognition = GetComponent<RecognitionModule>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        hit = m_recognition.MainView.Search(transform.position, transform.forward, m_target.position);
	}
}
