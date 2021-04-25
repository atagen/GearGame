using System.Xml;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
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
    private Collider2D _hitbox;
    private SpriteRenderer _spriteRenderer;
    public Sprite _clawSpriteClosed;
    public Sprite _clawSpriteOpen;
    public Sprite _drillSpriteA;
    public Sprite _drillSpriteB;
    public PlayerItems inventory;
    
    private double _drillAnimationTimer = 0.0;
    private int _drillAnimationSpriteIdx = 1;

    // Start is called before the first frame update
    void Start()
    {
        _clawstate = ClawState.Idle;
        _hitbox = this.GetComponent<BoxCollider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _clawSpriteClosed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_clawstate == ClawState.Spinning)
        {
            _drillAnimationTimer += Time.deltaTime;
            if (_drillAnimationTimer > 0.1) {
                _drillAnimationTimer = 0;
                _drillAnimationSpriteIdx = 1 - _drillAnimationSpriteIdx;
                if(_drillAnimationSpriteIdx == 0)
                {
                    _spriteRenderer.sprite = _drillSpriteA;
                } else
                {
                    _spriteRenderer.sprite = _drillSpriteB;
                }
            }

        }
        // lmb drill, rmb fire/manipulate
        if (Input.GetMouseButtonDown(0))
        {
            _drillAnimationTimer = 0.0f;
            _clawstate = ClawState.Spinning;
            Debug.Log("lmb, " + _clawstate);
        } else if (_clawstate == ClawState.Spinning & Input.GetMouseButtonDown(1))
        {
            _clawstate = ClawState.Firing;
            Debug.Log("lmb+rmb, " + _clawstate);
        } else if (Input.GetMouseButtonUp(0))
        {
            _spriteRenderer.sprite = _clawSpriteClosed;
            _clawstate = ClawState.Idle;
            Debug.Log("released claw, " + _clawstate);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (_clawstate == ClawState.Spinning || _clawstate == ClawState.Firing)
        {

            // break walls
            Tilemap map = other.GetComponentInParent<Tilemap>();
            if (map != null)
            {
                Vector3Int tilePos = map.WorldToCell(_hitbox.bounds.center);
                if (map.GetTile(tilePos) != null)
                {
                    map.GetComponentInParent<DestroyTile>().KillTile(tilePos, _hitbox.bounds.center);
                    //map.SetTile(tilePos, null);
                }
            }

            // harvest minerals 
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
