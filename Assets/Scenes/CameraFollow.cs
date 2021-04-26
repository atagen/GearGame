using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Transform target;
  public float smoothSpeed = 0.5f;
  private Vector3 velocity = Vector3.zero;
  private float zoom_vel = 0.0f;
  public SubmarineController submarine;
  public float zoom_smooth;
  public float speed_scale;
  public float zoom_min;
  public float zoom_max;
  private float prev_zoom;
  //public float[] zooms = { 30.0f, 50.0f, 80.0f, 120.0f, 150.0f };

  void Start() {
    transform.position = new Vector3(transform.position.x, transform.position.y, -zoom_min);
    prev_zoom = transform.position.z;
  }
  void FixedUpdate()
  {
    //var fixz = transform.position.z;
    Vector2 targetSpeed = submarine.getSpeedInfo();
    /*
    int zoomlevel = Mathf.RoundToInt(targetSpeed.x / targetSpeed.y * zooms.Length * speed_scale);
    zoomlevel = Mathf.Min(zoomlevel, zooms.Length);
    Vector3 newpos = new Vector3(target.position.x,
        target.position.y, -zooms[zoomlevel]);
        */
    float zoom = Mathf.Min(zoom_max, Mathf.Max(zoom_min, targetSpeed.x*speed_scale));
    
    zoom = Mathf.SmoothDamp(prev_zoom, zoom, ref zoom_vel, zoom_smooth);
    prev_zoom = zoom;
    Vector3 newpos = new Vector3(target.position.x,
            target.position.y, -zoom);
   

    transform.position = Vector3.SmoothDamp(transform.position, newpos, ref velocity, smoothSpeed);
  }

}
