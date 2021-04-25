using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FishGraphics : MonoBehaviour
{
    public AIPath  aiPath;
    // Start is called before the first frame update
    void Start()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
        else if(aiPath.desiredVelocity.x <= -0.01f)
            transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
