using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBox : MonoBehaviour
{
    [Header("データ")]
    public DescriptionBoxes data;
    private DescriptionBox current;
    
    [Header("表示内容")]
    public Image icon;
    public Text nameSpace;
    public Text explanation;

    [Header("ステップ")]
    public Transform stepParent;
    public GameObject segment;

    private CanvasGroup canvas;
    private int step = 0;
    private bool isOpening = false;

    private int segStep = 1;
    private int maxSeg = 1;
    
    void Start()
    {
        canvas = this.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (step < data.descriptions.Length) {
            if (!isOpening) {
                current = data.descriptions[step];
            }

            //表示内容を反映
            if (current.icon.Length > 0) {
                icon.sprite = current.icon[0];
            }
            nameSpace.text = current.name;
            explanation.text = current.explanation;

            if (!isOpening && maxSeg <= 1) {
                maxSeg = 0;
                segStep = 0;
                for (int i = 0; i < data.descriptions.Length; i++) {
                    if (GameManager.level >= data.descriptions[i].level && step <= i) {
                        segStep = 1;
                        maxSeg++;
                    }
                }
            }

            //説明を表示
            if (GameManager.level >= current.level && !isOpening) {
                isOpening = true;
                step++;
                Open();
            }
        }

        Vector2 segSize = segment.GetComponent<RectTransform>().sizeDelta * 1.5f;
        segment.GetComponent<RectTransform>().anchoredPosition = Vector3.left * (maxSeg - 1) * segSize.x * 0.5f;

        foreach (Transform obj in stepParent) {
            if ( 0 <= obj.gameObject.name.LastIndexOf("Clone") ) {
                Destroy(obj.gameObject);
            }
        }

        Color light;
        if (segStep == 1) {
            light = Color.yellow;
        } else {
            light = new Color(0.3f, 0.3f, 0f, 1f);
        }
        segment.GetComponent<Image>().color = light;

        for ( int i = 1; i < maxSeg ; i++ ) {
            RectTransform s = (RectTransform)Instantiate(segment).transform;
            s.SetParent(stepParent , false);
            s.localPosition = new Vector2(
                s.localPosition.x + s.sizeDelta.x * 1.5f * i, 0
            );
            
            if (segStep == (i + 1)) {
                light = Color.yellow;
            } else {
                light = new Color(0.3f, 0.3f, 0f, 1f);
            }

            s.GetComponent<Image>().color = light;
        }
    }

    void Open() {
        canvas.alpha = 1f;
        Time.timeScale = 0f;
    }
    public void Close() {
        segStep++;
        if (segStep > maxSeg) {
            maxSeg = 0;
        }

        isOpening = false;
        Time.timeScale = 1f;
        canvas.alpha = 0f;
    }
}
