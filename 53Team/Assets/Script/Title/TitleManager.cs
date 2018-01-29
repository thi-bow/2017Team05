using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {
    private bool _sceneMove = false;

	// Use this for initialization
	void Start () {
        ResultScore.Reset();
        GameController._pause = false;
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!_sceneMove && Input.anyKeyDown)
        {
            _sceneMove = true;
            SoundManger.Instance.PlaySE(5);
            SceneManagerScript.sceneManager.FadeOut("Game");
        }
		
	}
}
