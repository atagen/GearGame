using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
public class DestroyTile : MonoBehaviour
{
    public Tilemap map;
    public GameObject Psys; // particle system to execute on death
    public void KillTile(Vector3Int tilePos, Vector3 worldPos)
    {
        map.SetTile(tilePos, null);
        ParticleDrop(worldPos);
    }

    public void ParticleDrop(Vector3 worldPos)
    {
        Instantiate(Psys, worldPos, Quaternion.identity);
    }

}
