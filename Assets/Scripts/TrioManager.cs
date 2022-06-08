using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrioManager : MonoBehaviour
{
    private int shapeId;            //形のＩＤ
    public GameObject jewel;        //トリオに使用するジュエル
    public Sprite normalJewel;                     //通常ジュエル用のスプライト
    public Sprite[] specialJewels = new Sprite[5]; //スペシャルジュエル用のスプライト
    public GameObject scorePopUp;   //スコア加算ポップアップ
    public GameObject vanishEffect; //消えるエフェクト

    public static GameObject[] trioJewels = new GameObject[7];//各ジュエルのオブジェクト
    private int[] colorIds = new int[7];                      //各ジュエルの色
    private Vector2 pos = new Vector2(0f, 0f);                //座標
    TrioSpecials trioSpecials;                                //TrioSpecialsクラス
    private List<int[]> matchedList;                          //そろった色の座標を格納するリスト

    [Header("効果音")]
    public AudioClip RotateSound;
    public AudioClip DropSound;
    public AudioClip VanishedSound;
    public AudioClip LoseSound;

    void Start()
    {
        //TrioSpecialsクラスの取得
        trioSpecials = GetComponent<TrioSpecials>();

        //最初の形
        GameManager.nextShape();
        //個数あたりの角度
        float anglePerCount = (float)(60 * Mathf.Deg2Rad);
        //中心の作成
        trioJewels[0] = Instantiate(
            jewel, 
            new Vector3(0, 0, -1f), 
            Quaternion.identity, this.transform
        );

        for (int i = 1; i < colorIds.Length; i++){
            //中心の周囲を作成
            float angle = anglePerCount * (i-1);
            float distance = 2.62f;
            trioJewels[i] = Instantiate(
                jewel, 
                new Vector3((float)Math.Sin(angle)*distance, (float)Math.Cos(angle)*distance, -1f), 
                Quaternion.identity, this.transform
            );
        }
        //仮オブジェクトを非表示にする（アクティブにつながるため、透明にする）
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

        setShape();
    }

    void setShape(){
        shapeId = GameManager.nextShapeId;
        /*
        変数colorIds
        -1：なし
         0：赤色
         1：黄色
         2：黄緑色
         3：水色
         4~8：スペシャルジュエル
        */
        colorIds[0] = GameManager.nextColorIds[0];
        colorIds[1] = GameManager.nextColorIds[1];
        for (int i = 2; i < colorIds.Length; i++){
            colorIds[i] = -1;
        }
        switch (shapeId){
            case 0:
            //まっすぐ
            colorIds[4] = GameManager.nextColorIds[2];
            break;

            case 1:
            //くの字
            colorIds[3] = GameManager.nextColorIds[2];
            break;
            
            case 2:
            //三角形
            colorIds[2] = GameManager.nextColorIds[2];
            break;
        }

        if (GameManager.specialFill == GameManager.specialMax) {
            if (UnityEngine.Random.Range(0, 5) == 0) {
                colorIds[0] = UnityEngine.Random.Range(5, 8);
            }
        }
        pos = new Vector2(0f, 0f);
        
        //次の形
        GameManager.nextShape();
    }

    void Update()
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        for (int i = 0; i < colorIds.Length; i++){
            trioJewels[i].SetActive(colorIds[i] >= 0);
        }

        //座標の上限・下限
        pos.x = Mathf.Clamp(pos.x, -4f, 4f);
        int posYupper = 4 - (int)Math.Max(0, pos.x);
        int posYlower = -4 + (int)Math.Min(0, pos.x) * -1;
        pos.y = Mathf.Clamp(pos.y, posYlower, posYupper);

        for (int i = 0; i < (colorIds.Length - 1); i++){
            if (colorIds[i+1] >= 0){
                //中心の周囲のジュエルの中で、colorIdが0以上の状態である部分から、
                //座標の上限・下限を過ぎたら中心の座標をずらす設定。
                Vector2 jewelPos = new Vector2(
                    StageManager.directions[i, 0] + pos.x,
                    StageManager.directions[i, 1] + pos.y
                ); //座標の取得
                //座標の上限・下限
                if (jewelPos.x < -4){
                    pos.x -= jewelPos.x + 4;
                } else if (jewelPos.x > 4){
                    pos.x -= jewelPos.x - 4;
                }
                posYupper = 4 - (int)Math.Max(0, jewelPos.x);
                posYlower = -4 + (int)Math.Min(0, jewelPos.x) * -1;
                if (jewelPos.y < posYlower){
                    pos.y -= jewelPos.y - posYlower;
                } else if (jewelPos.y > posYupper){
                    pos.y -= jewelPos.y - posYupper;
                }
            }
        }

        //座標の整理
        rect.anchoredPosition = setPos(pos);
        
        //スプライトの切り替え
        for (int i = 0; i < trioJewels.Length; i++){
            SpriteRenderer jewel = trioJewels[i].GetComponent<SpriteRenderer>();
            jewel.color = GameManager.getColor(colorIds[i]);
            if (colorIds[i] >= 4){
                jewel.sprite = specialJewels[colorIds[i] - 4];
            } else {
                jewel.sprite = normalJewel;
            }
        }
    }
    //操作
    public void Move(int x, int y){
        /** 上下操作 **/
        pos.x += x;
        pos.y += y;
    }
    public void Rotate(){
         /* 回転 */
        List<int[]> jewelList = new List<int[]>();

        for (int i = 1; i < colorIds.Length; i++){
            if (colorIds[i] >= 0){
                int rotateIndex = i+1;
                if (rotateIndex > 6) rotateIndex = 1;
                /*
                左回転
                int rotateIndex = i-1;
                if (rotateIndex < 1) rotateIndex = 6;
                */
                jewelList.Add(new int[2]{rotateIndex, colorIds[i]});
                colorIds[i] = -1;
            }
        }

        foreach (int[] index in jewelList){
            colorIds[index[0]] = index[1];
        }

        SoundPlay(RotateSound);
    }
    public void Drop(){
        /* 置く */
        int posX = (int)pos.x;
        int posY = (int)pos.y;
        List<int[]> fitList = new List<int[]>();

        //すでに置いてあるジュエルの上にないかチェックする。
        if (StageManager.hexas[posX+4, posY+4].id == 0){
            fitList.Add(new int[3]{posX+4, posY+4, colorIds[0]});
        }
        for (int i = 0; i < (colorIds.Length-1); i++){
            if (colorIds[i+1] >= 0){
                int stageX = StageManager.directions[i, 0]+4+posX;
                int stageY = StageManager.directions[i, 1]+4+posY;
                if (StageManager.hexas[stageX, stageY].id == 0){
                    fitList.Add(new int[3]{stageX, stageY, colorIds[i+1]});
                }
            }
        }
        //すべて空いていればＯＫ
        if (fitList.Count >= 3){
            //１つずつ置く
            foreach(int[] fitVals in fitList){
                /*
                fitVals[0]＝x座標
                fitVals[1]＝y座標
                fitVals[2]＝色
                */
                int changeId = fitVals[2]+1;
                if (changeId < 5){
                    StageManager.hexas[fitVals[0], fitVals[1]].id = changeId;
                } else if (changeId == 5) {
                    /* ボムジュエル */
                    StartCoroutine(trioSpecials.BombJewel(new Vector2(fitVals[0]-4, fitVals[1]-4)));
                    TrioController.control = false;
                } else if (changeId >= 6 && changeId <= 8) {
                    /* アロージュエル */
                    StartCoroutine(trioSpecials.ArrowJewel(new Vector2(fitVals[0]-4, fitVals[1]-4), changeId - 6));
                    TrioController.control = false;
                } else if (changeId == 9) {
                    /* スタージュエル */
                    StartCoroutine(trioSpecials.StarJewel(new Vector2(fitVals[0]-4, fitVals[1]-4)));
                    TrioController.control = false;
                }
            }
            //置いている合間だと正確に認識できないので、再度foreachをする。
            foreach(int[] fitVals in fitList){
                matchedList = new List<int[]>();

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

                int jewelCount = JewelCount(fitVals[0], fitVals[1], dummyHexaIds, 0);
                if (jewelCount >= 4){
                    //４つそろった時
                    foreach (int[] pos in matchedList){
                        StageManager.hexas[pos[0], pos[1]].id = 0;
                        Instantiate(
                            vanishEffect, 
                            setPos(new Vector2(pos[0]-4, pos[1]-4)), 
                            Quaternion.identity
                        ); //消えるエフェクトの作成
                    }
                    Vector3 popUpPos = new Vector3(fitVals[0]-4, fitVals[1]-4, -5); //得点増加オブジェクトの座標を設定
                    int score = (int)Math.Pow((jewelCount - 3), 2) * (GameManager.level + 99); //得点（仮）
                    ScorePopUp(setPos(popUpPos), score);
                    GameManager.jewels += jewelCount;

                    SoundPlay(VanishedSound);
                } else {
                    SoundPlay(DropSound);
                }
            }
            setShape();
            GameManager.nextShape();

            if (GameManager.CheckForGameOver()) {
                SoundPlay(LoseSound);
            }
        }

        if (GameManager.specialFill == GameManager.specialMax) {
            GameManager.specialFill = 0;
        } else {
            GameManager.specialFill++;
        }
    }

    public int JewelCount(int x, int y, int[,] dummyHexaIds, int count){
        matchedList.Add(new int[2]{x, y}); //そろった時に消す座標をmatchedListに追加
        int colorId = dummyHexaIds[x, y];  //色を取得
        dummyHexaIds[x, y] = -1;            //同じ場所を認識しないよう、あらかじめidを-1にする
        count++;                           //個数を1ずつ増やす

        int[,] directions = StageManager.directions; //directionsを取得
        for (int i = 0; i < directions.GetLength(0); i++){
            int xPos = x + directions[i, 0];
            int yPos = y + directions[i, 1];
            int posYupper = 8 - (int)Math.Max(0, (xPos-4));
            int posYlower = (int)Math.Min(0, (xPos-4)) * -1;

            if (
                colorId != 0 &&                   //赤～水色のいずれかである
                xPos >= 0 && yPos >= posYlower && //X、Y座標の下限以上である
                xPos <= 8 && yPos <= posYupper && //X、Y座標の上限以下である
                dummyHexaIds[xPos, yPos] == colorId   //色が同じである
            ){
                count = JewelCount(xPos, yPos, dummyHexaIds, count); //再帰関数を使用
            }
        }
        //総合個数を返す
        return count;
    }
    
    void ScorePopUp(Vector2 pos, int value){
        /* スコア増加 */
        GameManager.ScorePopUpPos = pos;                            //座標
        GameManager.ScorePopUpValue = value;                        //値
        Instantiate(scorePopUp, Vector2.zero, Quaternion.identity); //作成
    }
    Vector2 setPos(Vector2 pos){
        /* 座標の整理 */
        Vector2 finalPos = new Vector2(2.26f*pos.x, 2.62f*pos.y);
        finalPos.y += 1.31f*pos.x;
        return finalPos;
    }

    void SoundPlay(AudioClip sound) {
        GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
