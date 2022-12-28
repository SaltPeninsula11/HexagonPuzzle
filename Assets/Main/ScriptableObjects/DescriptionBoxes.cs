using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DescriptionBox {
    [Header("対応レベル")]
    public int level;
    [Header("表示する内容")]
    public Sprite[] icon;
    public string name;
    [TextArea] public string explanation;
}

[CreateAssetMenu(fileName = "DescriptionBoxes", menuName = "ScriptableObjects/DescriptionBoxes")]
public class DescriptionBoxes : ScriptableObject
{
    public DescriptionBox[] descriptions = new DescriptionBox[7];
}
