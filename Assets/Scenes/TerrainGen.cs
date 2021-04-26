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
    public float noise_scale;
    public int iterations;
    public bool exclusive;
    public float gem_chance;
    public Mineral gem;
    public int gem_neighbours;
    private int seed;
    private bool PerlinBool(Vector3 pos)
    {
        float noise = Mathf.PerlinNoise(pos.x + seed, pos.y + seed);
        return (noise > threshold) ? true : false;
    }
    
    Vector3 v3IntToFloat(Vector3Int v, float scale)
    {
        return new Vector3(v.x * scale, v.y * scale, 0);
    }

    bool checkLimits(int occupied, bool exclusive = false)
    {
        return (occupied > min_neighbours && occupied < max_neighbours)
                || 
                (exclusive && (occupied < min_neighbours || occupied > max_neighbours));
    }

    List<Vector3Int> analyseTerrain()
    {

        List<Vector3Int> changes = new List<Vector3Int>();

        Vector3Int offset = new Vector3Int(-map.cellBounds.min.x, -map.cellBounds.min.y, seed);
        
        int gemsSpawned = 0;

        for (int x = map.cellBounds.min.x+1; x < map.cellBounds.max.x-1; x++)
        {
            for (int y = map.cellBounds.min.y+1; y < map.cellBounds.max.y-1; y++)
            {
                Vector3Int pos = new Vector3Int(x,y,0);
                int occupied_neighbours = 0;
                
                // procedural terrain disturbance
                // only act on tiles that are occupied with our intended target already
                if (!exclusive && map.GetTile(pos) == ruleTile
                    ||
                    exclusive )
                {
                    

                    for (int h = -1; h < 2; h++)
                    {
                        for (int v = -1; v < 2; v++)
                        {
                            Vector3Int npos = pos + new Vector3Int(h,v,0);

                            if (!(h == 0 && v == 0) && map.GetTile(npos) == ruleTile)
                            {
                                occupied_neighbours++;
                            }
                        }
                    }


                    if (checkLimits(occupied_neighbours))
                    {
                        for (int h = -1; h < 2; h++)
                        {
                            for (int v = -1; v < 2; v++)
                            {
                                Vector3Int npos = pos + new Vector3Int(h,v,0);
                                if ( PerlinBool( v3IntToFloat(npos+offset, noise_scale)) )
                                    changes.Add(npos);
                            }
                        }
                        
                    }

                } // if tile exists
                

                //gem placement
                if (occupied_neighbours > gem_neighbours && Random.Range(0f, 1f) < gem_chance)
                    Instantiate(gem, map.CellToWorld(pos), Quaternion.Euler(0,0,Random.Range(0,360)));


            } // y
        } // x
        Debug.Log("spawned " + gemsSpawned + " gems");
        return changes;
    }

    // Start is called before the first frame update
    void Start()
    {
        seed = (int)UnityEngine.Random.Range(0, 65535);
        Debug.Log("generating terrain..");

        for (int it = 0; it < iterations; it++)
        {
            Debug.Log("performing terrain pass " + it + " ..");
            // analyse terrain and list required changes
            List<Vector3Int> changes = analyseTerrain();
            
            // run through required changes and make them
            changes.ForEach(delegate(Vector3Int v)
            {
                map.SetTile(v, ruleTile);
            });

            Debug.Log("changed " + changes.Count + " tiles"!);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
