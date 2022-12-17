using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeAttackButton : MonoBehaviour
{
    [Header("データ")]
    public GameData data;

    [Header("値")]
    public Slider[] sliderValues = new Slider[3];

    // Update is called once per frame
    void Update()
    {
        data.timeLimit = sliderValues[0].value;
        data.goalAmounts = (int)sliderValues[1].value;
        data.colors = (int)sliderValues[2].value;
    }

    public void GameStart(){
        data.mode = GameMode.TimeAttack;

        SceneManager.LoadScene("HexagonPuzzle");
    }
}
