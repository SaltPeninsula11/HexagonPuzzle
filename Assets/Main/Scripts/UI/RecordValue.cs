using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordValue : MonoBehaviour
{
    [Header("データ")]
    public Ranking r;
    public RankingData data;

    [HideInInspector] public int index = 0;

    [Header("テキスト")]
    public Text rank;
    public Text playerName;
    public Text level;
    public Text jewels;
    public Text score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<RankingEntry> ranking = r.timeAttack ? data.timeEntries : data.normalEntries;

        string rankStr = "th";
        switch (index) {
            case 0:
            rankStr = "st";
            rank.color = new Color(1f, 1f, 0f, 1f);
            break;

            case 1:
            rankStr = "nd";
            rank.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            break;

            case 2:
            rankStr = "rd";
            rank.color = new Color(0.8f, 0.35f, 0f, 1f);
            break;

            default:
            rank.color = Color.white;
            break;
        }
        rank.text = (index + 1).ToString() + rankStr;
        playerName.text = ranking[index].name;
        level.text = ranking[index].level.ToString("D2");
        jewels.text = ranking[index].jewels.ToString("D4");
        score.text = ranking[index].score.ToString("D8");
    }
}
