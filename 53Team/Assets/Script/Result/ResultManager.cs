using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {

    public static bool IsClear = true;
    [SerializeField] private GameObject[] logoImage = null;
    [SerializeField] private Text _killCount = null;
    [SerializeField] private Text _shotKill = null;
    [SerializeField] private Text _approachKill = null;
    [SerializeField] private Text _clearTime = null;



	// Use this for initialization
	void Start () {
		if(IsClear)
        {
            logoImage[0].SetActive(true);
            logoImage[1].SetActive(false);

            _killCount.gameObject.SetActive(true);
            _killCount.text = "敵を倒した数:" + ResultScore.KillCount.ToString();


            //_shotKill.gameObject.SetActive(true);
            //_shotKill.text = "敵を倒した数:" + ResultScore.ShotKillCount.ToString();

            //_approachKill.gameObject.SetActive(true);
            //_approachKill.text = ResultScore.ApproachKillCount.ToString();


            _clearTime.gameObject.SetActive(true);
            _clearTime.text = TimeText();

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

    string TimeText()
    {
        string timeText = "";
        int time = (int)TimeCount._timeCount;
        string secondsText = "00";
        int minutesTime = 0;
        string minutuText = "00";
        if (time >= 60)
        {
            minutesTime = time / 60;
            time = time - (minutesTime * 60);

            if(minutesTime <= 9)
            {
                minutuText = "0" + minutesTime.ToString();
            }
            else
            {
                minutuText = minutesTime.ToString();
            }
        }

        if(time <= 9)
        {
            secondsText = "0" + time.ToString();
        }
        else
        {
            secondsText = time.ToString();
        }

        timeText = "クリア時間:" + minutuText + "分" + secondsText + "秒";

        return timeText;
    }
}
