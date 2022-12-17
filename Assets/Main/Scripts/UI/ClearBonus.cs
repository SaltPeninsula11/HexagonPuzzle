using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearBonus : MonoBehaviour
{
    [Header("データ")]
    public GameData data;
    [Header("ヘッダー")]
    public Text bonusHeader;
    
    private int previousScore;
    private Text values;
    private int timeBonus = 100000;
    private float colorRate = 1.0f;
    private int specialBonus = 0;
    private int comboBonus = 0;
    
    private int specialBDisplay = 0;
    private int comboBDisplay = 0;

    private int timeBDisplay = 0;
    private float colorRDisplay = 1.0f;
    private float comboRDisplay = 1.0f;

    private bool timeAttack = false;
    private int step = 0;

    private AudioSource source;
    private bool sound = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        previousScore = GameManager.score;

        //レベル99クリア
        specialBonus = GameManager.specialCounts * 100000;
        comboBonus = GameManager.maxCombo * 50000;
        //タイムアタック
        timeBonus = (int)((GameManager.timeLimit / data.timeLimit) * 100000f);
        colorRate = data.colors * 0.5f;

        values = this.GetComponent<Text>();

        timeAttack = (data.mode == GameMode.TimeAttack);

        if (timeAttack) {
            StartCoroutine("TimeAttackBonus");
        } else {
            StartCoroutine("Bonus");
        }

        source = this.GetComponent<AudioSource>();
        StartCoroutine("BonusSound");
    }

    // Update is called once per frame
    void Update()
    {
        if (timeAttack) {
            bonusHeader.text = 
                "SCORE\n" +
                "TIME BONUS\n" + 
                "COLORS RATE\n" + 
                "COMBO RATE\n" + 
                "\n" + 
                "TOTAL SCORE";
            values.text = 
                previousScore.ToString() + "\n" +
                timeBDisplay.ToString() + "\n" + 
                "×" + colorRDisplay.ToString("F1") + "\n" + 
                "×" + comboRDisplay.ToString("F1") + "\n\n" + 
                GameManager.score.ToString();
            
            switch (step) {
                case 1:
                //タイムボーナス
                timeBDisplay += (int)(20000 * Time.deltaTime);
                if (timeBDisplay > timeBonus) {
                    timeBDisplay = timeBonus;
                }
                break;

                case 2:
                //カラーレート
                colorRDisplay += 2f * Time.deltaTime;
                if (colorRDisplay > colorRate) {
                    colorRDisplay = colorRate;
                }
                break;

                case 3:
                //コンボレート
                comboRDisplay += 2f * Time.deltaTime;
                if (comboRDisplay > GameManager.maxCombo) {
                    comboRDisplay = (float)GameManager.maxCombo;
                }
                break;
            }

            //（スコア + タイムボーナス）× カラーレート × コンボレート
            GameManager.score = (int)((previousScore + timeBDisplay) * colorRDisplay * comboRDisplay);
        } else {
            bonusHeader.text = 
                "SCORE\n" +
                "SPECIAL BONUS\n" + 
                "COMBO BONUS\n" + 
                "\n" + 
                "TOTAL SCORE";
            values.text = 
                previousScore.ToString() + "\n" +
                specialBDisplay.ToString() + "\n" + 
                comboBDisplay.ToString() + "\n\n" + 
                GameManager.score.ToString();
            
            switch (step) {
                case 1:
                //スペシャルボーナス
                specialBDisplay += (int)(200000 * Time.deltaTime);
                if (specialBDisplay > specialBonus) {
                    specialBDisplay = specialBonus;
                }
                break;

                case 2:
                //コンボボーナス
                comboBDisplay += (int)(200000 * Time.deltaTime);
                if (comboBDisplay > comboBonus) {
                    comboBDisplay = comboBonus;
                }
                break;
            }

            //スコア + スペシャルボーナス + コンボボーナス
            GameManager.score = previousScore + specialBDisplay + comboBDisplay;
        }

        source.volume = data.soundVolume * 0.75f;
    }

    IEnumerator Bonus() {
        yield return new WaitForSeconds(1f);

        step = 1;

        sound = true;
        while (specialBDisplay != specialBonus) {
            yield return null;
        }
        sound = false;

        yield return new WaitForSeconds(1f);

        step = 2;

        sound = true;
        while (comboBDisplay != comboBonus) {
            yield return null;
        }
        sound = false;

        yield return new WaitForSeconds(3f);

        //タイトル画面に戻る
        SceneManager.LoadScene("Ranking");
    }

    IEnumerator TimeAttackBonus() {
        yield return new WaitForSeconds(1f);

        step = 1;

        sound = true;
        while (timeBDisplay != timeBonus) {
            yield return null;
        }
        sound = false;

        yield return new WaitForSeconds(1f);

        step = 2;

        sound = true;
        while (colorRDisplay != colorRate) {
            yield return null;
        }
        sound = false;

        yield return new WaitForSeconds(1f);

        step = 3;

        sound = true;
        while (comboRDisplay != (float)GameManager.maxCombo) {
            yield return null;
        }
        sound = false;

        yield return new WaitForSeconds(3f);

        //タイトル画面に戻る
        SceneManager.LoadScene("Ranking");
    }

    IEnumerator BonusSound() {
        while (true) {
            if (sound) {
                source.Play();
                yield return new WaitForSeconds(0.075f);
            } else {
                yield return null;
            }
        }
    }
}
