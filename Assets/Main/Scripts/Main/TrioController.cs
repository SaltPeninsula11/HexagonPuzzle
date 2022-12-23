using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrioController : MonoBehaviour
{
    [HideInInspector] public bool control = true;
    private float time;
    
    void Update()
    {
        if (Time.timeScale <= 0f) {
            //ポーズされているとき
            return;
        }

        TrioManager trioManager = GetComponent<TrioManager>();
        
        if (control && !GameManager.gameOver && !GameManager.cleared){
            //操作可能の時
            float[] view_buttons = new float[2];
            view_buttons[0] = Input.GetAxis("Horizontal");
            view_buttons[1] = Input.GetAxis("Vertical");
            
            if (Math.Abs(view_buttons[0]) > 0 || Math.Abs(view_buttons[1]) > 0){
                if (time > 0){
                    time -= Time.deltaTime;
                }
            } else {
                time = 0.01f;
            }

            if (view_buttons[0] < -0.05 && view_buttons[1] <= 0 && time <= 0.0f){
                //左に移動
                trioManager.Move(-1, 0);
                time = 0.3f;
            } else if (view_buttons[0] < -0.05 && view_buttons[1] > 0 && time <= 0.0f){
                //左上に移動
                trioManager.Move(-1, 1);
                time = 0.3f;
            } else if (view_buttons[0] > 0.05 && view_buttons[1] >= 0 && time <= 0.0f){
                //右に移動
                trioManager.Move(1, 0);
                time = 0.3f;
            } else if (view_buttons[0] > 0.05 && view_buttons[1] < 0 && time <= 0.0f){
                //右下に移動
                trioManager.Move(1, -1);
                time = 0.3f;
            } else if (view_buttons[1] > 0 && time <= 0.0f){
                //上に移動
                trioManager.Move(0, 1);
                time = 0.3f;
            } else if (view_buttons[1] < 0 && time <= 0.0f){
                //下に移動
                trioManager.Move(0, -1);
                time = 0.3f;
            }

            if (Input.GetButtonDown("A")){
                trioManager.Rotate();
            }
            
            if (Input.GetButtonDown("B")){
                trioManager.Drop();
            }
        }
    }
}