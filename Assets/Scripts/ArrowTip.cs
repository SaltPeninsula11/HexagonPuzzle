using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position += transform.up * 0.5f;

        if (!this.GetComponent<Renderer>().isVisible) {
            Destroy(gameObject);
        }
    }
}
