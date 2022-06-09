using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Descriptions : MonoBehaviour
{
    public int index;
    [Header("タイトル")]
    public Text title;
    [Header("コンテンツ")]
    public GameObject competitionUI;
    public GameObject helpUI;
    public GameObject optionsUI;
    public GameObject creditsUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* タイトル */
        string txt = "";
        switch (index) {
            case 0:
            txt = "COMPETITION";
            break;
            
            case 1:
            txt = "INSTRUCTIONS";
            break;
            
            case 2:
            txt = "OPTIONS";
            break;

            case 3:
            txt = "CREDITS";
            break;
        }
        title.text = txt;

        /* コンテンツ */
        //対戦モード
        competitionUI.SetActive(index == 0);
        //ゲーム説明
        helpUI.SetActive(index == 1);
        //オプション
        optionsUI.SetActive(index == 2);
        //クレジット
        creditsUI.SetActive(index == 3);
    }
}
