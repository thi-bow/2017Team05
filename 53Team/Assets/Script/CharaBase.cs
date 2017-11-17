using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharaParameter
{

    [Header("キャラクターのHP")]
    #region Hp
    public int _maxHP = 1000;
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
    #region Attack
    public int _rightAttack = 0;
    public int _leftAttack = 0;
    public int _legAttack = 0;
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

    [Header("キャラクターのWeight")]
    #region Lv
    public int _bodyLevel = 1;
    public int _rightArmLevel = 1;
    public int _leftArmLevel = 1;
    public int _legLevel = 1;
    public int _boosterLevel = 1;
    #endregion

    [Space(10)]
    public int _attack = 1;
    [Space(10)]
    public float _speed = 1.0f;


    [Header("キャラクターの装備見た目変更する最低個数")]
    public int _rightArm_BorderNumber = 5;
    public int _leftArm_BorderNumber = 5;
    public int _leg_BorderNumber = 5;

    [Header("キャラクターの装備見た目を切り替える個数")]
    public int _rightArm_SwitchNumber = 5;
    public int _leftArm_SwitchNumber = 5;
    public int _leg_SwitchNumber = 5;

    [System.NonSerialized] public Weapon.Attack_State _rightArm_AttackState = Weapon.Attack_State.NULL;
    [System.NonSerialized] public Weapon.Attack_State _leftArm_AttackState = Weapon.Attack_State.NULL;
    [System.NonSerialized] public Weapon.Attack_State _leg_AttackState = Weapon.Attack_State.NULL;
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
    [SerializeField] protected List<Armor> _legPartsPair = new List<Armor>(); //両脚に着けるために、複製したアームを入れるリスト
    List<Parts> _allPartsList = new List<Parts>(); 
    private int partsMax = 5;
    private Parts _parts;

    protected bool _fullParge = true;
    protected bool _rightArmParge = true;
    protected bool _leftArmParge = true;
    protected bool _legParge = true;


    [Space(10)]
    [SerializeField] private GameObject[] _partsLocation;
    [SerializeField] private GameObject[] _specialWepon_Shot;
    [SerializeField] private GameObject[] _specialWepon_Approach;

    protected bool _rightArmStrike = false;
    protected bool _leftArmStrike = false;
    protected bool _legStrike = false;
    [SerializeField] private float _rightStrikeCoolTime = 3.0f;
    private float _rightStrileCoolCount = 0.0f;
    [SerializeField] private float _leftStrikeCoolTime = 3.0f;
    private float _leftStrileCoolCount = 0.0f;
    [SerializeField] private float _legStrikeCoolTime = 3.0f;
    private float _legStrileCoolCount = 0.0f;
    #endregion

    protected Action _deadAction = null;


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
        if(_rightArmStrike)
        {
            _rightStrileCoolCount += Time.deltaTime;
            if(_rightStrileCoolCount >= _rightStrikeCoolTime)
            {
                _rightStrileCoolCount = 0.0f;
                _rightArmStrike = false;
            }
        }
        if (_leftArmStrike)
        {
            _legStrileCoolCount += Time.deltaTime;
            if (_legStrileCoolCount >= _legStrikeCoolTime)
            {
                _legStrileCoolCount = 0.0f;
                _leftArmStrike = false;
            }
        }
        if (_legStrike)
        {
            _legStrileCoolCount += Time.deltaTime;
            if (_legStrileCoolCount >= _legStrikeCoolTime)
            {
                _legStrileCoolCount = 0.0f;
                _legStrike = false;
            }
        }
    }

    #region GetPartsList
    protected List<Armor> GetPartsList(Parts partsCheck)
    {
        List<Armor> partsList = new List<Armor>();
        switch (partsCheck)
        {
            case Parts.Body:
                partsList = BodyArmorList;
                break;
            case Parts.RightArm:
                partsList = RightArmArmorList;
                break;
            case Parts.LeftArm:
                partsList = LeftArmArmorList;
                break;
            case Parts.Leg:
                partsList = LegArmorList;
                break;
            case Parts.Booster:
                partsList = BoosterArmorList;
                break;
            default:
                break;
        }
        return partsList;
    }
    #endregion

    //パーツの装着
    #region PartsAdd
    public void PartsAdd(Parts parts, Armor armor)
    {
        if (_charaPara._totalWeight >= _charaPara._maxWeight)
        {
            return;
        }
        armor.GetComponent<BoxCollider>().enabled = false;
        int _shootNumber = 0;
        switch (parts)
        {
            case Parts.Body:
                _bodyList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._bodyDefense += armor.ArmorDefPara;
                _charaPara._bodyHp += armor.ArmorHpPara;
                _charaPara._bodyWeight += armor.ArmorWeightPara;
                armor.gameObject.GetComponent<BoxCollider>().enabled = false;
                Destroy(armor.gameObject.GetComponent<Rigidbody>());
                armor.gameObject.transform.SetParent(_partsLocation[0].transform);
                armor.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                armor.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                PartsLevelChenge(_bodyList.Count, out _charaPara._bodyLevel);
                
                break;
            case Parts.RightArm:
                _rightArmList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._rightArmDefense += armor.ArmorDefPara;
                _charaPara._rightArmHp += armor.ArmorHpPara;
                _charaPara._rightArmWeight += armor.ArmorWeightPara;
                armor.gameObject.GetComponent<BoxCollider>().enabled = false;
                Destroy(armor.gameObject.GetComponent<Rigidbody>());
                if (armor.GetComponent<Weapon>() != null)
                {
                    _charaPara._rightAttack += armor.GetComponent<Weapon>().atk;
                }
                armor.gameObject.transform.SetParent(_partsLocation[1].transform);
                armor.transform.localPosition = PartsAddPara.PlayerRightArmPosition[_rightArmList.Count - 1];
                armor.transform.localRotation = Quaternion.Euler(PartsAddPara.PlayerRightArmRotation[_rightArmList.Count - 1]);

                if(_rightArmList.Count < _charaPara._rightArm_BorderNumber)
                {
                    break;
                }
                //右腕が近接攻撃特化か、遠距離攻撃特化か見極める
                for(int i = 0; i < _rightArmList.Count; i++)
                {
                    if (_rightArmList[i].GetComponent<Weapon>() != null && _rightArmList[i].GetComponent<Weapon>().state == Weapon.Attack_State.shooting)
                    {
                        _shootNumber++;
                    }
                    else if (_rightArmList[i].GetComponent<Weapon>() != null && _rightArmList[i].GetComponent<Weapon>().state == Weapon.Attack_State.approach)
                    {
                        _shootNumber--;
                    }
                }
                if(_shootNumber >= _charaPara._rightArm_SwitchNumber)
                {
                    if (_charaPara._rightArm_AttackState != Weapon.Attack_State.shooting)
                    {
                        print("右腕を遠距離攻撃に切り替えた");
                        //遠距離特殊武器のActiveをtrueにし、近距離攻撃のActiveをfalseにする
                        _specialWepon_Shot[0].SetActive(true);
                        _specialWepon_Approach[0].SetActive(false);
                        //
                        _partsLocation[1].gameObject.SetActive(false);
                        _charaPara._rightArm_AttackState = Weapon.Attack_State.shooting;
                    }
                }
                else if(_shootNumber <= -_charaPara._rightArm_SwitchNumber)
                {
                    if (_charaPara._rightArm_AttackState != Weapon.Attack_State.approach)
                    {
                        print("右腕を近距離攻撃に切り替えた");
                        //近距離特殊武器のActiveをtrueにし、遠距離攻撃のActiveをfalseにする
                        _specialWepon_Shot[0].SetActive(false);
                        _specialWepon_Approach[0].SetActive(true);
                        //
                        _partsLocation[1].gameObject.SetActive(false);
                        _charaPara._rightArm_AttackState = Weapon.Attack_State.approach;
                    }
                }
                else
                {
                    //特殊状態から切り替えるときは、武器のActiveがfalseになったいるためtrueにする
                    if (_charaPara._rightArm_AttackState != Weapon.Attack_State.NULL)
                    {
                        print("右腕をガラクタがくっついている状態に切り替えた");
                        //近距離特殊武器と遠距離攻撃のActiveをfalseにする
                        _specialWepon_Shot[0].SetActive(false);
                        _specialWepon_Approach[0].SetActive(false);
                        //
                        _partsLocation[1].gameObject.SetActive(true);
                        _charaPara._rightArm_AttackState = Weapon.Attack_State.NULL;
                    }
                }

                PartsLevelChenge(_rightArmList.Count, out _charaPara._rightArmLevel);
                _rightArmParge = true;
                break;
            case Parts.LeftArm:
                _leftArmList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._leftArmDefense += armor.ArmorDefPara;
                _charaPara._leftArmHp += armor.ArmorHpPara;
                _charaPara._leftArmWeight += armor.ArmorWeightPara;
                armor.gameObject.GetComponent<BoxCollider>().enabled = false;
                Destroy(armor.gameObject.GetComponent<Rigidbody>());
                if (armor.GetComponent<Weapon>() != null)
                {
                    _charaPara._leftAttack += armor.GetComponent<Weapon>().atk;
                }
                armor.gameObject.transform.SetParent(_partsLocation[2].transform);
                armor.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                
                if (_leftArmList.Count < _charaPara._leftArm_BorderNumber)
                {
                    break;
                }
                //右腕が近接攻撃特化か、遠距離攻撃特化か見極める
                for (int i = 0; i < _leftArmList.Count; i++)
                {
                    if (_leftArmList[i].GetComponent<Weapon>() != null && _leftArmList[i].GetComponent<Weapon>().state == Weapon.Attack_State.shooting)
                    {
                        _shootNumber++;
                    }
                    else if (_leftArmList[i].GetComponent<Weapon>() != null && _leftArmList[i].GetComponent<Weapon>().state == Weapon.Attack_State.approach)
                    {
                        _shootNumber--;
                    }
                }
                if (_shootNumber >= _charaPara._leftArm_SwitchNumber)
                {
                    print("左腕を遠距離攻撃に切り替えた");
                    //遠距離特殊武器のActiveをtrueにし、近距離攻撃のActiveをfalseにする
                    _specialWepon_Shot[1].SetActive(true);
                    _specialWepon_Approach[1].SetActive(false);
                    //
                    _partsLocation[2].gameObject.SetActive(false);
                    _charaPara._leftArm_AttackState = Weapon.Attack_State.shooting;
                }
                else if (_shootNumber <= -_charaPara._leftArm_SwitchNumber)
                {
                    print("左腕を近距離攻撃に切り替えた");
                    //近距離特殊武器のActiveをtrueにし、遠距離攻撃のActiveをfalseにする
                    _specialWepon_Shot[1].SetActive(false);
                    _specialWepon_Approach[1].SetActive(true);
                    //
                    _partsLocation[2].gameObject.SetActive(false);
                    _charaPara._leftArm_AttackState = Weapon.Attack_State.approach;
                }
                else
                {
                    //特殊状態から切り替えるときは、武器のActiveがfalseになったいるためtrueにする
                    if(_charaPara._leg_AttackState != Weapon.Attack_State.NULL)
                    {
                        print("左腕をガラクタがくっついている状態に切り替えた");
                        //近距離特殊武器と遠距離攻撃のActiveをfalseにする
                        _specialWepon_Shot[1].SetActive(false);
                        _specialWepon_Approach[1].SetActive(false);
                        //
                        _partsLocation[2].gameObject.SetActive(true);
                        _charaPara._leg_AttackState = Weapon.Attack_State.NULL;
                    }
                }

                PartsLevelChenge(_leftArmList.Count, out _charaPara._leftArmLevel);

                _leftArmParge = true;
                break;
            case Parts.Leg:
                _legList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._legDefense += armor.ArmorDefPara;
                _charaPara._legHp += armor.ArmorHpPara;
                _charaPara._legWeight += armor.ArmorWeightPara;
                armor.gameObject.GetComponent<BoxCollider>().enabled = false;
                Destroy(armor.gameObject.GetComponent<Rigidbody>());
                if (armor.GetComponent<Weapon>() != null)
                {
                    _charaPara._legAttack += armor.GetComponent<Weapon>().atk;
                }

                //足に装着する場合は、右足と左足両方に装着する
                Armor pair = Instantiate(armor);
                armor.gameObject.transform.SetParent(_partsLocation[3].transform);
                armor.transform.localPosition = PartsAddPara.PlayerRightLegPosition[_legList.Count - 1];
                armor.transform.localRotation = Quaternion.Euler(PartsAddPara.PlayerRightLegRotation[_legList.Count - 1]);
                pair.gameObject.transform.SetParent(_partsLocation[4].transform);
                pair.transform.localPosition = PartsAddPara.PlayerLeftLegPosition[_legList.Count - 1];
                pair.transform.localRotation = Quaternion.Euler(PartsAddPara.PlayerLeftLegRotation[_legList.Count - 1]);
                _legPartsPair.Add(pair);

                if (_legList.Count < _charaPara._leg_BorderNumber)
                {
                    break;
                }
                //脚が近接攻撃特化か、遠距離攻撃特化か見極める
                for (int i = 0; i < _leftArmList.Count; i++)
                {
                    if (_legList[i].GetComponent<Weapon>() != null && _legList[i].GetComponent<Weapon>().state == Weapon.Attack_State.shooting)
                    {
                        _shootNumber++;
                    }
                    else if (_legList[i].GetComponent<Weapon>() != null && _legList[i].GetComponent<Weapon>().state == Weapon.Attack_State.approach)
                    {
                        _shootNumber--;
                    }
                }
                if (_shootNumber >= _charaPara._leg_SwitchNumber)
                {
                    if (_charaPara._leg_AttackState != Weapon.Attack_State.shooting)
                    {
                        print("脚を遠距離攻撃に切り替えた");
                        //遠距離特殊武器のActiveをtrueにし、近距離攻撃のActiveをfalseにする
                        _specialWepon_Shot[2].SetActive(true);
                        _specialWepon_Approach[2].SetActive(false);
                        //
                        _partsLocation[3].gameObject.SetActive(false);
                        _partsLocation[4].gameObject.SetActive(false);
                        _charaPara._leg_AttackState = Weapon.Attack_State.shooting;
                    }
                }
                else if (_shootNumber <= -_charaPara._leg_SwitchNumber)
                {
                    if (_charaPara._leg_AttackState != Weapon.Attack_State.approach)
                    {
                        print("脚を近距離攻撃に切り替えた");
                        //近距離特殊武器のActiveをtrueにし、遠距離攻撃のActiveをfalseにする
                        _specialWepon_Shot[2].SetActive(false);
                        _specialWepon_Approach[2].SetActive(true);

                        //
                        _partsLocation[3].gameObject.SetActive(false);
                        _partsLocation[4].gameObject.SetActive(false);
                        _charaPara._leg_AttackState = Weapon.Attack_State.approach;
                    }
                }
                else
                {
                    if (_charaPara._leg_AttackState != Weapon.Attack_State.NULL)
                    {
                        print("脚をガラクタがくっついている状態に切り替えた");
                        //近距離特殊武器と遠距離攻撃のActiveをfalseにする
                        _specialWepon_Shot[2].SetActive(false);
                        _specialWepon_Approach[2].SetActive(false);
                        //
                        _partsLocation[3].gameObject.SetActive(true);
                        _partsLocation[4].gameObject.SetActive(true);
                        _charaPara._leg_AttackState = Weapon.Attack_State.NULL;
                    }
                }

                PartsLevelChenge(_legList.Count, out _charaPara._legLevel);

                _legParge = true;
                break;
            case Parts.Booster:
                _boosterList.Add(armor);
                //装備のパラメータをプレイヤーに上乗せする
                _charaPara._boosterDefense += armor.ArmorDefPara;
                _charaPara._boosterHp += armor.ArmorHpPara;
                _charaPara._boosterWeight += armor.ArmorWeightPara;
                armor.gameObject.transform.SetParent(_partsLocation[5].transform);
                armor.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                armor.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));

                PartsLevelChenge(_boosterList.Count, out _charaPara._boosterLevel);
                break;
            default:
                break;
        }
        _charaPara._totalWeight += armor.ArmorWeightPara;
        if (!_fullParge) _fullParge = true;
    }
    #endregion

    #region PartsLevelChenge
    private void PartsLevelChenge(int count, out int level)
    {
        int checkLevel = 0;
        if(count >= 8)
        {
            checkLevel = 3;
        }
        else if(count >= 3)
        {
            checkLevel = 2;
        }
        else
        {
            checkLevel = 1;
        }
        level = checkLevel;
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
                _charaPara._rightAttack = 0;
                _charaPara._totalWeight -= _charaPara._rightArmWeight;
                _charaPara._rightArmWeight = 0;
                _rightArmParge = false;
                if (_charaPara._rightArm_AttackState != Weapon.Attack_State.NULL)
                {
                    _specialWepon_Shot[0].SetActive(false);
                    _specialWepon_Approach[0].SetActive(false);
                    _partsLocation[1].SetActive(true);
                    _charaPara._rightArm_AttackState = Weapon.Attack_State.NULL;
                }
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
                _charaPara._leftAttack = 0;
                _charaPara._totalWeight -= _charaPara._leftArmWeight;
                _charaPara._leftArmWeight = 0;
                _leftArmParge = false;
                if (_charaPara._leftArm_AttackState != Weapon.Attack_State.NULL)
                {
                    _specialWepon_Shot[1].SetActive(false);
                    _specialWepon_Approach[1].SetActive(false);
                    _partsLocation[2].SetActive(true);
                    _charaPara._leftArm_AttackState = Weapon.Attack_State.NULL;
                }
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
                _charaPara._legAttack = 0;
                _charaPara._totalWeight -= _charaPara._legWeight;
                _charaPara._legWeight = 0;
                for(int i = 0; i < _legPartsPair.Count; i++)
                {
                    Destroy(_legPartsPair[i].gameObject);
                }
                _legPartsPair.Clear();
                _legParge = false;
                if (_charaPara._leg_AttackState != Weapon.Attack_State.NULL)
                {
                    _specialWepon_Shot[2].SetActive(false);
                    _specialWepon_Approach[2].SetActive(false);
                    _partsLocation[3].SetActive(true);
                    _partsLocation[4].SetActive(true);
                    _charaPara._leg_AttackState = Weapon.Attack_State.NULL;
                }
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
        //何も装備していなかったら何もしない
        if(_bodyList.Count + _rightArmList.Count + _leftArmList.Count + _legList.Count + _boosterList.Count <= 0)
        {
            return;
        }
        _fullParge = false;
        if (action != null)
        {
            action();
        }
        for (int i = 0; i < _allPartsList.Count; i++)
        {
            BrowOffParge(_allPartsList[i]);
        }

        //デバッグ中は、フルパージが終了したらいつでもフルパージできるようにする(パージ処理が全て慣性したらこの処理を消す)
        _fullParge = false;
    }
    #endregion

    #region BrowOffParge
    public void BrowOffParge(Parts parts)
    {
        PartsPurge(parts, () => {
            for (int i = 0; i < GetPartsList(parts).Count; i++)
            {
                GetPartsList(parts)[i].gameObject.AddComponent<Rigidbody>();
                float randx = UnityEngine.Random.Range(-20, 20);
                float randy = UnityEngine.Random.Range(0, 20);
                float randz = UnityEngine.Random.Range(-20, 20);
                GetPartsList(parts)[i].GetComponent<Rigidbody>().velocity = new Vector3(randx, randy, randz);
                Destroy(GetPartsList(parts)[i].gameObject, 2.0f);
            }
            if (parts == Parts.Leg)
            {
                for (int i = 0; i < _legPartsPair.Count; i++)
                {
                    _legPartsPair[i].gameObject.AddComponent<Rigidbody>();
                    float randx = UnityEngine.Random.Range(-20, 20);
                    float randy = UnityEngine.Random.Range(0, 20);
                    float randz = UnityEngine.Random.Range(-20, 20);
                    _legPartsPair[i].GetComponent<Rigidbody>().velocity = new Vector3(randx, randy, randz);
                    Destroy(_legPartsPair[i].gameObject, 2.0f);
                }
            }

        });
    }
    #endregion

    #region PargeAttack
    public void PargeAttack(Parts parts)
    {
        switch (parts)
        {
            case Parts.Body:
                _partsLocation[0].GetComponent<BoxCollider>().enabled = true;
                _partsLocation[0].transform.parent = null;
                break;
            case Parts.RightArm:
                _partsLocation[1].GetComponent<BoxCollider>().enabled = true;
                _partsLocation[1].transform.parent = null;
                break;
            case Parts.LeftArm:
                _partsLocation[2].GetComponent<BoxCollider>().enabled = true;
                _partsLocation[2].transform.parent = null;
                break;
            case Parts.Leg:
                _partsLocation[3].GetComponent<BoxCollider>().enabled = true;
                _partsLocation[4].GetComponent<BoxCollider>().enabled = true;
                _partsLocation[3].transform.parent = null;
                _partsLocation[4].transform.parent = null;
                break;
            case Parts.Booster:;
                _partsLocation[5].GetComponent<BoxCollider>().enabled = true;
                _partsLocation[5].transform.parent = null;
                break;
            default:
                break;
        }
    }
    #endregion

    //右腕の射撃攻撃
    #region RightArmtShot
    protected void RightArmtShot()
    {
        if (_rightArmList.Count <= 0) return;
        for(int i = 0; i < _rightArmList.Count; i++)
        {
            Weapon _wepon = null;
            _wepon = _rightArmList[i].GetComponent<Weapon>();
            //特殊射撃かどうか
            if (_charaPara._rightArm_AttackState == Weapon.Attack_State.shooting)
            {
                print("右腕の特殊射撃");
                continue;
            }
            else if(_charaPara._rightArm_AttackState == Weapon.Attack_State.NULL)
            {
                if (_wepon == null || _wepon.state != Weapon.Attack_State.shooting)
                {
                    print("右腕の" + i + "この装備には射撃がない");
                    continue;
                }
                _wepon.Shooting();
            }             
        }
    }
    #endregion

    //左腕の射撃攻撃
    #region LeftArmShot
    protected void LeftArmShot()
    {
        if (_leftArmList.Count <= 0) return;
        for (int i = 0; i < _leftArmList.Count; i++)
        {
            Weapon _wepon = null;
            _wepon = _leftArmList[i].GetComponent<Weapon>();
            if (_charaPara._leftArm_AttackState == Weapon.Attack_State.shooting)
            {
                print("左腕の特殊射撃");
                continue;
            }
            else if(_charaPara._leftArm_AttackState == Weapon.Attack_State.NULL)
            {
                if (_wepon == null || _wepon.state != Weapon.Attack_State.shooting)
                {
                    print("左腕の" + i + "この装備には射撃がない");
                    continue;
                }
                _wepon.Shooting();
            }
        }
    }
    #endregion

    //脚の攻撃
    #region LegShot
    protected void LegShot()
    {
        //脚が近接状態で、近接攻撃ができる状態なら近接攻撃をする
        if(_charaPara._leg_AttackState == Weapon.Attack_State.approach && !_legStrike)
        {
            //ここに近接攻撃を命令するものを作成する
            _legStrike = true;
            return;
        }

        //脚の攻撃状態が近接、もしくは武器を装着していなかったら攻撃はできない
        if (_legList.Count <= 0 || _charaPara._leg_AttackState == Weapon.Attack_State.approach) return;
        for (int i = 0; i < _legList.Count; i++)
        {
            Weapon _wepon = null;
            _wepon = _legList[i].GetComponent<Weapon>();
            if (_charaPara._leg_AttackState == Weapon.Attack_State.shooting)
            {
                print("脚の特殊射撃");
                continue;
            }
            else if (_charaPara._leg_AttackState == Weapon.Attack_State.NULL)
            {
                if (_wepon == null || _wepon.state != Weapon.Attack_State.shooting)
                {
                    print("脚の" + i + "この装備には射撃がない");
                    continue;
                }
                _wepon.Shooting();
            }
        }
    }
    #endregion

    //敵の右腕の射撃攻撃
    #region EnemyRightArmtShot
    protected void EnemyRightArmtShot(Ray ray)
    {
        if (_rightArmList.Count <= 0) return;
        for (int i = 0; i < _rightArmList.Count; i++)
        {
            Weapon _wepon = null;
            _wepon = _rightArmList[i].GetComponent<Weapon>();
            if (_charaPara._rightArm_AttackState == Weapon.Attack_State.shooting)
            {
                print("右腕の特殊射撃");
                continue;
            }
            else if (_charaPara._rightArm_AttackState == Weapon.Attack_State.NULL)
            {
                if (_wepon == null || _wepon.state != Weapon.Attack_State.shooting)
                {
                    print("右腕の" + i + "この装備には射撃がない");
                    continue;
                }
                _wepon.Shooting(ray);
            }
        }
    }
    #endregion

    //敵の左腕の射撃攻撃
    #region EnemyLeftArmShot
    protected void EnemyLeftArmShot(Ray ray)
    {
        if (_leftArmList.Count <= 0) return;
        for (int i = 0; i < _leftArmList.Count; i++)
        {
            if (_charaPara._leftArm_AttackState == Weapon.Attack_State.shooting)
            {
                print("左腕の特殊射撃");
                continue;
            }
            else if (_charaPara._leftArm_AttackState == Weapon.Attack_State.NULL)
            {
                Weapon _wepon = null;
                _wepon = _leftArmList[i].GetComponent<Weapon>();
                if (_wepon == null || _wepon.state != Weapon.Attack_State.shooting)
                {
                    print("左腕の" + i + "この装備には射撃がない");
                    continue;
                }
                _wepon.Shooting(ray);
            }
        }
    }
    #endregion

    //敵の脚の射撃攻撃
    #region EnemyLegShot
    protected void EnemyLegShot(Ray ray)
    {
        //脚が近接状態で、近接攻撃ができる状態なら近接攻撃をする
        if (_charaPara._leg_AttackState == Weapon.Attack_State.approach && !_legStrike)
        {
            //ここに近接攻撃を命令するものを作成する
            _legStrike = true;
            return;
        }

        //脚の攻撃状態が近接、もしくは武器を装着していなかったら攻撃はできない
        if (_legList.Count <= 0 || _charaPara._leg_AttackState == Weapon.Attack_State.approach) return;
        for (int i = 0; i < _legList.Count; i++)
        {
            if (_charaPara._leg_AttackState == Weapon.Attack_State.shooting)
            {
                print("脚の特殊射撃");
                continue;
            }
            else if (_charaPara._leg_AttackState == Weapon.Attack_State.NULL)
            {
                Weapon _wepon = null;
                _wepon = _legList[i].GetComponent<Weapon>();
                if (_wepon == null && _wepon.state != Weapon.Attack_State.shooting)
                {
                    print("脚の" + i + "この装備には射撃がない");
                    continue;
                }
                _wepon.Shooting(ray);
            }
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
        if (_deadAction != null)
        {
            _deadAction();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

}
