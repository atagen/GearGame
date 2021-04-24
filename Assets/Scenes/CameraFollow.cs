//using System.Threading.Tasks.Dataflow;
using System.Transactions;
//using System.Numerics;
using System;
//using System.Threading.Tasks.Dataflow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Transform target;
  public float smoothSpeed = 5.0f;

  void LateUpdate()
  {
    transform.position = Vector3.Lerp(transform.position,
        target.position, smoothSpeed * Time.deltaTime);
    transform.position = new Vector3(transform.position.x, transform.position.y, -20.0f);
  }

}
