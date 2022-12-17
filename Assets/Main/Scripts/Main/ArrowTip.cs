using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTip : MonoBehaviour
{
    private bool trigger = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position += transform.up * 0.5f;

        if (!this.GetComponent<Renderer>().isVisible && !trigger) {
            trigger = true;
            StartCoroutine(destroyThis());
        }
    }

    IEnumerator destroyThis() {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
