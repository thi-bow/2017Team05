using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextManager : MonoBehaviour {

    //[SerializeField] private string[] partsAddText;
    //private int partsAddTextNumber = 0;
    //[SerializeField] private string[] attackText;
    //private int attackTexttNumber = 0;
    //[SerializeField] private string[] partsRobText;
    //private int partsRobTexttNumber = 0;
    //[SerializeField] private string[] pargeText;
    //private int pargeTexttNumber = 0;
    //[SerializeField] private string[] takeText;
    //private int takeTexttNumber = 0;
    //public static List<string[]> TutorialTex;
    [SerializeField] private Text _text = null;
    public bool _skipFlag = false;
    public float _writeTime = 0.01f;
    public int _writeNumber = 0;

    // Use this for initialization
    void Start() {
        //TutorialTex = new List<string[]>() { partsAddText, attackText, partsRobText, pargeText, takeText };

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            _skipFlag = true;
        }
    }

    string[][] TutorialTex = new string[][]
    {
        new string[]{"", "" },
        new string[]{"", "" },
        new string[]{"", "" },
        new string[]{"", "" },
        new string[]{"", "" },
        new string[]{"", "" },
    };

    public IEnumerator TextWrite(int tutorialNumber)
    {
        var writeText = TutorialTex[tutorialNumber][_writeNumber];
        _text.text = "";
        for (int i = 0; i < writeText.Length; i++)
        {
            if (_skipFlag) break;
            _text.text += writeText.Substring(i, 1);
            yield return new WaitForSeconds(_writeTime); //1文字ずつわずかに表示を遅らせる
        }

        if(_skipFlag)
        {
            _skipFlag = false;
            _text.text = writeText;
            yield return new WaitForSeconds(_writeTime);
        }
        if(_writeNumber < TutorialTex[tutorialNumber].Length - 1)
        {
            _writeNumber++;
            TextWrite(tutorialNumber);
        }
        else
        {
            _writeNumber = 0;
            TutorialManager.tutorialMove = true;
        }
    }
}
