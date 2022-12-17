using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopUpManager : MonoBehaviour
{
    public RectTransform dynamicObj; //動的オブジェクト
    public Text value;               //値のテキスト

    void Awake()
    {
        dynamicObj.position = RectTransformUtility.WorldToScreenPoint (Camera.main, GameManager.ScorePopUpPos); //座標の設定
        value.text = (GameManager.ScorePopUpValue).ToString(); //テキストの設定
        GameManager.score += GameManager.ScorePopUpValue;      //加点
    }

    void DestroyObj()
    {
        //アニメーションが終わったら、破壊する。
        Destroy(gameObject);
    }
}
