using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexaManager : MonoBehaviour
{
    public int id = 0;                        //置かれているかどうかを判断するためのＩＤ
    public Vector2 pos = new Vector2(0f, 0f); //座標

    [Header("スプライト")]
    public Sprite normalJewel;
    public Sprite rainbowJewel;

    void Update()
    {
        //x 2.26f, y 1.31f
        RectTransform hexa = this.GetComponent<RectTransform>();
        //座標の設定
        Vector2 finalPos = new Vector2(2.26f*pos.x, 2.62f*pos.y);
        finalPos.y += 1.31f*pos.x;
        hexa.anchoredPosition = finalPos;
        //SpriteRendererの取得
        SpriteRenderer spR = this.GetComponent<SpriteRenderer>();
        //変数colorValの宣言
        Color colorVal;

        if (id > 0){
            //スケールを85%に設定（置かれているジュエル）
            colorVal = GameManager.getColor(id-1);
            hexa.localScale = new Vector3(0.85f, 0.85f, 1f);
        } else {
            //スケールを60%に設定（置かれていない場合）
            colorVal = new Color(0.4f, 0.4f, 0.4f, 1f); //黒色
            hexa.localScale = new Vector3(0.6f, 0.6f, 1f);
        }
        spR.color = colorVal;

        //スプライト
        spR.sprite = (id == 6) ? rainbowJewel : normalJewel;
    }
}
