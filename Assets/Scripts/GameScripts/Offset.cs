using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using System;

public class Offset : MonoBehaviour
{
    private float countingTime;
    private float maxCountingTime;
    private float shakeAmplitude;
    private Vector3 initialPos;
    private float counter;
    private bool upordown;
    public float shakeSpeed;

    void Start()
    {
        initialPos = this.transform.position;
        MessageManager.Instance.Get<OffsetControlType>().RegisterHandler(OffsetControlType.Shake, Shake);
    }

    private void Shake(object sender, EventArgs e)
    {
        if (e is OffsetArgs)
        {
            var tp = e as OffsetArgs;
            countingTime = tp.time;
            maxCountingTime = tp.amplitude;
            shakeAmplitude = tp.amplitude;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (countingTime > 0)
        {
            if (counter > 1 / shakeSpeed)
            {
                transform.position = initialPos;
                float angle = UnityEngine.Random.Range(80f, 100f) * Mathf.PI / 180;
                float distance = (upordown ? 1 : -1) * shakeAmplitude * (countingTime / maxCountingTime);
                float x = Mathf.Cos(angle) * distance;
                float y = Mathf.Sin(angle) * distance;
                transform.position += (Vector3)new Vector2(x, y);
                counter -= 1 / shakeSpeed;
            }
            if (upordown)
            {
                upordown = false;
            }
            else
            {
                upordown = true;
            }
            counter += Time.unscaledDeltaTime;
            countingTime -= Time.unscaledDeltaTime;
        }
        else
        {
            countingTime = 0;
            transform.position = initialPos;
        }
    }
}

public enum OffsetControlType
{
    Shake,
    Others,
}

public class OffsetArgs: EventArgs
{
    public float time;
    public float amplitude;

    public OffsetArgs(float t, float a)
    {
        time = t;
        amplitude = a;
    }
}

