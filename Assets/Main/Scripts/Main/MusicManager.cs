using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource music;
    private bool changedMusic = false;
    private int musicStep = 0;
    private float musicOpacity = 1.0f;

    [Header("データ")]
    public GameData data;
    [Header("通常BGM")]
    public AudioClip[] musics = new AudioClip[5];
    public AudioClip timeAttackMusic;
    [Header("クリアBGM")]
    public AudioClip clearMusic;

    void Start()
    {
        music = this.GetComponent<AudioSource>();

        if (data.mode == GameMode.TimeAttack) {
            music.clip = timeAttackMusic;
            music.Play();
        } else {
            music.clip = musics[0];
            music.Play();
        }
    }

    void Update()
    {
        //音量
        music.volume = data.musicVolume * 0.4f * musicOpacity;

        //レベルに応じてBGMが変わる
        if (data.mode != GameMode.TimeAttack && musicStep < musics.Length - 1) {
            if ((int)Math.Floor(GameManager.level / 15f) > musicStep) {
                musicStep++;
                music.clip = musics[musicStep];
                music.Play();
            }
        }

        //急げ！！
        if (data.mode == GameMode.TimeAttack && GameManager.timeLimit <= 10.0f && !GameManager.cleared) {
            music.pitch = 1.25f;
        } else {
            music.pitch = 1f;
        }

        //クリアBGM
        if (GameManager.cleared && !changedMusic) {
            changedMusic = true;
            ClearMusic();
        }

        //ゲームオーバー
        if (GameManager.gameOver) {
            musicOpacity -= 2f * Time.deltaTime;
        }
    }

    public void ClearMusic() {
        //クリア時に流れるBGM
        music.clip = clearMusic;
        music.loop = false;
        music.Play();
    }
}
