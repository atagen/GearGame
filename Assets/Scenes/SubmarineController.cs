using System.Threading;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public Vector2 move_speed;
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

        _rigidbody.AddForce(new Vector2(hmove, vmove) * move_speed, ForceMode2D.Force );
    }
}
