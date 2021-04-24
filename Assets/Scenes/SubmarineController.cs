using System.Threading;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    private Vector2 _velocity;
    public Vector2 v_move_speeds; // up and down movespeeds
    public float friction;
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var hmove = Input.GetAxis("Horizontal");
        var vmove = Input.GetAxis("Vertical");
        vmove *= (vmove >= 0.0f) ? v_move_speeds.x : v_move_speeds.y;
        _velocity += new Vector2(hmove, vmove);
        _rigidbody.AddForce(new Vector2(_velocity.x, _velocity.y) * Time.deltaTime, ForceMode.Acceleration );
        _velocity *= new Vector2(friction,friction);
    }
}
