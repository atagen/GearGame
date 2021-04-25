using System.ComponentModel;
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
        } else if (_clawstate == ClawState.Spinning & Input.GetMouseButtonDown(1))
        {
            _clawstate = ClawState.Firing;
            Debug.Log("lmb+rmb, " + _clawstate);
        } else if (Input.GetMouseButtonUp(0))
        {
            _clawstate = ClawState.Idle;
            Debug.Log("released claw, " + _clawstate);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_clawstate == ClawState.Spinning)
        {
            Mineral min = other.GetComponent<Mineral>();
            if (min != null && min.GetType() == typeof(Mineral))
            {
                inventory.mineral_count += min.getValue();
                // particle system will play then destroy object
                other.GetComponent<ParticleSystem>().Play();
                other.GetComponent<SpriteRenderer>().enabled = false;
                // in the meantime, destroy the mineral so we can't double-mine it
                Destroy(min);
            }
        }
    }

}
