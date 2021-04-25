using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineral : MonoBehaviour
{
    public float Mineral_value;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = MineralHolder.GetRandom();
    }

    public float getValue()
    {
        return Mineral_value;
    }
}