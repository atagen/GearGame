using System.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Transform target;
  public float smoothSpeed = 0.5f;
  private Vector3 velocity = Vector3.zero;

  void FixedUpdate()
  {
    var newpos = new Vector3(target.position.x,
        target.position.y, -20.0f);
    
    transform.position = Vector3.SmoothDamp(transform.position, newpos, ref velocity, smoothSpeed);
  }

}
