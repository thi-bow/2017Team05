using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharaParameter
{

    [Header("キャラクターのHP")]
    #region Hp
    public int _hp = 1000;
    public int _bodyHp = 0;
    public int _rightArmHp = 0;
    public int _leftArmHp = 0;
    public int _legHp = 0;
    public int _boosterHp = 0;
    #endregion

    [Header("キャラクターのDefense")]
    #region Defense
    public int _defense = 0;
    public int _bodyDefense = 0;
    public int _rightArmDefense = 0;
    public int _leftArmDefense = 0;
    public int _legDefense = 0;
    public int _boosterDefense = 0;
    #endregion

    [Header("キャラクターのWeight")]
    #region Weight
    public int _totalWeight = 0;
    public int _maxWeight = 0;
    public int _bodyWeight = 0;
    public int _rightArmWeight = 0;
    public int _leftArmWeight = 0;
    public int _legWeight = 0;
    public int _boosterWeight = 0;
    #endregion
    [Space(10)]
    public int _attack = 1;
    [Space(10)]
    public float _speed = 1.0f;
}

public class CharaBase : MonoBehaviour
{
    public enum Parts
    {
        Body = 0,
        RightArm,
        LeftArm,
        Leg,
        Booster,
        WeakPoint,
    }

    public CharaParameter _charaPara;

    #region 装備
    [Header("キャラクターベース")]
    [SerializeField] private List<Armor> _bodyList = new List<Armor>();
    [SerializeField] private List<Armor> _rightArmList = new List<Armor>();
    [SerializeField] private List<Armor> _leftArmList = new List<Armor>();
    [SerializeField] private List<Armor> _legList = new List<Armor>();
    [SerializeField] private List<Armor> _boosterList = new List<Armor>();
    List<Parts> _allPartsList = new List<Parts>(); 
    private int partsMax = 5;
    private Parts _parts;
    [SerializeField] private GameObject[] _partsLocation;
    #endregion

    private Action pargeBefore = null;


    #region 装備のプロパティ

    public List<Armor> BodyArmorList
    {
        get { return _bodyList; }
    }

    public List<Armor> RightArmArmorList
    {
        get { return _rightArmList; }
    }

    public List<Armor> LeftArmArmorList
    {
        get { return _leftArmList; }
    }

    public List<Armor> LegArmorList
    {
        get { return _legList; }
    }

    public List<Armor> BoosterArmorList
    {
        get { return _boosterList; }
    }
    #endregion

    #region HPのプロパティ
    public int HP
    {
        get { return _charaPara._hp; }
    }
    public int BodyHP
    {
        get { return _charaPara._bodyHp; }
    }
    public int RightArmHP
    {
        get { return _charaPara._rightArmHp; }
    }
    public int LeftArmHP
    {
        get { return _charaPara._leftArmHp; }
    }
    public int LegHP
    {
        get { return _charaPara._legHp; }
    }
    public int BoosterHP
    {
        get { return _charaPara._boosterHp; }
    }

    #endregion

    #region 攻撃のプロパティ
    public int Attack
    {
        get { return _charaPara._attack; }
    }

    #endregion

    #region 移動のプロパティ
    public float Speed
    {
        get { return _charaPara._speed; }
    }
    #endregion

    // Use this for initialization
    protected virtual void Start ()
    {
        _allPartsList = new List<Parts> { Parts.Body, Parts.RightArm, Parts.LeftArm, Parts.Leg, Parts.Booster };
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
    }

    #region PartsAdd
    public void PartsAdd(Parts parts, Armor armor)
    {
        if (_charaPara._totalWeight >= _charaPara._maxWeight)
        {
            return;
        }
        switch (parts)
        {
            case Parts.Body:
                _bodyList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._bodyDefense += armor.ArmorDefPara;
                _charaPara._bodyHp += armor.ArmorHpPara;
                _charaPara._bodyWeight += armor.ArmorWeightPara;
                armor.gameObject.transform.SetParent(_partsLocation[0].transform);
                armor.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case Parts.RightArm:
                _rightArmList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._rightArmDefense += armor.ArmorDefPara;
                _charaPara._rightArmHp += armor.ArmorHpPara;
                _charaPara._rightArmWeight += armor.ArmorWeightPara;
                armor.gameObject.transform.SetParent(_partsLocation[1].transform);
                armor.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case Parts.LeftArm:
                _leftArmList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._leftArmDefense += armor.ArmorDefPara;
                _charaPara._leftArmHp += armor.ArmorHpPara;
                _charaPara._leftArmWeight += armor.ArmorWeightPara;
                armor.gameObject.transform.SetParent(_partsLocation[2].transform);
                armor.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case Parts.Leg:
                _legList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._legDefense += armor.ArmorDefPara;
                _charaPara._legHp += armor.ArmorHpPara;
                _charaPara._legWeight += armor.ArmorWeightPara;

                //足に装着する場合は、右足か左足かランダムで決める
                int rand = UnityEngine.Random.Range(3, 5);
                armor.gameObject.transform.SetParent(_partsLocation[rand].transform);
                armor.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case Parts.Booster:
                _boosterList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._boosterDefense += armor.ArmorDefPara;
                _charaPara._boosterHp += armor.ArmorHpPara;
                _charaPara._boosterWeight += armor.ArmorWeightPara;
                armor.gameObject.transform.SetParent(_partsLocation[5].transform);
                armor.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            default:
                break;
        }
        armor.GetComponent<BoxCollider>().enabled = false;
        _charaPara._totalWeight += armor.ArmorWeightPara;
    }
    #endregion

