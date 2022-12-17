using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RankingEntry {
    public string name;
    public int level;
    public int jewels;
    public int score;

    public RankingEntry(string n, int l, int j, int s) {
        name = n;
        level = l;
        jewels = j;
        score = s;
    }
}

[CreateAssetMenu(fileName = "RankingData", menuName = "ScriptableObjects/RankingData")]
public class RankingData : ScriptableObject
{
    public List<RankingEntry> normalEntries = new List<RankingEntry>() {
        new RankingEntry("ABCDEFGHIJ", 27, 800, 100000),
        new RankingEntry("BCDEFGHIJK", 25, 720, 90000),
        new RankingEntry("CDEFGHIJKL", 22, 640, 80000),
        new RankingEntry("DEFGHIJKLM", 19, 560, 70000),
        new RankingEntry("EFGHIJKLMN", 17, 480, 60000),
        new RankingEntry("FGHIJKLMNO", 14, 400, 50000),
        new RankingEntry("GHIJKLMNOP", 11, 320, 40000),
        new RankingEntry("HIJKLMNOPQ", 9, 240, 30000),
        new RankingEntry("IJKLMNOPQR", 6, 160, 20000),
        new RankingEntry("JKLMNOPQRS", 3, 80, 10000)
    };
    public List<RankingEntry> timeEntries = new List<RankingEntry>() {
        new RankingEntry("ABCDEFGHIJ", 1, 20, 100000),
        new RankingEntry("BCDEFGHIJK", 1, 19, 90000),
        new RankingEntry("CDEFGHIJKL", 1, 18, 80000),
        new RankingEntry("DEFGHIJKLM", 1, 17, 70000),
        new RankingEntry("EFGHIJKLMN", 1, 16, 60000),
        new RankingEntry("FGHIJKLMNO", 1, 15, 50000),
        new RankingEntry("GHIJKLMNOP", 1, 14, 40000),
        new RankingEntry("HIJKLMNOPQ", 1, 13, 30000),
        new RankingEntry("IJKLMNOPQR", 1, 12, 20000),
        new RankingEntry("JKLMNOPQRS", 1, 11, 10000)
    };

    public void reset() {
        normalEntries = new List<RankingEntry>() {
            new RankingEntry("ABCDEFGHIJ", 27, 800, 100000),
            new RankingEntry("BCDEFGHIJK", 25, 720, 90000),
            new RankingEntry("CDEFGHIJKL", 22, 640, 80000),
            new RankingEntry("DEFGHIJKLM", 19, 560, 70000),
            new RankingEntry("EFGHIJKLMN", 17, 480, 60000),
            new RankingEntry("FGHIJKLMNO", 14, 400, 50000),
            new RankingEntry("GHIJKLMNOP", 11, 320, 40000),
            new RankingEntry("HIJKLMNOPQ", 9, 240, 30000),
            new RankingEntry("IJKLMNOPQR", 6, 160, 20000),
            new RankingEntry("JKLMNOPQRS", 3, 80, 10000)
        };
        timeEntries = new List<RankingEntry>() {
            new RankingEntry("ABCDEFGHIJ", 1, 20, 100000),
            new RankingEntry("BCDEFGHIJK", 1, 19, 90000),
            new RankingEntry("CDEFGHIJKL", 1, 18, 80000),
            new RankingEntry("DEFGHIJKLM", 1, 17, 70000),
            new RankingEntry("EFGHIJKLMN", 1, 16, 60000),
            new RankingEntry("FGHIJKLMNO", 1, 15, 50000),
            new RankingEntry("GHIJKLMNOP", 1, 14, 40000),
            new RankingEntry("HIJKLMNOPQ", 1, 13, 30000),
            new RankingEntry("IJKLMNOPQR", 1, 12, 20000),
            new RankingEntry("JKLMNOPQRS", 1, 11, 10000)
        };
    }

    public void addEntry(int index, bool timeAttack, string name, int level, int jewels, int score) {
        if (timeAttack) {
            timeEntries.Insert(index, new RankingEntry(name, level, jewels, score));
        } else {
            normalEntries.Insert(index, new RankingEntry(name, level, jewels, score));
        }
    }
}
