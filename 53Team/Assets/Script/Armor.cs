using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour {

    [Header("装備のパラメーター")]
    public int _armorHp = 0;
    public int _armorDefense = 0;
    public float _armorSpeed = 0.0f;
    public int _armorWeight = 0;
    [SerializeField] private CharaBase.Parts _parts = CharaBase.Parts.Body;
    public CharaBase.Parts GetParts
    {
        get { return _parts; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public int ArmorHpPara
    {
        get { return _armorHp; }
    }
    public int ArmorDefPara
    {
        get { return _armorDefense; }
    }

    public float ArmorSpeedPara
    {
        get { return _armorSpeed; }
    }

    public int ArmorWeightPara
    {
        get { return _armorWeight; }
    }
}
