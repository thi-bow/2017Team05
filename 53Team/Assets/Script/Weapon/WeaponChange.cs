using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour {

    private WeaponList _WeaponList;

    public GameObject[] weapons;
    private int num;
    int add;

    // Use this for initialization
    void Start () {
        _WeaponList = this.gameObject.GetComponent<WeaponList>();
        for (int i = 0; i <_WeaponList.list.Count; i++)
        {
            if (_WeaponList.list[i].prefub != null)
            {
                _WeaponList.list[i].prefub.SetActive(false);
            }
        }

		/*for (int i =0; i < weapons.Length; i++) {
            if (weapons[i] != null) {
                weapons[i].SetActive(false);
            }
        }
        num = 0;*/
        //weapons[num].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void Change() {
        if (Input.GetKeyDown(KeyCode.C))
        {
            weapons[num].SetActive(false);
            num++;
            if (num >= weapons.Length)
            {
                num = 0;
            }

            weapons[num].SetActive(true);
        }
    }
}
