using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour {

    public List<WeaponData> list = new List<WeaponData>();

    [System.SerializableAttribute]
    public class WeaponData
    {
        public GameObject prefub;
        public WeaponStatus.WeaponType Type;
        public int bullets;
        // 最高装填数
        public int maxBullets;
        // 威力
        public float atk;
        // 射程距離
        public float distance;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
