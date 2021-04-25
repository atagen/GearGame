using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    public float mineral_count;
    public int player_health;
    // Start is called before the first frame update
    void Start()
    {
        mineral_count = 0.0f;
        player_health = 3;
    }

}
