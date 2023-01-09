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
    public GameObject selectUI;
    public GameObject timeUI;
    public GameObject helpUI;
    public GameObject optionsUI;
    public GameObject creditsUI;

    [Header("説明ページ")]
    public GameObject[] helpPages;

    int helpPage = 0;

    void Awake()
    {
        helpPage = 0;
    }

    void Update()
    {
        /* タイトル */
        string txt = "";
        switch (index) {
            case 0:
            txt = "SELECT THE DIFFICULTY";
            break;

            case 1:
            txt = "TIME ATTACK";
            break;
            
            case 2:
            txt = "INSTRUCTIONS";
            break;
            
            case 3:
            txt = "OPTIONS";
            break;

            case 4:
            txt = "CREDITS";
            break;
        }
        title.text = txt;

        /* コンテンツ */
        //難易度選択
        selectUI.SetActive(index == 0);
        //タイムアタックモード
        timeUI.SetActive(index == 1);
        //ゲーム説明
        helpUI.SetActive(index == 2);
        for (int i = 0; i < helpPages.Length; i++) {
            helpPages[i].SetActive(helpPage == i);
        }
        //オプション
        optionsUI.SetActive(index == 3);
        //クレジット
        creditsUI.SetActive(index == 4);
    }

    public void NextPage(bool right) {
        if (right) {
            helpPage++;
            if (helpPage >= helpPages.Length) {
                helpPage = 0;
            }
        } else {
            helpPage--;
            if (helpPage < 0) {
                helpPage = helpPages.Length - 1;
            }
        }
    }
}
