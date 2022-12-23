using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty {
    Easy, Normal, Hard, Expert
}
public enum GameMode {
    Normal, TimeAttack
}

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject
{
    [Header("ゲームモード")]
    public Difficulty difficulty;
    public GameMode mode;
    public bool gameOver;

    [Header("タイムアタック")]
    public float timeLimit = 10f; //制限時間（秒単位）
    public int colors = 2;        //色の種類
    public int goalAmounts = 10;  //目標個数
    
    [Header("音量")]
    public float musicVolume;
    public float soundVolume;
}
