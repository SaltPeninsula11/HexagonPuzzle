using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleScreen : MonoBehaviour
{
    public int openingIndex = 0;
    public GameObject mainUI;
    public GameObject descriptions;

    [Header("データ")]
    public GameData data;
    public RankingData ranking;

    [Header("音量変更")]
    public Slider musicVolume;
    public Slider soundVolume;

    SaveManager save;
    // Start is called before the first frame update
    void Awake()
    {
        save = this.GetComponent<SaveManager>();
        save.LoadPlayerData();

        musicVolume.value = data.musicVolume * 100f;
        soundVolume.value = data.soundVolume * 100f;
    }

    // Update is called once per frame
    void Update()
    {
        /* メイン */
        mainUI.SetActive(openingIndex == 0);

        /* 対戦～クレジット */
        descriptions.SetActive(openingIndex != 0);
        descriptions.GetComponent<Descriptions>().index = (openingIndex - 1);

        this.GetComponent<AudioSource>().volume = data.musicVolume * 0.4f;

        data.musicVolume = musicVolume.value / 100f;
        data.soundVolume = soundVolume.value / 100f;
    }

    /* メイン */
    public void GameStart(){
        openingIndex = 1;
    }
    public void TimeAttack(){
        openingIndex = 2;
    }
    public void Help(){
        openingIndex = 3;
    }
    public void Options(){
        openingIndex = 4;
    }
    public void Credits() {
        openingIndex = 5;
    }

    public void Quit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
        #endif
    }

    /* ゲーム開始 */
    public void StartEasy(){
        data.difficulty = Difficulty.Easy;
        data.mode = GameMode.Normal;

        SceneManager.LoadScene("HexagonPuzzle");
    }
    public void StartNormal(){
        data.difficulty = Difficulty.Normal;
        data.mode = GameMode.Normal;

        SceneManager.LoadScene("HexagonPuzzle");
    }
    public void StartHard(){
        data.difficulty = Difficulty.Hard;
        data.mode = GameMode.Normal;

        SceneManager.LoadScene("HexagonPuzzle");
    }
    public void StartExpert(){
        data.difficulty = Difficulty.Expert;
        data.mode = GameMode.Normal;

        SceneManager.LoadScene("HexagonPuzzle");
    }

    public void Ranking() {
        SceneManager.LoadScene("Ranking");
    }

    /* 対戦～クレジット */
    public void Back() {
        if (openingIndex == 4) {
            save.SavePlayerData();
        }
        openingIndex = 0;
    }

    /* セーブデータ削除 */
    public void DeleteSaveData() {
        save.DeletePlayerData();
    }
}
