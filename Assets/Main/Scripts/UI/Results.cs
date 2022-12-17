using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour
{
    [Header("データ")]
    public GameData data;
    public RankingData ranking;
    public Ranking r;

    [Header("スコア")]
    public Text score;

    [Header("ランク")]
    public Sprite[] rankSprites = new Sprite[3];
    public Image topThree;
    public Text below;
    public GameObject outOfRank;
    private int rank = 11;

    [Header("名前を入力するか否か")]
    public GameObject nameEntry;
    public GameObject goToRanking;
    public InputField entry;
    // Start is called before the first frame update
    void Start()
    {
        List<RankingEntry> entries = ranking.normalEntries;
        if (data.mode == GameMode.TimeAttack) {
            entries = ranking.timeEntries;
        }
        r.timeAttack = (data.mode == GameMode.TimeAttack);

        int length = Math.Min(9, entries.Count - 1);
        for (int i = length; i >= 0; i--) {
            if (GameManager.score > entries[i].score) {
                rank--;
            } else {
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //スコア
        score.text = GameManager.score.ToString("D8");

        //ランク
        topThree.gameObject.SetActive(rank <= 3);
        below.gameObject.SetActive(rank >= 4 && rank <= 10);
        outOfRank.SetActive(rank >= 11);

        if (rank > 0 && rank < 4) {
            topThree.sprite = rankSprites[rank - 1];
        }
        below.text = rank.ToString() + "th";
        r.index = rank - 1;

        //名前を入力するか否か
        nameEntry.SetActive(rank <= 10);
        goToRanking.SetActive(rank >= 11);

        //名前の上限
        if (entry.text.Length > 10) {
            entry.text = entry.text.Substring(0, 10);
        }
    }
}
