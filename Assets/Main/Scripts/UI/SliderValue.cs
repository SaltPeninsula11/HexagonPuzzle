using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public Slider slider;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = slider.value.ToString();
    }
}