    //部位ごとにパージする
    #region PartsPurge
    public void PartsPurge(Parts parts, Action action = null)
    {
        switch (parts)
        {
            case Parts.Body:
                if (_bodyList.Count <= 0) return;
                for (int i = 0; i < _bodyList.Count; i++)
                {
                    _bodyList[i].transform.parent = null;
                }
                if(action != null)
                {
                    action();
                }
                _bodyList.Clear();
                _charaPara._bodyDefense = 0;
                _charaPara._totalWeight -= _charaPara._bodyWeight;
                _charaPara._bodyWeight = 0;
                break;
            case Parts.RightArm:
                if (_rightArmList.Count <= 0) return;
                for (int i = 0; i < _rightArmList.Count; i++)
                {
                    _rightArmList[i].transform.parent = null;
                }
                if (action != null)
                {
                    action();
                }
                _rightArmList.Clear();
                _charaPara._rightArmDefense = 0;
                _charaPara._totalWeight -= _charaPara._rightArmWeight;
                _charaPara._rightArmWeight = 0;
                break;
            case Parts.LeftArm:
                if (_leftArmList.Count <= 0) return;
                for (int i = 0; i < _leftArmList.Count; i++)
                {
                    _leftArmList[i].transform.parent = null;
                }
                if (action != null)
                {
                    action();
                }
                _leftArmList.Clear();
                _charaPara._leftArmDefense = 0;
                _charaPara._totalWeight -= _charaPara._leftArmWeight;
                _charaPara._leftArmWeight = 0;
                break;
            case Parts.Leg:
                if (_legList.Count <= 0) return;
                for (int i = 0; i < _legList.Count; i++)
                {
                    _legList[i].transform.parent = null;
                }
                if (action != null)
                {
                    action();
                }
                _legList.Clear();
                _charaPara._legDefense = 0;
                _charaPara._totalWeight -= _charaPara._legWeight;
                _charaPara._legWeight = 0;
                break;
            case Parts.Booster:
                if (_boosterList.Count <= 0) return;
                for (int i = 0; i < _boosterList.Count; i++)
                {
                    _boosterList[i].transform.parent = null;
                }
                if (action != null)
                {
                    action();
                }
                _boosterList.Clear();
                _charaPara._boosterDefense = 0;
                _charaPara._totalWeight -= _charaPara._boosterWeight;
                _charaPara._boosterWeight = 0;
                break;
            default:
                break;
        }
    }
    #endregion

    //全ての装備をパージする
    #region FullParge
    public void FullParge(Action action = null)
    {
        if (action != null)
        {
            action();
        }
        for (int i = 0; i < _allPartsList.Count; i++)
        {
            PartsPurge(_allPartsList[i]);
        }
    }
    #endregion

    //部位に攻撃が当たった時のダメージ計算
    #region PartsDamage
    public void PartsDamage(int attackPower, Parts parts, Action action = null)
    {
        switch (parts)
        {
            case Parts.Body:
                //パーツに何もついてなければ本体にダメージが入る
                if (_charaPara._bodyHp <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                attackPower -= _charaPara._bodyDefense;
                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _charaPara._bodyHp -= attackPower;
                if (_charaPara._bodyHp <= 0)
                {
                    _charaPara._bodyHp = 0;
                    PartsPurge(parts, action);
                }
                Damage(attackPower);
                break;
            case Parts.RightArm:
                //パーツに何もついてなければ本体にダメージが入る
                if (_charaPara._rightArmHp <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _charaPara._rightArmHp -= attackPower;
                if (_charaPara._rightArmHp <= 0)
                {
                    _charaPara._rightArmHp = 0;
                    PartsPurge(parts, action);
                }
                break;
            case Parts.LeftArm:
                //パーツに何もついてなければ本体にダメージが入る
                if (_charaPara._leftArmHp <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _charaPara._leftArmHp -= attackPower;
                if (_charaPara._leftArmHp <= 0)
                {
                    _charaPara._leftArmHp = 0;
                    PartsPurge(parts, action);
                }
                break;
            case Parts.Leg:
                //パーツに何もついてなければ本体にダメージが入る
                if (_charaPara._legHp <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _charaPara._legHp -= attackPower;
                if (_charaPara._legHp <= 0)
                {
                    _charaPara._legHp = 0;
                    PartsPurge(parts, action);
                }
                break;
            case Parts.Booster:
                //パーツに何もついてなければ本体にダメージが入る
                if (_charaPara._boosterHp <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _charaPara._boosterHp -= attackPower;
                if (_charaPara._boosterHp <= 0)
                {
                    _charaPara._boosterHp = 0;
                    PartsPurge(parts, action);
                }
                break;
            default:
                break;
        }

    }
    #endregion

    //ダメージを受けた時の計算
    #region Damage
    /// <summary>
    /// Damageを受けたときの処理
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    public virtual void Damage(int attackPower)
    {
        attackPower -= _charaPara._defense;
        if (attackPower <= 1)
        {
            attackPower = 1;
        }

        _charaPara._hp -= attackPower;
        if(_charaPara._hp <= 0)
        {
            Dead();
        }
    }
    #endregion

    #region Dead
    /// <summary>
    /// キャラクターの死亡処理
    /// </summary>
    public virtual void Dead()
    {
        Destroy(this.gameObject);
    }
    #endregion

}
