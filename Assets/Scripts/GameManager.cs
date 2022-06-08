using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int score = 0;
    public static int hiScore = 10000;
    public static int jewels = 0;
    public static int level = 1;

    public static bool gameOver;
    public static int nextShapeId;                 //次の形のＩＤ
    public static int[] nextColorIds = new int[3]; //次の形の各色のＩＤ
    public static Vector2 ScorePopUpPos;           //スコア加算ポップアップの座標
    public static int ScorePopUpValue;             //スコア加算ポップアップの値

    public static int specialFill = 0;
    public static int specialMax = 50;

    void Start()
    {
        //初期設定
        score = 0;
        hiScore = 10000;
        jewels = 0;
        level = 1;
    }

    void Update()
    {
        /** システム **/
        //ハイスコア更新
        if (score > hiScore) hiScore = score;
        //レベルアップ（条件は仮、各30ジュエルでレベルアップ）
        if ((int)Math.Floor(jewels / 30f) == level) {
            GetComponent<HUDManager>().StartCoroutine("LevelUpDisplay");
            level++;
        }

        /** 上限・下限の設定 **/
        //スコア
        score = Mathf.Clamp(score, 0, 99999999);
        //ハイスコア
        hiScore = Mathf.Clamp(hiScore, 0, 99999999);
        //消したジュエルの数
        jewels = Mathf.Clamp(jewels, 0, 9999);
        //レベル
        level = Mathf.Clamp(level, 1, 99);
    }
    /* 次の形を指定 */
    public static void nextShape(){
        //色の範囲を指定
        int colorRange;
        if (level >= 30){
            //レベルが30以上
            colorRange = 4;
        } else if (level >= 15){
            //レベルが15以上
            colorRange = 3;
        } else{
            //それ以外
            colorRange = 2;
        }
        //形のIDを0~2までのランダムにする。
        nextShapeId = UnityEngine.Random.Range(0, 3);
        //色のIDを0~3までのランダムにする。
        for (int i = 0; i < nextColorIds.Length; i++){
            nextColorIds[i] = UnityEngine.Random.Range(0, colorRange);
        }
    }
    /* 色の取得 */
    public static Color getColor(int colorId){
        Color colorVal;
        switch (colorId){
            case 0:
            colorVal = new Color(1f, 0f, 0f, 1f); //赤色
            break;

            case 1:
            colorVal = new Color(1f, 1f, 0f, 1f); //黄色
            break;

            case 2:
            colorVal = new Color(0f, 1f, 0f, 1f); //黄緑色
            break;

            case 3:
            colorVal = new Color(0f, 1f, 1f, 1f); //水色
            break;

            default:
            colorVal = new Color(1f, 1f, 1f, 1f); //白色
            break;
        }
        return colorVal;
    }

    public static bool CheckForGameOver() {
        //StageManagerのhexasに影響しないよう、ダミーの配列を作る。
        int[,] dummyHexaIds = new int[9, 9];
        for (int x = 0; x < StageManager.hexas.GetLength(0); x++){
            for (int y = 0; y < StageManager.hexas.GetLength(1); y++){
                if (StageManager.hexas[x, y] == null){
                    //ステージ外の場合
                    dummyHexaIds[x, y] = -1;
                } else {
                    //ステージ内の場合
                    dummyHexaIds[x, y] = StageManager.hexas[x, y].id;
                }
            }
        }

        int emptyCount = 0;
        int maxEmptyCount = 0;

        for (int x = 0; x < dummyHexaIds.GetLength(0); x++) {
            for (int y = 0; y < dummyHexaIds.GetLength(1); y++){
                if (dummyHexaIds[x, y] == 0){
                    //ステージ外の場合
                    emptyCount = EmptyCount(x, y, dummyHexaIds, 0);

                    if (emptyCount > maxEmptyCount) {
                        maxEmptyCount = emptyCount;
                    }
                }
            }
        }

        gameOver = (maxEmptyCount < 3);

        return gameOver;
    }
    public static int EmptyCount(int x, int y, int[,] dummyHexaIds, int count){
        dummyHexaIds[x, y] = -1;            //同じ場所を認識しないよう、あらかじめidを-1にする
        count++;                           //個数を1ずつ増やす

        int[,] directions = StageManager.directions; //directionsを取得
        for (int i = 0; i < directions.GetLength(0); i++){
            int xPos = x + directions[i, 0];
            int yPos = y + directions[i, 1];
            int posYupper = 8 - (int)Math.Max(0, (xPos-4));
            int posYlower = (int)Math.Min(0, (xPos-4)) * -1;

            if (
                xPos >= 0 && yPos >= posYlower && //X、Y座標の下限以上である
                xPos <= 8 && yPos <= posYupper && //X、Y座標の上限以下である
                dummyHexaIds[xPos, yPos] == 0   //色が同じである
            ){
                count = EmptyCount(xPos, yPos, dummyHexaIds, count); //再帰関数を使用
            }
        }
        //総合個数を返す
        return count;
    }
}
