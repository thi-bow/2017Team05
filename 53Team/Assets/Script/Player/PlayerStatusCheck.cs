using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusCheck : MonoBehaviour
{
    #region プレイヤーデータ
    [SerializeField] private Player _player = null;
    [SerializeField] private Image _playerHP = null;
    [SerializeField] private Image[] _partsHP = null;
    [SerializeField] private Color[] _hpColor = null;
    #endregion

    [SerializeField] private Image _skillGageImage = null;
    private float _skillGage = 0.0f;

    // Use this for initialization
    void Start () {
        PlayerHP();
        for (int i = 0; i < _player._partsHP.Count; i++)
        {
            ArmorHP(i, _player._partsHP[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerHP()
    {
        float count = 1.0f / (float)_player._charaPara._maxHP;
        _playerHP.fillAmount = _player._charaPara._hp * count;
    }

    public void ArmorHP(int partsNumber, int hp)
    {
        if (partsNumber >= 4) return;
        int hpNumber = 0;
        if (hp >= 100) hpNumber = 2;
        else if (hp >= 50) hpNumber = 1;
        else hpNumber = 0;
        _partsHP[partsNumber].color = _hpColor[hpNumber];

    }

    //プレイヤーのスキルゲージの確認
    public float SkillCheck
    {
        get { return _skillGage; }
    }

    //スキルゲージの増幅
    public void SkillGageAdd(float addNumber = 1.0f)
    {
        _skillGage += addNumber;
        if (_skillGage >= 100.0f)
        {
            _skillGage = 100.0f;
        }
        _skillGageImage.fillAmount = _skillGage / 100.0f;
    }

    //スキルゲージの減少
    public void SkillGageSub(float subNumber = 25.0f)
    {
        _skillGage -= subNumber;
        if (_skillGage <= 0.0f)
        {
            _skillGage = 0.0f;
        }
        _skillGageImage.fillAmount = _skillGage / 100.0f;
    }

    //スキルゲージのリセット
    public void SkillGageReset()
    {
        _skillGage = 0.0f;
        _skillGageImage.fillAmount = 0.0f;
    }
}
