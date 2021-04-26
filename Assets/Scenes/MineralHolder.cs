using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralHolder : MonoBehaviour
{
    private static Sprite[] _minerals;
    
    // Start is called before the first frame update
    void Start()
    {
        _minerals = Resources.LoadAll<Sprite>("");
        //Debug.Log(_minerals.Length);
    }

    public static Sprite GetRandom(int tier)
    {
        int max = _minerals.Length/(tier+1);
        int min = _minerals.Length/(tier+2);
        return _minerals[Random.Range(min, max)];
    }
}