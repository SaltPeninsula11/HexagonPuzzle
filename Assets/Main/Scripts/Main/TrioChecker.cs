using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrioChecker : MonoBehaviour
{
    private int[,] shape1dirs = {
        {1, 0, 0, 1, 0, 0},
        {0, 1, 0, 0, 1, 0},
        {0, 0, 1, 0, 0, 1}
    };
    private int[,] shape2dirs = {
        {1, 0, 1, 0, 0, 0},
        {0, 1, 0, 1, 0, 0},
        {0, 0, 1, 0, 1, 0},
        {0, 0, 0, 1, 0, 1},
        {1, 0, 0, 0, 1, 0},
        {0, 1, 0, 0, 0, 1}
    };
    private int[,] shape3dirs = {
        {1, 1, 0, 0, 0, 0},
        {0, 1, 1, 0, 0, 0},
    };

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            int[] c = Check();
            for (int i = 0; i < c.Length; i++) {
                Debug.Log(i + " : " + c[i]);
            }
        }
    }

    public int[] Check() {
        int[] counts = {0, 0, 0};
        HexaManager[,] hexas = this.GetComponent<TrioManager>().stage.hexas;

        for (int r = 0; r < hexas.GetLength(0); r++) {
            for (int c = 0; c < hexas.GetLength(1); c++) {
                if (hexas[r, c] != null) {
                    if (hexas[r, c].id == 0) {
                        counts[0] += CountShapes(c, r, shape1dirs);
                        counts[1] += CountShapes(c, r, shape2dirs);
                        counts[2] += CountShapes(c, r, shape3dirs);
                    }
                } else {
                    continue;
                }
            }
        }

        return counts;
    }
    int CountShapes(int x, int y, int[,] shapes) {
        int count = 0;
        HexaManager[,] hexas = this.GetComponent<TrioManager>().stage.hexas;

        for (int r = 0; r < shapes.GetLength(0); r++) {
            int fitCount = 0;

            /* 要改善 */
            for (int d = 0; d < StageManager.directions.GetLength(0); d++) {
                int[] dir = {StageManager.directions[d, 0], StageManager.directions[d, 1]};
                    
                try {
                    HexaManager h = hexas[y - dir[1], x + dir[0]];
                    if (h.id == 0 && shapes[r, d] == 1) {
                        fitCount++;
                    }
                } catch {
                    continue;
                }
            }

            if (fitCount == 2) {
                count++;
            }
        }
        

        return count;
    }
}
