using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PargeAttackCollider : MonoBehaviour {

    [SerializeField] float sizeUpspeed = 1.0f;
    bool _parge = false;
    int _attackPower = 1000;
    float _collderSize = 5.0f;
    float radius = 0.0f;
	
	// Update is called once per frame
	void Update ()
    {
        if (_parge)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, radius, transform.forward, out hit))
            {
                if (hit.collider.GetComponent<BoneCollide>() != null && hit.collider.tag != this.tag)
                {
                    Debug.Log(hit.collider.name + "：" + _attackPower);
                    hit.collider.gameObject.GetComponent<BoneCollide>().Damage(_attackPower, Weapon.Attack_State.approach);
                }
            }

            radius += Time.deltaTime * sizeUpspeed;
            if (radius >= _collderSize)
            {
                _parge = false;
                gameObject.SetActive(false);
                return;
            }
        }


    }

    public void PargeStart(int power, float collderSize)
    {
        radius = 0.5f;
        _attackPower = power;
        _collderSize = collderSize;
        _parge = true;
    }

    //private void OnTriggerStay(Collider other)
    //{

    //    if (other.gameObject.GetComponent<BoneCollide>() != null && other.tag != this.tag)
    //    {
    //        Debug.Log(other.gameObject.name + "ほげほげ");
    //        //other.gameObject.GetComponent<BoneCollide>().Damage(_attackPower, Weapon.Attack_State.NULL);
    //    }
    //}

    private void OnDrawGizmos()
    {

    }


}
