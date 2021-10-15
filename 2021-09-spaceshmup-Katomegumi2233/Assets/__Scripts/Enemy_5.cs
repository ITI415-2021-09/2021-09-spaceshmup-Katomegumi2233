using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_5 : Enemy
{
    [Header("Set in Inspector: Enemy_5")]
    // # seconds for a full sine wave
    public float waveFrequency = 10;
    // sine wave width in meters
    public float waveWidth = 20;
    public float waveRotY = 45;

    private float x0; // The initial x value of pos
    private float birthTime;

    public GameObject herogo;

    // Use this for initialization
    void Start()
    {
        herogo = GameObject.FindWithTag("Hero");


        // Set x0 to the initial x position of Enemy_1
        x0 = pos.x;

        birthTime = Time.time;
    }

    // Override the Move function on Enemy
    public override void Move()
    {

        Vector3 direction1 = herogo.transform.position - transform.position;
   
        float angle = 360 - Mathf.Atan2(direction1.x, direction1.y) * Mathf.Rad2Deg;
      
        transform.eulerAngles = new Vector3(0, 0, angle);


        transform.position += direction1.normalized *Time.deltaTime*30;


    }
}
