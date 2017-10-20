using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaParameter : MonoBehaviour
{

    [Header("キャラクターの基本のパラメーター")]
    #region Hp
    public int _hp = 1000;
    protected int _bodyHp = 0;
    protected int _rightArmHp = 0;
    protected int _leftArmHp = 0;
    protected int _legHp = 0;
    protected int _boosterHP = 0;
    #endregion

    #region Defense
    public int _defense = 0;
    protected int _bodyDefense = 0;
    protected int _rightArmDefense = 0;
    protected int _leftArmDefense = 0;
    protected int _legDefense = 0;
    protected int _boosterDefense = 0;
    #endregion

    #region Weight
    protected int _totalWeight = 0;
    protected int _maxWeight = 0;
    protected int _bodyWeight = 0;
    protected int _rightArmWeight = 0;
    protected int _leftArmWeight = 0;
    protected int _legWeight = 0;
    protected int _boosterWeight = 0;
    #endregion
    public int _attack = 1;
    public float _speed = 1.0f;

}
