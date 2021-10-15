using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {





    private float speed = 10f; // The speed in m/s


    private float x0; // The initial x value of pos
    private float birthTime;

    public float x_pos;

   


    public GameObject herogo;

 
    public GameObject targetgo;


    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;

    // This public property masks the field _type and takes action when it is set
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }




   

    private void Awake()
    {
        herogo = GameObject.FindWithTag("Hero");




        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }



    private void Start()
    {

        x0 = herogo.transform.position.x+x_pos;

        birthTime = Time.time;
    }

    private void Update()
    {




        switch (_type)
        {
            case WeaponType.phaser:

               Vector3 tempPos = transform.localPosition;

                //  float age = Time.time - birthTime;
                float age = Time.time - birthTime;
                float theta = Mathf.PI * 2 * age / 2f;
                float sin = Mathf.Sin(theta);
                tempPos.x = x0 + 2* sin;

                tempPos.x =  sin+x0;
    

                //rotate a bit about y
                Vector3 rot = new Vector3(0, sin*45, 0);
                transform.localRotation = Quaternion.Euler(rot);

                tempPos.y += speed * Time.deltaTime;
                 transform.position = tempPos;

                // print (bndCheck.isOnScreen);

                break;



            case WeaponType.SwivelGun:




                break;


            case WeaponType.laser:


                transform.localPosition = new Vector3(herogo.transform.position.x,herogo.transform.position.y+23,transform.position.z);

                break;

            case WeaponType.missile:


                if (targetgo == null)
                {
                  
                        GameObject[] ary = GameObject.FindGameObjectsWithTag("Enemy");

                        int index = UnityEngine.Random.Range(0, ary.Length);

                        targetgo = ary[index];



                }
                else
                {

                    Vector3 direction1 = targetgo.transform.position - transform.position;
              
                    float angle = 360 - Mathf.Atan2(direction1.x, direction1.y) * Mathf.Rad2Deg;
                   
                    transform.eulerAngles = new Vector3(0, 0, angle);


                    transform.position += direction1.normalized * Time.deltaTime * 30;


                }



                break;


        }




        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }

    ///<summary>
    /// Sets the _type private field and colors this projectile to match the
    /// WeaponDefinition.
    /// </summary>
    /// <param name="eType">The WeaponType to use.</param>
    public void SetType(WeaponType eType)
    {
        // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
