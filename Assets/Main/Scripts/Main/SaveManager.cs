using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public GameData game;
    public RankingData ranking;

    //セーブデータ
    [Serializable]
    public class SaveData {
        public float musicVolume;
        public float soundVolume;
        public bool showDescription;
        public RankingEntry[] normalEntries;
        public RankingEntry[] timeEntries;
    }
    SaveData saveData = new SaveData();

    public void SavePlayerData() {
        StreamWriter writer;
        //音量データを上書き
        saveData.musicVolume = game.musicVolume;
        saveData.soundVolume = game.soundVolume;
        //その他データを上書き
        saveData.showDescription = game.showDescription;
        //ランキングデータを上書き
        saveData.normalEntries = ranking.normalEntries.ToArray();
        saveData.timeEntries = ranking.timeEntries.ToArray();

        string jsonstr = JsonUtility.ToJson(saveData);

        writer = new StreamWriter(Application.persistentDataPath + "/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    public void LoadPlayerData() {
        if (File.Exists(Application.persistentDataPath + "/savedata.json")) {
            string datastr = "";
            StreamReader reader;

            reader = new StreamReader(Application.persistentDataPath + "/savedata.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            saveData = JsonUtility.FromJson<SaveData>(datastr); // ロードしたデータで上書き

            //音量データを読み込み
            game.musicVolume = saveData.musicVolume;
            game.soundVolume = saveData.soundVolume;
            //その他データを読み込み
            game.showDescription = saveData.showDescription;
            //ランキングデータを読み込み
            ranking.normalEntries = new List<RankingEntry>(saveData.normalEntries);
            ranking.timeEntries = new List<RankingEntry>(saveData.timeEntries);
        }
    }

    public void DeletePlayerData() {
        if (File.Exists(Application.persistentDataPath + "/savedata.json")) {
            //データの削除
            File.Delete(Application.persistentDataPath + "/savedata.json");
        }
        //ランキングの初期化
        ranking.reset();
    }
}
