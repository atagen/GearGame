using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
enum ClawState
{
    Idle,
    Spinning,
    Firing
}
public class ClawInteractions : MonoBehaviour
{
    private ClawState _clawstate;
    public PlayerItems inventory;

    // Start is called before the first frame update
    void Start()
    {
        _clawstate = ClawState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        // lmb drill, rmb fire/manipulate
        if (Input.GetMouseButtonDown(0))
        {
            _clawstate = ClawState.Spinning;
            Debug.Log("lmb, " + _clawstate);
        } else if (Input.GetMouseButtonDown(1))
        {
            _clawstate = ClawState.Firing;
            Debug.Log("rmb, " + _clawstate);
        } else {
            _clawstate = ClawState.Idle;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit something with claw");
        if (_clawstate == ClawState.Spinning)
        {
            Mineral min = other.GetComponent<Mineral>();
            Debug.Log("it was: " + min);
            if (min != null)
            {
                Debug.Log("drilled mineral for " + min.mineral_value);
                inventory.mineral_count += min.mineral_value;
            }
        }
    }

}
