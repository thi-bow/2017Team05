using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
    [SerializeField] private GameObject _textUI = null;
    [SerializeField] private Text _text = null;
    [SerializeField] private Text _guide = null;
    private Action _endAction = null;
    public bool _skipFlag = false;
    private bool _textOff = false;
    public float _writeTime = 0.01f;
    public int _writeNumber = 0;
    private int _tutorialNumber = 0;

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
            if(_textOff)
            {
                //TextOff();
            }
        }
    }

    string[][] TutorialTex = new string[][]
    {
        new string[]{
            "こんにちは。チュートリアル先生です\nさっそくですが道なりに進むと敵だ1体います。",
            "画面中央の点を敵に当て 「RT」 を押して敵を攻撃し\n倒してみてください" },
        new string[]{
            "敵からパーツが落ちましたね。\n敵から落ちるパーツは装着するとこができます",
            "「十字右」を押して右腕に装着してみましょう!" },
        new string[]{
            "パーツを右腕に装着することができました。\nパーツを装着することによってステータスをあげることができます。",
            "では実際に今手に入れた武器で敵を倒してみましょう!"},

        new string[]{
            "今度はアーマーが落ちましたね。\n武器とは違いアーマーとブースター近づくと自動的に装備をします。",
            "実際に近づいて装備をしてみましょう。", },
        new string[]{
            "装備することができましたね。\nでは次に必殺技をお教えしますね。",
            "十字ボタンを長押しすると装備が外れてしまう代わりに必殺技を使うことができます。",
            "「十字上」を長押ししてみてください。"},
        new string[]{
            "パージできましたね。\nパージをすると装着している装備によって異なる必殺技を放つことができます。",
            "また「十字上」以外にも「十字右」「十字左」「十字下」を押すと特殊な必殺技を放つことができます",
            "お疲れ様です。\nこれにてチュートリアルは終了です",
            "この進の先にはたくさんの敵が待ち構えているので、こちらで基本装備を準備しこの先に配置しときました。",
            "その装備を基本に倒しつくしましょう!!",
            "それでは頑張ってください"},
    };

    string[] GuideText = new string[]
    {
        "「RT」で攻撃して敵を倒す",
        "装備に近づいて「十字右」で装着",
        "新しい敵を倒す",
        "パーツに近づいてアーマーを装備",
        "「十字上」で必殺技",
        "でてはいけない"
    };

    public IEnumerator TextWrite(int tutorialNumber, Action endAction = null)
    {
        if (!_textUI.activeInHierarchy)
        {
            _textUI.SetActive(true);
        }
        if(_guide.gameObject.activeInHierarchy)
        {
            _guide.gameObject.SetActive(false);
        }
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
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(TextWrite(tutorialNumber, endAction));
        }
        else
        {
            _writeNumber = 0;
            _textOff = true;
            yield return new WaitForSeconds(1.5f);
            if (endAction != null)
            {
                _endAction = endAction;
            }
            TextOff();
        }
    }

    private void TextOff()
    {
        _textOff = false;
        _textUI.SetActive(false);
        TutorialManager.tutorialMove = true;
        if (_endAction != null)
        {
            _endAction();
        }
        else
        {
            _guide.gameObject.SetActive(true);
            _guide.text = GuideText[_tutorialNumber];
            _tutorialNumber++;
        }
    }
}
