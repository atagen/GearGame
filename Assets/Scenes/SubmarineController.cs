
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
    private Rigidbody2D _rigidbody;
    public Vector2 move_speed;
    public float max_speed;
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
        _rigidbody.AddForce(new Vector2(hmove, vmove) * move_speed, ForceMode2D.Force );
        
        // limit speed
        if (_rigidbody.velocity.magnitude > max_speed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * max_speed;
        }

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

    }
}
