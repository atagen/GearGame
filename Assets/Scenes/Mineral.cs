using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineral : MonoBehaviour
{
    public float value;
    public float[] Mineral_values = {5f, 10f, 20f, 50f, 100f};
    public float[] chance_pct = {60f, 20f, 10f, 7.5f, 2.5f};
    void Start()
    {
        float roll = Random.Range(0f, 100f);

        int drop = 0;
        float cum = 0.0f;
        for (drop = 0; drop < chance_pct.Length-1; drop++)
        {
            cum += chance_pct[drop];
            if (roll <= cum)
            {
                break;
            }
        }
        
        value = Mineral_values[drop];

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = MineralHolder.GetRandom(drop);
    }

    public float getValue()
    {
        return value;
    }
}