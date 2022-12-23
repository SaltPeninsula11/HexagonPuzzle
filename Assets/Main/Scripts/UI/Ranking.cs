using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Ranking : MonoBehaviour
{
    [Header("データ")]
    public GameData data;
    public RankingData ranking;

    [Header("タイトル")]
    public Text title;

    [Header("結果発表画面")]
    public GameObject resultsScreen;
    public Text nameText;
    [HideInInspector] public int index;

    [Header("ランキング画面")]
    public GameObject rankingScreen;
    public GameObject valuesRow;

    [Header("ランキング画面")]
    public Text toggleButton;

    [HideInInspector] public bool timeAttack = false;

    //セーブデータ
    SaveManager save;

    void Start()
    {
        save = this.GetComponent<SaveManager>();
        save.LoadPlayerData();

        //結果発表画面へ移動
        if (data.gameOver) {
            Results();
            data.gameOver = false;
        }

        //10行の記録を生成
        Transform parent = valuesRow.transform.parent;
        int length = Math.Min(9, ranking.normalEntries.Count - 1);
        for (int i = 1; i <= length; i++) {
            RectTransform row = Instantiate(valuesRow, this.transform.position, Quaternion.identity, parent).GetComponent<RectTransform>();
            row.anchoredPosition = Vector2.down * 20f * i;
            row.gameObject.GetComponent<RecordValue>().index = i;
        }
    }

    void Update()
    {
        this.GetComponent<AudioSource>().volume = data.musicVolume * 0.4f;

        toggleButton.text = timeAttack ? "タイムアタック" : "通常";
    }

    public void Results() {
        title.text = "RESULTS";
        resultsScreen.SetActive(true);
        rankingScreen.SetActive(false);
    }

    public void NameEntry() {
        //10文字までの名前を入力をした後、
        //ランキングに登録する。
        //ただし、何も入力されていない場合は、「Nanashi」とする。
        string name = nameText.text == "" ? "Nanashi" : nameText.text;
        ranking.addEntry(index, (data.mode == GameMode.TimeAttack), name, GameManager.level, GameManager.jewels, GameManager.score);
        GoToRanking();
        save.SavePlayerData();
    }

    public void GoToRanking() {
        title.text = "RANKING";
        resultsScreen.SetActive(false);
        rankingScreen.SetActive(true);
    }

    public void GoToTitle() {
        SceneManager.LoadScene("Title");
    }

    public void Toggle() {
        timeAttack = !timeAttack;
    }
}
