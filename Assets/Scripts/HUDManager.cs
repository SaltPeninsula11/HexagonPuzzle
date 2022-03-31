using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public RectTransform[] rects = new RectTransform[2]; //移動するＪＥＷＥＬＳとＬＥＶＥＬ
    public Text[] values = new Text[4];                  //各値を表示するテキスト
    public GameObject gameOverObj;                       //ゲームオーバー表示に使用
    public GameObject levelUpObj;                        //レベルアップ表示に使用
    public GameObject[] nextJewels = new GameObject[3];  //次の形の各ジュエル
    public Sprite normalJewel;                           //通常ジュエル用のスプライト
    public Sprite[] specialJewels = new Sprite[5];       //スペシャルジュエル用のスプライト
    public Image specialJewelFill;                       //スペシャルジュエルが出るまでのゲージ
    private int[] colorIds = new int[3];                 //次の形の各ジュエルの色
    private bool isSmartPhone;                           //スマホ判定に使用

    [Header("効果音")]
    public AudioClip LevelUpSound;

    void Update()
    {
        //スコア（8桁表示）
        values[0].text = GameManager.score.ToString("D8");
        //ハイスコア（8桁表示）
        values[1].text = GameManager.hiScore.ToString("D8");
        //消したジュエルの数（4桁表示）
        values[2].text = GameManager.jewels.ToString("D4");
        //レベル（2桁表示）
        values[3].text = GameManager.level.ToString("D2");

        //ゲームオーバー表示
        gameOverObj.SetActive(GameManager.gameOver);

        //移動するジュエルの宣言
        RectTransform bottomJewel = nextJewels[2].GetComponent<RectTransform>();
        //形状判定
        switch (GameManager.nextShapeId){
            case 0:
            //まっすぐ
            bottomJewel.anchoredPosition = new Vector2(0f, -29.5f);
            break;

            case 1:
            //くの字
            bottomJewel.anchoredPosition = new Vector2(22.6f, -16.3f);
            break;
            
            case 2:
            //三角形
            bottomJewel.anchoredPosition = new Vector2(22.6f, 9.9f);
            break;
        }
        //色・アイコンの変更
        for (int i = 0; i < nextJewels.Length; i++){
            Image nextJewel = nextJewels[i].GetComponent<Image>();
            //色を変更
            nextJewel.color = GameManager.getColor(GameManager.nextColorIds[i]);

            if (GameManager.nextColorIds[i] >= 4){
                //色IDが4以上の場合、スペシャルジュエルのアイコンに変更
                nextJewel.sprite = specialJewels[GameManager.nextColorIds[i]-4];
            } else {
                //通常
                nextJewel.sprite = normalJewel;
            }
        }
        //とりあえず50%に設定
        int current = 5;
        int max = 10;
        float sFillAmount = (float)current / (float)max; //割合の計算
        specialJewelFill.fillAmount = sFillAmount;

        //スマホ画面の判定
        #if UNITY_IOS
            isSmartPhone = true;
        #elif UNITY_IPHONE
            isSmartPhone = true;
        #elif UNITY_ANDROID
            isSmartPhone = true;
        #else
            isSmartPhone = false;
        #endif

        Vector2[] pos = {
            rects[0].anchoredPosition,
            rects[1].anchoredPosition,
        };
        //ＪＥＷＥＬＳ、ＬＥＶＥＬの配置を上に移動する。
        if (isSmartPhone){
            rects[0].anchoredPosition = new Vector2(pos[0].x, 320f);
            rects[1].anchoredPosition = new Vector2(pos[1].x, 320f);
        } else {
            rects[0].anchoredPosition = new Vector2(pos[0].x, 35f);
            rects[1].anchoredPosition = new Vector2(pos[1].x, 35f);
        }
        
        if (Input.GetKey(KeyCode.Escape)){
            //Escキーで終了
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
            #endif
        }
    }

    public IEnumerator LevelUpDisplay() {
        //効果音を鳴らす
        SoundPlay(LevelUpSound);

        for (int i = 0; i < 5; i++) {
            levelUpObj.SetActive(true);
            yield return new WaitForSeconds (0.05f);
            levelUpObj.SetActive(false);
            yield return new WaitForSeconds (0.05f);
        }
        levelUpObj.SetActive(true);
        yield return new WaitForSeconds (1f);
        levelUpObj.SetActive(false);
    }

    void SoundPlay(AudioClip sound) {
        GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
