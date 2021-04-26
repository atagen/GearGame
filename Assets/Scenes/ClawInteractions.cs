using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Pathfinding;

enum ClawState
{
    Idle,
    Spinning,
    Firing
}
public class ClawInteractions : MonoBehaviour
{
    public Sprite _clawSpriteClosed;
    public Sprite _clawSpriteOpen;
    public Sprite _drillSpriteA;
    public Sprite _drillSpriteB;
    public PlayerItems inventory;
    public double particle_freq;
    public double drill_timeout;
    public AstarPath _pathfinding;
    public TileBase undrillable_tile;

    private ClawState _clawstate;
    private Collider2D _hitbox;
    private SpriteRenderer _spriteRenderer;

    private AudioSource _audioSourceDrill;
    private AudioSource _audioSourceDig;

    private double _drillTimeout = 0.0;
    private double _particleTimeout = 0.0;
    
    private double _drillAnimationTimer = 0.0;
    private int _drillAnimationSpriteIdx = 1;

    // Start is called before the first frame update
    void Start()
    {
        _clawstate = ClawState.Idle;
        _hitbox = this.GetComponent<BoxCollider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _audioSourceDrill = this.GetComponents<AudioSource>()[0];
        _audioSourceDig = this.GetComponents<AudioSource>()[1];

        _pathfinding = FindObjectOfType<AstarPath>();
        
        _spriteRenderer.sprite = _clawSpriteClosed;
    }

    // Update is called once per frame
    void Update()
    {
        _drillTimeout += Time.deltaTime;
        _particleTimeout += Time.deltaTime;
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
            _clawstate = ClawState.Spinning;
            _drillAnimationTimer = 0.0f;
            _audioSourceDrill.Play();
        } else if (_clawstate == ClawState.Spinning & Input.GetMouseButtonDown(1))
        {
            _clawstate = ClawState.Firing;
            Debug.Log("lmb+rmb, " + _clawstate);
        } else if (Input.GetMouseButtonUp(0))
        {
            _clawstate = ClawState.Idle;
            _spriteRenderer.sprite = _clawSpriteClosed;
            _audioSourceDrill.Stop();
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
                Vector3 drillTip = other.ClosestPoint(_hitbox.bounds.center);
                //Vector3Int cell = Vector3Int.RoundToInt(drillTip);
               
                // new Vector3(_hitbox.bounds.center.x, _hitbox.bounds.max.y, _hitbox.bounds.center.z);

                if (_drillTimeout > drill_timeout)
                {
                    _drillTimeout = 0.0;
                    Vector3Int tilePos = map.WorldToCell(drillTip);
                    TileBase tile = map.GetTile(tilePos);
                    
                    if (tile == true && tile != undrillable_tile)
                    {
                        _audioSourceDig.Play();
                        //_pathfinding.Scan();
                        //_pathfinding.ScanAsync();
                        //_pathfinding.UpdateGraphs(_pathfinding.graph);
                        //Pathfinding.AstarPathEditor.MenuScan();
                        map.GetComponentInParent<DestroyTile>().KillTile(tilePos, drillTip);
                    }
                } else if (_particleTimeout > particle_freq)
                {
                    _particleTimeout = 0.0;
                    map.GetComponentInParent<DestroyTile>().ParticleDrop(drillTip);
                    _audioSourceDig.Play();
                }
            }

            // harvest minerals 
            Mineral min = other.GetComponent<Mineral>();
            if (min != null && min.GetType() == typeof(Mineral))
            {
                _audioSourceDig.Play();
                inventory.mineral_count += min.getValue();
                // particle system will play then destroy object
                other.GetComponent<BoxCollider2D>().enabled = false;
                other.GetComponent<ParticleSystem>().Play();
                other.GetComponent<SpriteRenderer>().enabled = false;
                // in the meantime, destroy the mineral so we can't double-mine it
                Destroy(min);
            }
        }
    }

}
