using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrioSpecials : MonoBehaviour
{
    public TrioManager manager;
    [Header("エフェクト")]
    public GameObject explosion; //爆発エフェクト
    public GameObject arrow;     //矢のオブジェクト
    public GameObject arrowStar; //矢のオブジェクト（スター）
    [Header("効果音")]
    public AudioClip explSound;
    public AudioClip arrowSound;
    public AudioClip scoreSound;

    private int erasedHexas = 0;
    
    /* ボムジュエル：広範囲爆発 */
    public IEnumerator BombJewel(Vector2 pos){
        manager.SoundPlay(explSound);

        makeExplosion((int)pos.x, (int)pos.y);
        manager.stage.hexas[(int)pos.x+4, (int)pos.y+4].id = 0;

        int x = 0;
        int y = 0;
        erasedHexas = 0;

        for (int i = 1; i <= 2; i++){
            //個数は6の倍数
            int count = i*6;
            //上から作成
            int dir = -1; //向き
            x = (int)pos.x; //x座標
            y = (int)pos.y + i; //y座標

            yield return new WaitForSeconds (0.1f); //0.1秒待つ

            for (int j = 0; j < count; j++){
                Vector2 indexPos = new Vector2(x, y);
                indexPos.x = Mathf.Clamp(indexPos.x, -4f, 4f);
                int posYupper = 4 - (int)Math.Max(0, indexPos.x);
                int posYlower = -4 + (int)Math.Min(0, indexPos.x) * -1;
                indexPos.y = Mathf.Clamp(indexPos.y, posYlower, posYupper);

                makeExplosion((int)indexPos.x, (int)indexPos.y);
                if (manager.stage.hexas[(int)indexPos.x+4, (int)indexPos.y+4].id != 0){
                    manager.stage.hexas[(int)indexPos.x+4, (int)indexPos.y+4].id = 0;
                    erasedHexas++;
                }

                if (j % i == 0){
                    //端まで行ったら向きを変える（60°ずつ）
                    dir++;
                    dir %= 6;
                }
                switch (dir) {
                    case 0:
                    pos += new Vector2(2.26f, -1.31f); //右下に移動
                    x += 1; y -= 1;
                    break;

                    case 1:
                    pos += new Vector2(0f, -2.62f); //下に移動
                    x += 0; y -= 1;
                    break;

                    case 2:
                    pos += new Vector2(-2.26f, -1.31f); //左下に移動
                    x -= 1; y += 0;
                    break;

                    case 3:
                    pos += new Vector2(-2.26f, 1.31f); //左上に移動
                    x -= 1; y += 1;
                    break;

                    case 4:
                    pos += new Vector2(0f, 2.62f); //上に移動
                    x += 0; y += 1;
                    break;

                    case 5:
                    pos += new Vector2(2.26f, 1.31f); //右上に移動
                    x += 1; y += 0;
                    break;
                }
            }
        }
        yield return new WaitForSeconds (0.5f); //0.5秒待つ
        this.GetComponent<TrioController>().control = true;

        //得点
        ScoreCalc(pos);
    }
    void makeExplosion(int x, int y){
        Vector2 finalPos = new Vector2(2.26f*x, 2.62f*y);
        finalPos.y += 1.31f*x;
        Instantiate(explosion, finalPos, Quaternion.identity);
    }

    /* アロージュエル：一定方向3通り */
    public IEnumerator ArrowJewel(Vector2 pos, int direction, bool point = true, GameObject a = null){
        if (point) {
            manager.SoundPlay(arrowSound);
        }
        if (a == null) {
            a = arrow;
        }

        makeArrow((int)pos.x, (int)pos.y, direction * -60f, a);
        makeArrow((int)pos.x, (int)pos.y, (direction * -60f) + 180, a);

        int xWay = (direction == 1 || direction == 2) ? 1 : 0;
        int yWay = 0;
        if (direction == 0) {
            yWay = 1;
        } else if (direction == 2) {
            yWay = -1;
        }

        erasedHexas = 0;
        for (int i = 0; i < 9; i++) {
            Vector2 positivePos = new Vector2(Math.Min(9, pos.x + (i * xWay) + 4), Math.Min(8, pos.y + (i * yWay) + 4));
            Vector2 negativePos = new Vector2(Math.Max(-1, pos.x - (i * xWay) + 4), Math.Max(0, pos.y - (i * yWay) + 4));

            if (positivePos.x < 9) {
                erasedHexas = eraseWithArrow(erasedHexas, positivePos);
            }
            if (negativePos.x > -1) {
                erasedHexas = eraseWithArrow(erasedHexas, negativePos);
            }

            yield return new WaitForSeconds (0.2f);
        }

        this.GetComponent<TrioController>().control = true;

        //得点
        if (point) {
            ScoreCalc(pos);
        }
    }
    int eraseWithArrow (int erasedHexas, Vector2 pos) {
        try {
            if (manager.stage.hexas[(int)pos.x, (int)pos.y].id != 0){
                manager.stage.hexas[(int)pos.x, (int)pos.y].id = 0;
                erasedHexas++;
            }

            return erasedHexas;
        } catch {
            return erasedHexas;
        }
    }
    void makeArrow(int x, int y, float rotation, GameObject a) {
        Vector2 finalPos = new Vector2(2.26f*x, 2.62f*y);
        finalPos.y += 1.31f*x;

        Transform arrowTip = Instantiate(a, finalPos, Quaternion.identity).transform;
        arrowTip.rotation = Quaternion.Euler(0, 0, rotation);
    }

    /* スタージュエル：6方向 */
    public void StarJewel(Vector2 pos){
        for (int i = 0; i < 3; i++) {
            StartCoroutine(ArrowJewel(new Vector2(pos.x, pos.y), i, (i == 0), arrowStar));
        }
    }

    /* レインボージュエル：どんな色でもよし */
    public void RainbowJewel(Vector2 pos) {
        int checkAround(int posX, int posY, int color) {
            if (color == 6) {
                //虹色同士で消すときは、高得点が出るようにする。
                return 0;
            }
            //StageManagerのhexasに影響しないよう、ダミーの配列を作る。
            int[,] dummyHexaIds = new int[9, 9];
            for (int x = 0; x < manager.stage.hexas.GetLength(0); x++){
                for (int y = 0; y < manager.stage.hexas.GetLength(1); y++){
                    if (manager.stage.hexas[x, y] == null){
                        //ステージ外の場合
                        dummyHexaIds[x, y] = -1;
                    } else {
                        //ステージ内の場合
                        dummyHexaIds[x, y] = manager.stage.hexas[x, y].id;
                    }
                }
            }

            //最初に取得した色の数を数える。
            //同時に、周囲に同じ色が存在したら、数え上げる。
            int count = manager.JewelCount(posX, posY, dummyHexaIds, 0, color);
            return count;
        }

        int vanishedCount = 0;

        for (int i = 0; i < StageManager.directions.GetLength(0); i++) {
            try {
                int posX = (int)pos.x + StageManager.directions[i, 0];
                int posY = (int)pos.y + StageManager.directions[i, 1];
                if (manager.stage.hexas[posX, posY].id != 0) {
                    manager.matchedList = new List<int[]>();
                    int jewelCount = checkAround(posX, posY, manager.stage.hexas[posX, posY].id);

                    if (jewelCount >= 4){
                        //４つそろった時
                        vanishedCount++;
                        GameManager.combo++;

                        foreach (int[] mpos in manager.matchedList){
                            if (manager.stage.hexas[mpos[0], mpos[1]].id != 6) {
                                manager.stage.hexas[mpos[0], mpos[1]].id = 0;
                            }
                            Instantiate(
                                manager.vanishEffect, 
                                manager.setPos(new Vector2(mpos[0]-4, mpos[1]-4)), 
                                Quaternion.identity
                            ); //消えるエフェクトの作成
                        }
                        Vector3 popUpPos = new Vector3(posX-4, posY-4, -5); //得点増加オブジェクトの座標を設定
                        
                        int score = (int)Math.Pow((jewelCount - 3), 2);    //得点（4つ＝50点、５つ＝200点、６つ＝450点・・・）
                        score *= (50 + (int)Math.Floor(150 * ((GameManager.level - 1) / 98f))); //レベルに応じる得点の上昇
                        score *= Math.Max(1, (GameManager.combo - 1) * 2); //コンボに応じる得点の上昇
                        manager.ScorePopUp(manager.setPos(popUpPos), score);
                        
                        GameManager.jewels += jewelCount;

                        manager.SoundPlay(manager.VanishedSound);
                    }
                }
            } catch {
                continue;
            }
        }

        if (vanishedCount <= 0) {
            //コンボのリセット
            GameManager.combo = 0;
        } else {
            manager.stage.hexas[(int)pos.x, (int)pos.y].id = 0;
        }
    }

    void ScoreCalc(Vector2 pos) {
        //得点
        int score = 0;
        if (erasedHexas <= 0) {
            score = 3000;
        } else {
            score = 50 * erasedHexas;
        }
        manager.ScorePopUp(manager.setPos(pos), score);

        manager.SoundPlay(scoreSound);
    }
}
