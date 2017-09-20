using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region プレイヤーデータ
    [SerializeField] private Image _skillGageImage = null;
    private float _skillGage = 0.0f;
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public float SkillCheck
    {
        get { return _skillGage; }
    }

    public void SkillGageAdd(float addNumber = 1.0f)
    {
        _skillGage += addNumber;
        if (_skillGage >= 100.0f)
        {
            _skillGage = 100.0f;
        }
        _skillGageImage.fillAmount = _skillGage / 100.0f;
    }

    public void SkillGageSub(float subNumber = 25.0f)
    {
        _skillGage -= subNumber;
        if (_skillGage <= 0.0f)
        {
            _skillGage = 0.0f;
        }
        _skillGageImage.fillAmount = _skillGage / 100.0f;
    }

    public void SkillGageReset()
    {
        _skillGage = 0.0f;
        _skillGageImage.fillAmount = 0.0f;
    }
}
