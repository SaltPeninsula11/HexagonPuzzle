using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrioSpecials : MonoBehaviour
{
    public GameObject explosion; //爆発エフェクト
    public GameObject arrow;     //矢のオブジェクト
    
    /* ボムジュエル：広範囲爆発 */
    public IEnumerator BombJewel(Vector2 pos){
        makeExplosion((int)pos.x, (int)pos.y);
        StageManager.hexas[(int)pos.x+4, (int)pos.y+4].id = 0;

        int x = 0;
        int y = 0;
        int erasedHexas = 0;

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
                if (StageManager.hexas[(int)indexPos.x+4, (int)indexPos.y+4].id != 0){
                    StageManager.hexas[(int)indexPos.x+4, (int)indexPos.y+4].id = 0;
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
        TrioController.control = true;
        GameManager.score += 10 * erasedHexas; //（仮）
    }
    void makeExplosion(int x, int y){
        Vector2 finalPos = new Vector2(2.26f*x, 2.62f*y);
        finalPos.y += 1.31f*x;
        Instantiate(explosion, finalPos, Quaternion.identity);
    }

    /* アロージュエル：一定方向3通り */
    public IEnumerator ArrowJewel(Vector2 pos, int direction){
        makeArrow((int)pos.x, (int)pos.y, direction * -60f);
        makeArrow((int)pos.x, (int)pos.y, (direction * -60f) + 180);

        int xWay = (direction == 1 || direction == 2) ? 1 : 0;
        int yWay = 0;
        if (direction == 0) {
            yWay = 1;
        } else if (direction == 2) {
            yWay = -1;
        }

        int erasedHexas = 0;
        for (int i = 0; i < 9; i++) {
            Vector2 positivePos = new Vector2(Math.Min(8, pos.x + (i * xWay) + 4), Math.Min(8, pos.y + (i * yWay) + 4));
            Vector2 negativePos = new Vector2(Math.Max(0, pos.x - (i * xWay) + 4), Math.Max(0, pos.y - (i * yWay) + 4));

            erasedHexas = eraseWithArrow(erasedHexas, positivePos);
            erasedHexas = eraseWithArrow(erasedHexas, negativePos);

            yield return new WaitForSeconds (0.2f);
        }

        TrioController.control = true;
        GameManager.score += 10 * erasedHexas;
    }
    int eraseWithArrow (int erasedHexas, Vector2 pos) {
        try {
            if (StageManager.hexas[(int)pos.x, (int)pos.y].id != 0){
                StageManager.hexas[(int)pos.x, (int)pos.y].id = 0;
                erasedHexas++;
            }

            return erasedHexas;
        } catch (NullReferenceException e) {
            return erasedHexas;
        } catch (IndexOutOfRangeException e) {
            return erasedHexas;
        }
    }
    void makeArrow(int x, int y, float rotation) {
        Vector2 finalPos = new Vector2(2.26f*x, 2.62f*y);
        finalPos.y += 1.31f*x;

        Transform arrowTip = Instantiate(arrow, finalPos, Quaternion.identity).transform;
        arrowTip.rotation = Quaternion.Euler(0, 0, rotation);
    }

    /* スタージュエル：6方向 */
    public IEnumerator StarJewel(Vector2 pos){
        for (int i = 0; i < 6; i++) {
            makeArrow((int)pos.x, (int)pos.y, i * -60f);
        }

        yield return new WaitForSeconds (1f);
        TrioController.control = true;
    }
}
