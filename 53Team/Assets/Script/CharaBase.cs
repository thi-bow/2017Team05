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
    [SerializeField] private List<GameObject> _headList = new List<GameObject>();
    [SerializeField] private List<GameObject> _bodyList = new List<GameObject>();
    [SerializeField] private List<GameObject> _rightArmList = new List<GameObject>();
    [SerializeField] private List<GameObject> _leftArmList = new List<GameObject>();
    [SerializeField] private List<GameObject> _rightLegList = new List<GameObject>();
    [SerializeField] private List<GameObject> _leftLegList = new List<GameObject>();
    [SerializeField] private List<GameObject> _boosterList = new List<GameObject>();
    private int partsMax = 5;
    private Parts _parts;
    #endregion

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
    }

    #region PartsAdd
    public void PartsAdd(Parts parts, GameObject armor)
    {
        switch (parts)
        {
            case Parts.Head:
                if(_headList.Count >= partsMax)
                {
                    break;
                }
                _headList.Add(armor);
                break;
            case Parts.Body:
                if (_bodyList.Count >= partsMax)
                {
                    break;
                }
                _bodyList.Add(armor);
                break;
            case Parts.RightArm:
                if (_rightArmList.Count >= partsMax)
                {
                    break;
                }
                _rightArmList.Add(armor);
                break;
            case Parts.LeftArm:
                if (_leftArmList.Count >= partsMax)
                {
                    break;
                }
                _leftArmList.Add(armor);
                break;
            case Parts.RihtLeg:
                if (_rightLegList.Count >= partsMax)
                {
                    break;
                }
                _rightLegList.Add(armor);
                break;
            case Parts.LeftLeg:
                if (_leftLegList.Count >= partsMax)
                {
                    break;
                }
                _leftLegList.Add(armor);
                break;
            case Parts.Booster:
                if (_boosterList.Count >= partsMax)
                {
                    break;
                }
                _boosterList.Add(armor);
                break;
            default:
                break;
        }
    }

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
                break;
            case Parts.Body:
                for (int i = 0; i < _bodyList.Count; i++)
                {
                    _bodyList[i].transform.parent = null;
                }
                _bodyList.Clear();
                break;
            case Parts.RightArm:
                for (int i = 0; i < _rightArmList.Count; i++)
                {
                    _rightArmList[i].transform.parent = null;
                }
                _rightArmList.Clear();
                break;
            case Parts.LeftArm:
                for (int i = 0; i < _leftArmList.Count; i++)
                {
                    _leftArmList[i].transform.parent = null;
                }
                _leftArmList.Clear();
                break;
            case Parts.RihtLeg:
                for (int i = 0; i < _rightLegList.Count; i++)
                {
                    _rightLegList[i].transform.parent = null;
                }
                _rightLegList.Clear();
                break;
            case Parts.LeftLeg:
                for (int i = 0; i < _leftLegList.Count; i++)
                {
                    _leftLegList[i].transform.parent = null;
                }
                _leftLegList.Clear();
                break;
            case Parts.Booster:
                for (int i = 0; i < _boosterList.Count; i++)
                {
                    _boosterList[i].transform.parent = null;
                }
                _boosterList.Clear();
                break;
            default:
                break;
        }
    }
    #endregion

    /// <summary>
    /// Damageを受けたときの処理
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    public void Damage(int attackPower)
    {
        attackPower -= _totalDefense;
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

    /// <summary>
    /// キャラクターの死亡処理
    /// </summary>
    void Dead()
    {
        Destroy(this.gameObject);
    }
}
