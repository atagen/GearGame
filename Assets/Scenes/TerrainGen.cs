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

    public Tilemap map;
    
    public TileBase ruleTile;
    public int min_neighbours;
    public int max_neighbours;
    public float threshold;
    private bool PerlinBool(Vector3 pos)
    {
        return (Mathf.PerlinNoise(pos.x, pos.y) > threshold) ? true : false;
    }
    
    // Start is called before the first frame update
    void Start()
    {

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
                
                if (occupied_neighbours > min_neighbours && occupied_neighbours < max_neighbours)
                {
                    for (int h = -1; h < 1; h++)
                    {
                        for (int v = -1; v < 1; v++)
                        {
                            Vector3Int npos = pos + new Vector3Int(h,v,0);
                            if ( PerlinBool(new Vector3((float)npos.x * 0.3f, (float)npos.y * 0.2f, 0)) )
                            {
                                Debug.Log("inserting tile at " + npos);
                                map.SetTile(npos, ruleTile);
                            } else
                            {
                                Debug.Log("destroying tile at " + npos);
                                map.SetTile(npos, null);
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
