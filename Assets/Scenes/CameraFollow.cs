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
    var fixz = transform.position.z;
    var newpos = new Vector3(target.position.x,
        target.position.y, fixz);
    
    transform.position = Vector3.SmoothDamp(transform.position, newpos, ref velocity, smoothSpeed);
  }

}
