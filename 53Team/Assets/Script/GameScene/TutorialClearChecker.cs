using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class TutorialClearChecker : MonoBehaviour {
    private TutorialManager _tutorialManager = null;
    [SerializeField] private Player _player = null;
    [SerializeField] private Enemy_Standard _TutorialEnemy = null;
    [SerializeField] private Enemy_Standard _TutorialSecondEnemy = null;
    [SerializeField] private GameObject _TutorialPargeEnemysParent = null;
    [SerializeField] private GameObject[] _TutorialPargeEnemys = null;
    [SerializeField] private InputManager _inputManager = null;
    
    private int partsCount = 0;
    private int bodyPartsCount = 0;
    private bool fullParge = false;
    private bool partsParge = false;

    // Use this for initialization
    void Start () {
        _tutorialManager = this.GetComponent<TutorialManager>();
        for (int i = 0; i < _player._allPartsList.Count; i++)
        {
            partsCount += _player.GetPartsList(_player._allPartsList[i]).Count;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
		
	}

    public bool AttackCheck()
    {
        if (_TutorialEnemy == null)
        {
            return true;
        }
        return false;
    }

    public bool PartsAddCheck()
    {
        int count = 0;
        for (int i = 0; i < _player._allPartsList.Count; i++)
        {
            count += _player.GetPartsList(_player._allPartsList[i]).Count;
        }

        if(count > partsCount)
        {
            _TutorialSecondEnemy.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    public bool SecontEnemyAttack()
    {
        if (_TutorialSecondEnemy == null)
        {
            return true;
        }
        return false;
    }

    public bool BodyPartsAddCheck()
    {
        int count = 0;
        count = _player.GetPartsList(Player.Parts.Body).Count;
        if(count > bodyPartsCount)
        {
            _TutorialPargeEnemysParent.SetActive(true);
            return true;
        }
        return false;
    }

    public bool FullPargeCheck()
    {
        int count = 0;
        for (int i = 0; i < _player._allPartsList.Count; i++)
        {
            count += _player.GetPartsList(_player._allPartsList[i]).Count;
        }
        if (count==0)
        {
            //for(int i = 0; i < _TutorialPargeEnemys.Length; i++)
            //{
            //    if(_TutorialPargeEnemys[i] == null)
            //    {
            //        fullParge = true;
            //        _TutorialPargeEnemysParent.SetActive(false);
            //        return true;
            //    }
            //}
            StartCoroutine(TutorialEnemysRemove());
            return true;
        }
        
        return false;
    }

    IEnumerator TutorialEnemysRemove()
    {
        yield return new WaitForSeconds(1.5f);
        fullParge = true;
        _TutorialPargeEnemysParent.SetActive(false);
    }

    public bool PartsPargeCheck()
    {
        if ((!_player._rightArmParge || !_player._leftArmParge || !_player._legParge) && 
            !partsParge)
        {
            partsParge = true;
            return true;
        }
        return false;
    }
}
