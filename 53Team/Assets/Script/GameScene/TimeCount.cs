using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCount : MonoBehaviour {

    public static float _timeCount = 0.0f;

    // Use this for initialization
    void Start () {
        _timeCount = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {
        if(GameController.m_isTutorial)
        {
            _timeCount += Time.deltaTime;
        }
		
	}
}
