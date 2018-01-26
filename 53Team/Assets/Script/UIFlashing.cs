using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlashing : MonoBehaviour {
    private RectTransform _myRect = null;
    [SerializeField] private float _flashTime = 1.0f;

	// Use this for initialization
	void Start () {
        _myRect = this.GetComponent<RectTransform>();
        LeanTween.alpha(_myRect, 1.0f, _flashTime)
            .setEase(LeanTweenType.easeOutExpo)
            .setLoopPingPong();
		
	}
}
