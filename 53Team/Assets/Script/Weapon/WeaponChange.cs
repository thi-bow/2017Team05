using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour {

    public GameObject[] weapons;

    private Weapon _Weapon;

    private int num;

    // Use this for initialization
    void Start () {
        _Weapon = this.GetComponent<Weapon>();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[num] != null)
            {
                weapons[i].SetActive(false);
            }
        }
        num = 0;
        weapons[num].SetActive(true);
        _Weapon.SetWeapon(weapons[num]);
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void Change()
    {
        weapons[num].SetActive(false);
        num++;
        if (num >= weapons.Length)
        {
            num = 0;
        }

        weapons[num].SetActive(true);
        _Weapon.SetWeapon(weapons[num]);
    }

    public GameObject GetWeapon()
    {
        return weapons[num];
        //return _WeaponList.list[num].prefub;
    }
}
