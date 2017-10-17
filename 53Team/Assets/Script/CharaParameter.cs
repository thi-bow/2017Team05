using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaParameter : MonoBehaviour
{

    [Header("キャラクターの基本のパラメーター")]
    public int _hp = 1000;

    #region Defense
    public int _defense = 0;
    protected int _totalDefense;
    protected int _headDefense;
    protected int _bodyDefense;
    protected int _rightArmDefense;
    protected int _leftArmDefense;
    protected int _rightLegDefense;
    protected int _leftLegDefense;
    protected int _boosterDefense;
    #endregion
    public int _attack = 1;
    public float _speed = 1.0f;

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }


}
