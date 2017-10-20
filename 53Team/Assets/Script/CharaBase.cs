using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBase : CharaParameter
{
    public enum Parts
    {
        Body = 0,
        RightArm,
        LeftArm,
        Leg,
        Booster,
    }

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
        if (_totalWeight >= _maxWeight)
        {
            return;
        }
        switch (parts)
        {
            case Parts.Body:
                _bodyList.Add(armor);
                _bodyDefense += armor.ArmorDefPara;
                _bodyHp += armor.ArmorHpPara;
                _bodyWeight += (int)armor.ArmorWeightPara;
                break;
            case Parts.RightArm:
                _rightArmList.Add(armor);
                _rightArmDefense += armor.ArmorDefPara;
                _rightArmHp += armor.ArmorHpPara;
                _rightArmWeight += (int)armor.ArmorWeightPara;
                break;
            case Parts.LeftArm:
                _leftArmList.Add(armor);
                _leftArmDefense += armor.ArmorDefPara;
                _leftArmHp += armor.ArmorHpPara;
                _leftArmWeight += (int)armor.ArmorWeightPara;
                break;
            case Parts.Leg:
                _legList.Add(armor);
                _legDefense += armor.ArmorDefPara;
                _legHp += armor.ArmorHpPara;
                _legWeight += (int)armor.ArmorWeightPara;
                break;
            case Parts.Booster:
                _boosterList.Add(armor);
                _boosterDefense += armor.ArmorDefPara;
                _boosterHP += armor.ArmorHpPara;
                _boosterWeight += (int)armor.ArmorWeightPara;
                break;
            default:
                break;
        }
    }
    #endregion

    #region PartsPurge
    public void PartsPurge(Parts parts)
    {
        switch (parts)
        {
            case Parts.Body:
                for (int i = 0; i < _bodyList.Count; i++)
                {
                    _bodyList[i].transform.parent = null;
                }
                _bodyList.Clear();
                _bodyDefense = 0;
                _totalWeight -= _bodyWeight;
                _bodyWeight = 0;
                break;
            case Parts.RightArm:
                for (int i = 0; i < _rightArmList.Count; i++)
                {
                    _rightArmList[i].transform.parent = null;
                }
                _rightArmList.Clear();
                _rightArmDefense = 0;
                _totalWeight -= _rightArmWeight;
                _rightArmWeight = 0;
                break;
            case Parts.LeftArm:
                for (int i = 0; i < _leftArmList.Count; i++)
                {
                    _leftArmList[i].transform.parent = null;
                }
                _leftArmList.Clear();
                _leftArmDefense = 0;
                _totalWeight -= _leftArmWeight;
                _leftArmWeight = 0;
                break;
            case Parts.Leg:
                for (int i = 0; i < _legList.Count; i++)
                {
                    _legList[i].transform.parent = null;
                }
                _legList.Clear();
                _legDefense = 0;
                _totalWeight -= _legWeight;
                _legWeight = 0;
                break;
            case Parts.Booster:
                for (int i = 0; i < _boosterList.Count; i++)
                {
                    _boosterList[i].transform.parent = null;
                }
                _boosterList.Clear();
                _boosterDefense = 0;
                _totalWeight -= _boosterWeight;
                _boosterWeight = 0;
                break;
            default:
                break;
        }
    }
    #endregion

    #region FullParge
    public void FullParge()
    {
        for (int i = 0; i < _allPartsList.Count; i++)
        {
            PartsPurge(_allPartsList[i]);
        }
    }
    #endregion

    #region PartsDamage
    public void PartsDamage(int attackPower, Parts parts)
    {
        switch (parts)
        {
            case Parts.Body:
                //パーツに何もついてなければ本体にダメージが入る
                if (_bodyHp <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                attackPower -= _bodyDefense;
                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _bodyHp -= attackPower;
                if (_bodyHp <= 0)
                {
                    _bodyHp = 0;
                    PartsPurge(parts);
                }
                Damage(attackPower);
                break;
            case Parts.RightArm:
                //パーツに何もついてなければ本体にダメージが入る
                if (_rightArmHp <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _rightArmHp -= attackPower;
                if (_rightArmHp <= 0)
                {
                    _rightArmHp = 0;
                    PartsPurge(parts);
                }
                break;
            case Parts.LeftArm:
                //パーツに何もついてなければ本体にダメージが入る
                if (_leftArmHp <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _leftArmHp -= attackPower;
                if (_leftArmHp <= 0)
                {
                    _leftArmHp = 0;
                    PartsPurge(parts);
                }
                break;
            case Parts.Leg:
                //パーツに何もついてなければ本体にダメージが入る
                if (_legHp <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _legHp -= attackPower;
                if (_legHp <= 0)
                {
                    _legHp = 0;
                    PartsPurge(parts);
                }
                break;
            case Parts.Booster:
                //パーツに何もついてなければ本体にダメージが入る
                if (_boosterHP <= 0)
                {
                    Damage(attackPower);
                    break;
                }

                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _boosterHP -= attackPower;
                if (_boosterHP <= 0)
                {
                    _boosterHP = 0;
                    PartsPurge(parts);
                }
                break;
            default:
                break;
        }

    }
    #endregion

    #region Damage
    /// <summary>
    /// Damageを受けたときの処理
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    public virtual void Damage(int attackPower)
    {
        attackPower -= _defense;
        if (attackPower <= 1)
        {
            attackPower = 1;
        }

        _hp -= attackPower;
        if(_hp <= 0)
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
}
