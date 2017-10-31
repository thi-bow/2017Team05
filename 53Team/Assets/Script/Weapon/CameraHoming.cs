using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHoming : MonoBehaviour {

    [SerializeField] private GameObject fpsCamera;
    [SerializeField] private GameObject tpsCamera;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    void LateUpdate()
    {
        float x = Mathf.Lerp(this.transform.rotation.x, fpsCamera.transform.localEulerAngles.x, Time.deltaTime * 200.0f);
        float y = Mathf.Lerp(this.transform.rotation.y, fpsCamera.transform.localEulerAngles.y, Time.deltaTime * 200.0f);
        this.transform.rotation = Quaternion.Euler(x, y, 0);
    }
}
