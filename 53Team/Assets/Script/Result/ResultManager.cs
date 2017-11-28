using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {

    public static bool IsClear = true;
    [SerializeField] private GameObject[] logoImage = null;



	// Use this for initialization
	void Start () {
		if(IsClear)
        {
            logoImage[0].SetActive(true);
            logoImage[1].SetActive(false);
        }
        else
        {
            logoImage[0].SetActive(false);
            logoImage[1].SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Jump"))
        {
            SceneManagerScript.sceneManager.FadeOut(SceneName.sceneName.Title.ToString());
        }
    }
}
