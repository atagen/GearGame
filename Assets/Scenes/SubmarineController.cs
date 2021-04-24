
using System;
using System.Numerics;
using System.Transactions;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
public class SubmarineController : MonoBehaviour
{
    public Vector2 move_speed;
    public float max_speed;
    
    private Rigidbody2D _rigidbody;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var hmove = Input.GetAxis("Horizontal");
        var vmove = Input.GetAxis("Vertical");

        time += Time.deltaTime;

        _rigidbody.AddForce(new Vector2(hmove, vmove + Mathf.Sin(time * Mathf.PI) * 0.1f) * move_speed, ForceMode2D.Force );        

        // limit speed
        if (_rigidbody.velocity.magnitude > max_speed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * max_speed;
        }

        //todo: change this to work only for the texture and not flip the arm
/*
    
        if (hmove < 0.0f)
        {
            Quaternion rot = Quaternion.Euler(0.0f,180.0f,90.0f);
            transform.SetPositionAndRotation(transform.position, rot);
        }
        else if (hmove > 0.0f)
        {
            Quaternion rot = Quaternion.Euler(0.0f,0.0f,90.0f);
            transform.SetPositionAndRotation(transform.position, rot);
        }
*/
    }
}
