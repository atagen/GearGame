using System.Diagnostics;
using System.Net.Mail;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
public class TerrainGen : MonoBehaviour
{

    Tilemap map;
    private bool PerlinBool(float x, float y)
    {
        return (Mathf.PerlinNoise(x, y) > 0.5f) ? true : false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        Debug.Log("generating terrain..");
        for (int x = map.cellBounds.min.x+1; x < map.cellBounds.max.x-1; x++)
        {
            for (int y = map.cellBounds.min.y+1; y < map.cellBounds.max.y-1; y++)
            {
                Vector3Int pos = new Vector3Int(x,y,0);
                Tile thisTile = map.GetTile<Tile>(pos);
                int occupied_neighbours = 0;
                for (int h = -1; h < 1; h++)
                {
                    for (int v = -1; v < 1; v++)
                    {
                        Vector3Int npos = pos + new Vector3Int(h,v,0);
                        if (map.GetTile<Tile>(pos+npos) == true)
                        {
                            occupied_neighbours++;
                        }
                    }
                }

                if (occupied_neighbours > 2)
                {
                    for (int h = -1; h < 1; h++)
                    {
                        for (int v = -1; v < 1; v++)
                        {
                            Vector3Int npos = pos + new Vector3Int(h,v,0);
                            if (PerlinBool(x+h,y+v))
                            {
                                map.SetTile(pos+npos, thisTile);
                            }
                        }
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
