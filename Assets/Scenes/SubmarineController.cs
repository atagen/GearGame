using System.Xml.Resolvers;
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

    public float turn_speed;
    public float move_speed;
    public float max_speed;
    public float bob_amount;

    private Rigidbody2D _rigidbody;
    private float time = 0.0f;

    public Mineral m;

    public Vector2 getSpeedInfo()
    {
        return new Vector2(_rigidbody.velocity.magnitude, max_speed);
    }

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
       
        _rigidbody.AddTorque(-hmove * Mathf.Sign(vmove) * turn_speed);

        Vector2 xy;
        if (vmove > 0.0001f || vmove < -0.0001f)
        {
            var a = Mathf.Deg2Rad * transform.eulerAngles.z;
            xy = new Vector2(Mathf.Cos(a) * vmove, Mathf.Sin(a) * vmove);
        } else {
            xy = new Vector2(0.0f, 0.0f);
        }

        // helps us bob up and down slightly
        time += Time.deltaTime;

        _rigidbody.AddForce(new Vector2(xy.x * move_speed, xy.y * move_speed + Mathf.Sin(time * Mathf.PI) * bob_amount), ForceMode2D.Force );
        
        // limit speed
        if (_rigidbody.velocity.magnitude > max_speed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * max_speed;
        }
    }

}
