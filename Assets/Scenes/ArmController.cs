using System.Transactions;
using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ArmController : MonoBehaviour
{
    private Camera _camera;
    private Transform[] pivots;

    public Transform boat;
    public float[] lengths;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;

        pivots = new Transform[lengths.Length];
        pivots[0] = transform.GetChild(0);
        for (int i = 1; i < lengths.Length; i++)
        {
            pivots[i] = pivots[i - 1].GetChild(0);
            pivots[i].localPosition = new Vector3(0, -lengths[i-1], 0);
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }

    private void Solve(Vector2 target, float baseRotation)
    {
        Vector2[] p = new Vector2[lengths.Length + 1];
        p[0] = pivots[0].position;
        for (int i = 1; i < p.Length; i++)
        {
            float angle = (baseRotation - 45) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * lengths[i-1];
            p[i] = p[i-1] + dir;
        }

        int steps = 0;
        float[] angles = new float[lengths.Length];
        Vector2[] t = new Vector2[p.Length];
        t[p.Length - 1] = target;
        for (int i = p.Length - 2; i >= 0; i--)
        {
            t[i] = t[i + 1] + (p[i] - t[i + 1]).normalized * (lengths[i] + .1f);
        }
        for (int i = 1; i < p.Length; i++)
        {
            p[i] = t[i] - t[i - 1]; 
            angles[i - 1] = Mathf.Atan2(p[i].x, -p[i].y);
            p[i] = new Vector2(Mathf.Cos(angles[i - 1]), Mathf.Sin(angles[i - 1])) * lengths[i-1] + p[i-1];
        }

        for (int i = 0; i < pivots.Length; i++)
        {
            pivots[i].eulerAngles = new Vector3(0, 0, angles[i] * Mathf.Rad2Deg);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float rot = boat.eulerAngles.z;

        Vector3 mouse = _camera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * _camera.transform.position.z);
        
        Solve(mouse, rot);
    }
}
