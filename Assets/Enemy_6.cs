using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_6 : Enemy
{
    // Enemy_3 will move following a Bezier curve, which is a linear
    // interpolation between more than two points.
    [Header("Set in Inspector: Enemy_6")]
    public float lifeTime = 5;

    [Header("Set Dynamically: Enemy_6")]
    public Vector3[] points;
    public float birthTime;


    public GameObject herogo;
    public GameObject bullet;

    public float cdtime;


    private void Start()
    {


       herogo= GameObject.FindWithTag("Hero");


        points = new Vector3[3]; // Initialize points

        // The start position has already been set by Main.SpawnEnemy()
        points[0] = pos;

        // Set xMin and xMax the same way that Main.SpawnEnemy() does
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        Vector3 v;
        // Pick a random middle position in the bottom half of the screen
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;

        // Pick a random final position above the top of the screen
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        // Set the birthTime to the current time
        birthTime = Time.time;
    }

    public override void Move()
    {
        // Bezier curves work based on a u value between 0 & 1
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            // This Enemy_3 has finished its life
            Destroy(this.gameObject);
            return;
        }

        // Interpolate the three Bezier curve points
        Vector3 p01, p12;
        u = u - (0.2f * Mathf.Sin(u * Mathf.PI * 2));
        p01 = ((1 - u) * points[0]) + (u * points[1]);
        p12 = ((1 - u) * points[1]) + (u * points[2]);
        pos = ((1 - u) * p01) + (u * p12);
    }


    private void Update()
    {
        if (cdtime <= 0.0f)
        {
            cdtime = 1.0f;
            send_bullet();
        }
        else
        {
            cdtime -= Time.deltaTime;
        }

    }

    #region ·¢Éä×Óµ¯
    public void send_bullet()
    {

      


        Projectile p;
        Vector3 vel = Vector3.down*10;
        

        p = MakeProjectile(); // Make middle Projectile
        p.rigid.velocity = vel;

        p = MakeProjectile(); // Make right Projectile
        p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
        p.rigid.velocity = p.transform.rotation * vel;


        p = MakeProjectile(); // Make left Projectile
        p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
        p.rigid.velocity = p.transform.rotation * vel;


        p = MakeProjectile(); // Make left Projectile
        p.transform.rotation = Quaternion.AngleAxis(20, Vector3.back);
        p.rigid.velocity = p.transform.rotation * vel;


        p = MakeProjectile(); // Make left Projectile
        p.transform.rotation = Quaternion.AngleAxis(-20, Vector3.back);
        p.rigid.velocity = p.transform.rotation * vel;




    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(bullet);
        
            go.tag = "Enemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        
        go.transform.position = transform.position;
      //  go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
       // p.type = type;
       // lastShotTime = Time.time;
        return p;
    }


    #endregion


}
