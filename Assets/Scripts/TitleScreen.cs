using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public int openingIndex = 0;
    public GameObject mainUI;
    public GameObject descriptions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* メイン */
        mainUI.SetActive(openingIndex == 0);

        /* 対戦～クレジット */
        descriptions.SetActive(openingIndex != 0);
        descriptions.GetComponent<Descriptions>().index = (openingIndex - 1);
    }

    /* メイン */
    public void GameStart(){
        SceneManager.LoadScene("HexagonPuzzle");
    }
    public void Competition() {
        openingIndex = 1;
    }
    public void Help(){
        openingIndex = 2;
    }
    public void Options(){
        openingIndex = 3;
    }
    public void Credits() {
        openingIndex = 4;
    }

    public void Quit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
        #endif
    }

    /* 対戦～クレジット */
    public void Back() {
        openingIndex = 0;
    }
}
