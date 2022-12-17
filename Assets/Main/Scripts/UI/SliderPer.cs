using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderPer : MonoBehaviour
{
    public int perValue = 5;
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void OnValuechanged()
    {
        try {
            // まずスライダーの値を取得し...
            float sliderValue = slider.value;

            //nで割って整数に丸めてn倍してやり、n分ごとの値とする
            sliderValue = Mathf.Round(sliderValue / perValue) * perValue;

            // OnValueChangedを発生させずにスライダーの値を変更する
            slider.SetValueWithoutNotify(sliderValue);
        } catch {
            return;
        }
    }
}
