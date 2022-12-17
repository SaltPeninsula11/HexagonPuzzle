using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject hexa; //使用するヘキサ
    public static HexaManager[,] hexas = new HexaManager[9, 9];
    public static int[,] directions = new int[6,2]{
        {0, 1}, {1, 0},
        {1, -1}, {0, -1},
        {-1, 0}, {-1, 1}
    }; //中心周辺の座標（向き）

    /*
    変数hexas[i, j].idの値
     null = ステージ外
     0 = 黒色
     1 = 赤色
     2 = 黄色
     3 = 黄緑色
     4 = 水色
    */

    void Start()
    {
        makeHexa(hexa, 0, 0);
        int x = 0;
        int y = 0;
        for (int i = 1; i <= 4; i++){
            //個数は6の倍数
            int count = i*6;
            //上から作成
            Vector2 pos = new Vector2(0f, (2.62f*i));
            int dir = -1; //向き
            x = 0; //x座標
            y = i; //y座標

            for (int j = 0; j < count; j++){
                //ヘキサの作成
                makeHexa(hexa, x, y);
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
        //仮オブジェクトを非表示にする（アクティブにつながるため、透明にする）
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    public void makeHexa(GameObject hexa, int i, int j){
        //例：x座標-4 = インデックス0
        int finalI = i+4;
        int finalJ = j+4;
        hexas[finalI, finalJ] = Instantiate(
            hexa, 
            new Vector3(0f, 0f, 0f), 
            Quaternion.identity, this.transform
        ).GetComponent<HexaManager>();

        hexas[finalI, finalJ].pos = new Vector2(i, j);
        //ステージのヘキサを黒にする。
        hexas[finalI, finalJ].id = 0;
    }
}
