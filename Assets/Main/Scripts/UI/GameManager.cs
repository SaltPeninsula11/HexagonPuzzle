using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameData data;
    public RankingData ranking;
    public StageManager s;
    public StageManager s2p;
    public static StageManager stage;
    public static StageManager stage2p;

    public static int score = 0;
    public static int hiScore = 100000;
    public static int jewels = 0;
    public static int level = 1;

    public static int combo = 0;
    public static int maxCombo = 0;

    public static int specialCounts = 0;

    public static bool gameOver;
    public static bool cleared;
    public static int nextShapeId;                 //次の形のＩＤ
    public static int[] nextColorIds = new int[3]; //次の形の各色のＩＤ
    public static Vector2 ScorePopUpPos;           //スコア加算ポップアップの座標
    public static int ScorePopUpValue;             //スコア加算ポップアップの値

    public static int specialFill = 0;
    public static int specialMax = 50;
    public static int specialJewel = 0;

    public static float timeLimit = 10f;
    public static int goalAmounts = 10;

    public static int[] enableShapes = new int[3]{99, 99, 99};

    void Awake()
    {
        SaveManager save = this.GetComponent<SaveManager>();
        save.LoadPlayerData();

        //初期設定
        score = 0;
        jewels = 0;
        combo = 0;
        maxCombo = 0;
        level = (data.mode == GameMode.TimeAttack) ? Math.Max(1, (data.colors - 2) * 15) : 1;

        gameOver = false;
        cleared = false;
        specialFill = 0;
        specialJewel = UnityEngine.Random.Range(0, 4);

        specialCounts = 0;

        timeLimit = data.timeLimit;

        if (data.mode == GameMode.TimeAttack) {
            hiScore = ranking.timeEntries[0].score;
        } else {
            hiScore = ranking.normalEntries[0].score;
        }

        if (data.mode != GameMode.TimeAttack) {
            switch (data.difficulty) {
                case Difficulty.Normal:
                level = 15;
                jewels = 14 * 30;
                score = 50000;
                break;
                
                case Difficulty.Hard:
                level = 30;
                jewels = 29 * 30;
                score = 150000;
                break;

                case Difficulty.Expert:
                level = 45;
                jewels = 44 * 30;
                score = 300000;
                break;
            }
        }

        stage = s;
        stage2p = s2p;
    }

    void Update()
    {
        bool isNormal = (data.mode == GameMode.Normal);

        /** システム **/
        //ハイスコア更新
        if (score > hiScore) hiScore = score;
        //レベルアップ（条件は仮、各30ジュエルでレベルアップ）
        if ((int)Math.Floor(jewels / 30f) == level && isNormal && level < 99) {
            GetComponent<HUDManager>().StartCoroutine("LevelUpDisplay");
            level++;
        }
        //レベル９９，ジュエルを消した数が３０００に達するとクリア！
        if (isNormal && jewels >= 3000 && !cleared) {
            cleared = true;
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

        /** コンボ **/
        if (combo > maxCombo) {
            maxCombo = combo;
        }

        /** タイムアタック限定 **/
        if (data.mode == GameMode.TimeAttack) {
            if (!gameOver && !cleared) {
                //残り時間の減少
                timeLimit -= Time.deltaTime;
                timeLimit = Mathf.Clamp(timeLimit, 0f, 99.9f);

                if (timeLimit <= 0f) {
                    //時間切れ
                    gameOver = true;
                }
            }

            //ジュエルを消した数が、目標個数に達するとクリア！
            if (jewels >= data.goalAmounts && !cleared) {
                cleared = true;
            }

            //スペシャルジュエルは使用不可
            specialFill = 0;
        }

        if (data.mode == GameMode.TimeAttack) {
            data.gameOver = cleared;
        } else {
            data.gameOver = gameOver || cleared;
        }
    }
    /* 次の形を指定 */
    public static void nextShape(int special){
        //色の範囲を指定
        int colorRange;
        if (level >= 45){
            //レベルが45以上
            colorRange = 5;
        } else if (level >= 30){
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
        // int sum = 0;
        // foreach (int val in enableShapes) {
        //     sum += val;
        // }

        if (enableShapes[0] < 10 || enableShapes[1] < 10 || enableShapes[2] < 10) {
            int maxIndex = -1;
            int maxCount = 0;
            for (int i = 0; i < enableShapes.Length; i++) {
                if (enableShapes[i] > maxCount) {
                    maxIndex = i;
                    maxCount = enableShapes[i];
                }
            }
            nextShapeId = maxIndex;
        }
        // while (sum > 0 && enableShapes[nextShapeId] <= 0) {
        //     nextShapeId = UnityEngine.Random.Range(0, 3);
        //     Debug.Log("Step");
        // }

        if (special >= 0) {
            //スペシャルジュエル
            nextColorIds[0] = special + 5;

            for (int i = 1; i < nextColorIds.Length; i++){
                nextColorIds[i] = -1;
            }
        } else {
            //色のIDを0~3までのランダムにする。
            for (int i = 0; i < nextColorIds.Length; i++){
                nextColorIds[i] = UnityEngine.Random.Range(0, colorRange);
            }
        }
    }
    /* 色の取得 */
    public static Color getColor(int colorId){
        Color colorVal;
        switch (colorId){
            case -1:
            colorVal = new Color(0f, 0f, 0f, 0f); //非表示
            break;

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

            case 4:
            colorVal = new Color(0.55f, 0f, 1f, 1f); //紫色
            break;

            default:
            colorVal = new Color(1f, 1f, 1f, 1f); //白色
            break;
        }
        return colorVal;
    }

    public static void CheckForGameOver() {
        //StageManagerのhexasに影響しないよう、ダミーの配列を作る。
        int[,] dummyHexaIds = new int[9, 9];
        for (int x = 0; x < stage.hexas.GetLength(0); x++){
            for (int y = 0; y < stage.hexas.GetLength(1); y++){
                if (stage.hexas[x, y] == null){
                    //ステージ外の場合
                    dummyHexaIds[x, y] = -1;
                } else {
                    //ステージ内の場合
                    dummyHexaIds[x, y] = stage.hexas[x, y].id;
                }
            }
        }

        int emptyCount = 0;
        int maxEmptyCount = 0;

        for (int x = 0; x < dummyHexaIds.GetLength(0); x++) {
            for (int y = 0; y < dummyHexaIds.GetLength(1); y++){
                if (dummyHexaIds[x, y] == 0){
                    emptyCount = EmptyCount(x, y, dummyHexaIds, 0);

                    if (emptyCount > maxEmptyCount) {
                        maxEmptyCount = emptyCount;
                    }
                }
            }
        }

        //一つでも3つ並んでいる箇所がなければ、ゲームオーバー。
        gameOver = (maxEmptyCount < 3);
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
