using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    private SpriteRenderer background;
    private int currentBG = 0;
    private bool fade = false;
    private float fadeOpacity = 1.0f;

    [Header("データ")]
    public GameData data;
    [Header("背景")]
    public Sprite[] sprites = new Sprite[5];

    void Start()
    {
        background = this.GetComponent<SpriteRenderer>();

        if (data.mode == GameMode.TimeAttack) {
            background.sprite = sprites[data.colors - 2];
        } else {
            background.sprite = sprites[0];
        }
    }

    void Update()
    {
        //15レベルごとに背景を変える
        if (Math.Floor(GameManager.level / 15f) > currentBG && currentBG < sprites.Length - 1 && data.mode != GameMode.TimeAttack) {
            currentBG = (int)Math.Floor(GameManager.level / 15f);
            StartCoroutine("ChangeBG");
        }

        //フェード
        if (fade) {
            fadeOpacity -= 2 * Time.deltaTime;
        } else {
            fadeOpacity += 2 * Time.deltaTime;
        }
        fadeOpacity = Mathf.Clamp(fadeOpacity, 0f, 1f);

        background.color = new Color(1f, 1f, 1f, fadeOpacity);
    }

    IEnumerator ChangeBG() {
        //フェード演出
        fade = true;

        yield return new WaitForSeconds(1.5f);
        background.sprite = sprites[currentBG];

        fade = false;
    }
}
