﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ResultScore.Reset();
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Jump"))
        {
            SceneManagerScript.sceneManager.FadeOut("CheckScene");
        }
		
	}
}
