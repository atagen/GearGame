using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    private Camera _camera;
    private Transform[] pivots;

    public Transform boat;

    public float[] lengths;
    public float[] minAngles;
    public float[] maxAngles;

    // Start is called before the first frame update
    void Start()
    {
        pivots = new Transform[lengths.Length];
        pivots[0] = transform.GetChild(0);
        float offs = lengths[0];
        for (int i = 1; i < lengths.Length; i++)
        {
            pivots[i] = pivots[i - 1].GetChild(0);
            pivots[i].position = new Vector3(offs, 0, 0);
            offs += lengths[i];
        }
        for (int i = 0; i < lengths.Length; i++)
        {
            Transform arm = pivots[i].GetChild(1);
            arm.localScale = new Vector3(lengths[i],0.2f,1);
            arm.localPosition = new Vector3(lengths[i] / 2, 0, 0);
        }

        _camera = Camera.main;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }

    private void Solve(Vector2 target)
    {
        Vector2[] p = new Vector2[lengths.Length + 1];
        p[0] = pivots[0].position;
        for (int i = 1; i < p.Length; i++)
        {
            p[i] = new Vector2(p[i-1].x, p[i-1].y - lengths[i-1]);
        }

        int steps = 0;
        float[] angles = new float[lengths.Length];
        while (Vector2.Distance(p[p.Length - 1], target) > .1 && steps++ < 30)
        {
            Vector2[] t = new Vector2[p.Length];
            t[p.Length - 1] = target;
            for (int i = p.Length - 2; i >= 0; i--)
            {
                t[i] = t[i + 1] + (p[i] - t[i + 1]).normalized * lengths[i];
            }
            for (int i = 1; i < p.Length; i++)
            {
                p[i] = t[i] - t[i - 1];
                angles[i - 1] = ClampAngle(Mathf.Atan2(p[i].y, p[i].x) * Mathf.Rad2Deg, minAngles[i-1], maxAngles[i-1]) * Mathf.Deg2Rad;
                p[i] = new Vector2(Mathf.Cos(angles[i - 1]), Mathf.Sin(angles[i - 1])) * lengths[i-1] + p[i-1];
            }
        }

        for (int i = 0; i < pivots.Length; i++)
        {
            pivots[i].eulerAngles = new Vector3(0, 0, angles[i] * Mathf.Rad2Deg);
        }
    }

// Update is called once per frame
    void Update()
    {
        transform.position = boat.position;
        Vector3 mouse = _camera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * _camera.transform.position.z);
        Solve(mouse);
    }
}
