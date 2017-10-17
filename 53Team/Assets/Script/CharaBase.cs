using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBase : CharaParameter
{
    public enum Parts
    {
        Head = 0,
        Body,
        RightArm,
        LeftArm,
        RihtLeg,
        LeftLeg,
        Booster,
    }

    #region 装備
    [Header("キャラクターベース")]
    [SerializeField] private List<Armor> _headList = new List<Armor>();
    [SerializeField] private List<Armor> _bodyList = new List<Armor>();
    [SerializeField] private List<Armor> _rightArmList = new List<Armor>();
    [SerializeField] private List<Armor> _leftArmList = new List<Armor>();
    [SerializeField] private List<Armor> _rightLegList = new List<Armor>();
    [SerializeField] private List<Armor> _leftLegList = new List<Armor>();
    [SerializeField] private List<Armor> _boosterList = new List<Armor>();
    List<List<Armor>> _allArmorList = new List<List<Armor>>(); 
    private int partsMax = 5;
    private Parts _parts;
    #endregion

    // Use this for initialization
    protected virtual void Start ()
    {
        _allArmorList=new List<List<Armor>> { _headList, _bodyList, _rightArmList, _leftArmList, _rightLegList, _leftLegList, _boosterList };
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
    }

    #region PartsAdd
    public void PartsAdd(Parts parts, Armor armor)
    {
        switch (parts)
        {
            case Parts.Head:
                if(_headList.Count >= partsMax)
                {
                    _headDefense += armor._armorDefense;
                    _headHp += armor._armorHp;
                    break;
                }
                _headList.Add(armor);
                break;
            case Parts.Body:
                if (_bodyList.Count >= partsMax)
                {
                    _bodyDefense += armor._armorDefense;
                    _bodyHp += armor._armorHp;
                    break;
                }
                _bodyList.Add(armor);
                break;
            case Parts.RightArm:
                if (_rightArmList.Count >= partsMax)
                {
                    _rightArmDefense += armor._armorDefense;
                    _rightArmHp += armor._armorHp;
                    break;
                }
                _rightArmList.Add(armor);
                break;
            case Parts.LeftArm:
                if (_leftArmList.Count >= partsMax)
                {
                    _leftArmDefense += armor._armorDefense;
                    _leftArmHp += armor._armorHp;
                    break;
                }
                _leftArmList.Add(armor);
                break;
            case Parts.RihtLeg:
                if (_rightLegList.Count >= partsMax)
                {
                    _rightLegDefense += armor._armorDefense;
                    _rightLegHp += armor._armorHp;
                    break;
                }
                _rightLegList.Add(armor);
                break;
            case Parts.LeftLeg:
                if (_leftLegList.Count >= partsMax)
                {
                    _leftLegDefense += armor._armorDefense;
                    _leftLegHP += armor._armorHp;
                    break;
                }
                _leftLegList.Add(armor);
                break;
            case Parts.Booster:
                if (_boosterList.Count >= partsMax)
                {
                    _boosterDefense += armor._armorDefense;
                    _boosterHP += armor._armorHp;
                    break;
                }
                _boosterList.Add(armor);
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
            case Parts.Head:
                for(int i = 0; i < _headList.Count; i++)
                {
                    _headList[i].transform.parent = null;
                }
                _headList.Clear();
                _headDefense = 0;
                break;
            case Parts.Body:
                for (int i = 0; i < _bodyList.Count; i++)
                {
                    _bodyList[i].transform.parent = null;
                }
                _bodyList.Clear();
                _bodyDefense = 0;
                break;
            case Parts.RightArm:
                for (int i = 0; i < _rightArmList.Count; i++)
                {
                    _rightArmList[i].transform.parent = null;
                }
                _rightArmList.Clear();
                _rightArmDefense = 0;
                break;
            case Parts.LeftArm:
                for (int i = 0; i < _leftArmList.Count; i++)
                {
                    _leftArmList[i].transform.parent = null;
                }
                _leftArmList.Clear();
                _leftArmDefense = 0;
                break;
            case Parts.RihtLeg:
                for (int i = 0; i < _rightLegList.Count; i++)
                {
                    _rightLegList[i].transform.parent = null;
                }
                _rightLegList.Clear();
                _rightLegDefense = 0;
                break;
            case Parts.LeftLeg:
                for (int i = 0; i < _leftLegList.Count; i++)
                {
                    _leftLegList[i].transform.parent = null;
                }
                _leftLegList.Clear();
                _leftLegDefense = 0;
                break;
            case Parts.Booster:
                for (int i = 0; i < _boosterList.Count; i++)
                {
                    _boosterList[i].transform.parent = null;
                }
                _boosterList.Clear();
                _boosterDefense = 0;
                break;
            default:
                break;
        }
    }
    #endregion

    #region FullParge
    public void FullParge()
    {
        for (int i = 0; i < _allArmorList.Count; i++)
        {
            for(int j = 0; j <_allArmorList[i].Count; j++)
            {
                _allArmorList[i][j].transform.parent = null;
            }
            _allArmorList[i].Clear();
        }
    }
    #endregion

    #region PartsDamage
    public void PartsDamage(int attackPower, Parts parts)
    {
        switch (parts)
        {
            case Parts.Head:
                attackPower -= _headDefense;
                if(attackPower <= 1)
                {
                    attackPower = 1;
                }
                _headHp -= attackPower;
                if(_headHp <= 0)
                {
                    _headHp = 0;
                    PartsPurge(parts);
                }
                break;
            case Parts.Body:
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
                break;
            case Parts.RightArm:
                attackPower -= _rightArmDefense;
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
                attackPower -= _leftArmDefense;
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
            case Parts.RihtLeg:
                attackPower -= _rightLegDefense;
                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _rightLegHp -= attackPower;
                if (_rightLegHp <= 0)
                {
                    _rightLegHp = 0;
                    PartsPurge(parts);
                }
                break;
            case Parts.LeftLeg:
                attackPower -= _leftLegDefense;
                if (attackPower <= 1)
                {
                    attackPower = 1;
                }
                _leftLegHP -= attackPower;
                if (_leftLegHP <= 0)
                {
                    _leftLegHP = 0;
                    PartsPurge(parts);
                }
                break;
            case Parts.Booster:
                attackPower -= _boosterDefense;
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

        Damage(attackPower, true);
    }
    #endregion

    #region Damage
    /// <summary>
    /// Damageを受けたときの処理
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    public virtual void Damage(int attackPower, bool partsDamege = false)
    {
        if (!partsDamege)
        {
            attackPower -= _defense;
            if (attackPower <= 1)
            {
                attackPower = 1;
            }
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
}
