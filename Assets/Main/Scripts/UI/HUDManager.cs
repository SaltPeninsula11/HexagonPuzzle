using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [Header("パラメータ")]
    public RectTransform[] rects = new RectTransform[2]; //移動するＪＥＷＥＬＳとＬＥＶＥＬ
    public Text[] values = new Text[4];                  //各値を表示するテキスト
    public Text timeDisplay;                             //残り時間表示
    public GameObject countDown;                         //10秒前カウントダウン
    public Text goalJewels;                              //ジュエルの目標個数
    [Header("コンボ")]
    public GameObject comboGroup;                        //コンボのグループ
    public Text comboValue;                              //コンボ
    [Header("ゲームオーバー・レベルアップ")]
    public GameObject gameOverObj;                       //ゲームオーバー表示に使用
    public GameObject levelUpObj;                        //レベルアップ表示に使用
    public GameObject clearObj;                          //クリア表示に使用
    [Header("メッセージ")]
    public GameObject colorIncreased;
    public GameObject remainedJewels;
    [Header("次のジュエル")]
    public GameObject[] nextJewels = new GameObject[3];  //次の形の各ジュエル
    public Sprite normalJewel;                           //通常ジュエル用のスプライト
    [Header("スペシャルジュエル")]
    public GameObject specialGroup;                      //スペシャルジュエルのグループ
    public Sprite[] specialJewels = new Sprite[6];       //スペシャルジュエル用のスプライト
    public Image specialJewelFill;                       //スペシャルジュエルが出るまでのゲージ
    public Image specialJewelIcon;                       //スペシャルジュエルのアイコン
    private int[] colorIds = new int[3];                 //次の形の各ジュエルの色
    private bool isSmartPhone;                           //スマホ判定に使用
    [Header("ポーズ")]
    public GameObject pauseScreen;                       //ポーズ画面

    [Header("効果音")]
    public AudioClip LevelUpSound;
    public AudioClip GameOverSound;

    private GameData data;
    private bool gameOverTrigger = false;

    private bool paused = false;

    private float blinkTime = 0.5f;
    private int currentCountDown = 10;

    void Start()
    {
        data = this.GetComponent<GameManager>().data;
        gameOverTrigger = false;
    }

    void Update()
    {
        //スコア（8桁表示）
        values[0].text = GameManager.score.ToString("D8");
        //ハイスコア（8桁表示）
        values[1].text = GameManager.hiScore.ToString("D8");
        //消したジュエルの数（4桁表示）
        values[2].text = GameManager.jewels.ToString("D4");
        goalJewels.text = "/ " + data.goalAmounts.ToString("D4");
        goalJewels.gameObject.SetActive(data.mode == GameMode.TimeAttack);
        //レベル（2桁表示）
        values[3].text = GameManager.level.ToString("D2");

        //残り時間
        timeDisplay.text = string.Format("{0:f1}", GameManager.timeLimit);
        timeDisplay.color = GameManager.timeLimit <= 10.0f ? Color.red : Color.white;
        timeDisplay.gameObject.SetActive(data.mode == GameMode.TimeAttack);

        //コンボ
        comboGroup.SetActive(GameManager.combo >= 2);
        comboValue.text = GameManager.combo.ToString();

        //ゲームオーバー表示
        gameOverObj.SetActive(GameManager.gameOver);
        if (GameManager.gameOver && !gameOverTrigger) {
            StartCoroutine("GameOver");
            gameOverTrigger = true;
        }
        
        //クリア表示
        clearObj.SetActive(GameManager.cleared);

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

            if (GameManager.nextColorIds[i] >= 5){
                //色IDが5以上の場合、スペシャルジュエルのアイコンに変更
                nextJewel.sprite = specialJewels[GameManager.nextColorIds[i]-5];
            } else {
                //通常
                nextJewel.sprite = normalJewel;
            }
        }
        //スペシャルジュエル
        specialGroup.SetActive(data.mode != GameMode.TimeAttack);

        int current = GameManager.specialFill;
        int max = GameManager.specialMax;
        float sFillAmount = (float)current / (float)max; //割合の計算
        specialJewelFill.fillAmount = sFillAmount;
        specialJewelIcon.sprite = specialJewels[GameManager.specialJewel];

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

        //ポーズ画面
        pauseScreen.SetActive(paused);

        if (data.mode == GameMode.TimeAttack) {
            //残りn個
            int jewelsDif = data.goalAmounts - GameManager.jewels;
            remainedJewels.GetComponent<Text>().text = "残り " + jewelsDif.ToString() + " 個！";
            blinkTime -= Time.deltaTime;
            if (blinkTime <= 0) {
                blinkTime += 0.5f;
            }
            remainedJewels.SetActive((jewelsDif >= 1 && jewelsDif <= 20 && blinkTime <= 0.25f) && !GameManager.gameOver && !GameManager.cleared);

            //カウントダウン
            int cdvalue = (int)Math.Ceiling(GameManager.timeLimit);
            countDown.SetActive(GameManager.timeLimit > 0 && GameManager.timeLimit <= 10 && !GameManager.cleared);
            countDown.GetComponent<Text>().text = (cdvalue).ToString();

            if (cdvalue < currentCountDown) {
                countDown.GetComponent<Animator>().Play("CountDown", 0, 0);
                currentCountDown = cdvalue;
            }
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
    public IEnumerator ColorModeStart() {
        colorIncreased.SetActive(true);
        colorIncreased.GetComponent<Text>().text = ((int)(GameManager.level / 15f) + 2).ToString() + " COLORS MODE START!";

        yield return new WaitForSeconds (3f);

        colorIncreased.SetActive(false);
    }

    void SoundPlay(AudioClip sound) {
        GetComponent<AudioSource>().PlayOneShot(sound, data.soundVolume);
    }

    IEnumerator GameOver() {
        //ゲームオーバー
        SoundPlay(GameOverSound);

        yield return new WaitForSeconds(3f);

        bool goToRanking = false;
        if (data.mode == GameMode.TimeAttack) {
            goToRanking = false;
        } else {
            goToRanking = GameManager.gameOver || GameManager.cleared;
        }
        if (goToRanking) {
            SceneManager.LoadScene("Ranking");
        } else {
            SceneManager.LoadScene("Title");
        }
    }

    public void Pause() {
        paused = true;
        Time.timeScale = 0f;
    }
    public void Resume() {
        paused = false;
        Time.timeScale = 1f;
    }
    public void Quit() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
}
