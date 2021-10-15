using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[System.Serializable]
public class Wave
{
    public List<GameObject> enemygo;
    public float curtime;



}



public class Main : MonoBehaviour {

    static public Main S; // A singleton for Main
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies; // Array of Enemy prefabs
    public float enemySpawnPerSecond = 0.5f; // # Enemies/second
    public float enemyDefaultPadding = 1.5f; // Padding for position
    public WeaponDefinition[] weaponDefinitions;
    public Wave [] wave_ary;
    public int wave_index;
    public Wave cur_wave;

    public int level;

    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield, WeaponType.missile, WeaponType.phaser, WeaponType.shield, WeaponType.SwivelGun
    };

    private BoundsCheck bndCheck;

    public void ShipDestroyed( Enemy e)
    {
        // Potentially generate a PowerUp
        if (Random.value <= e.powerUpDropChance)
        {
            // Choose which PowerUp to pick
            // Pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            pu.SetType(puType);

            // Set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }

    private void Awake()
    {

        level = 1;

        S = this;
        // Set bndCheck to reference the BoundsCheck component on this GameObject
        bndCheck = GetComponent<BoundsCheck>();

        // Invoke SpawnEnemy() once (in 2 seconds, based on default values)
       // Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        // A generic Dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }

        wave_index = -1;
        cur_wave = null;
    }







    private void Update()
    {



        if (cur_wave == null)
        {
            wave_index += 1;

            if (wave_index <= wave_ary.Length)
            {
           
               
                cur_wave = wave_ary[wave_index];
                SpawnEnemy();
            }
            else
            {
         
     
                level += 1;

                for(int i = 0; i < wave_ary.Length; i++)
                {
                    wave_ary[i].curtime = 1.0f;

                }

                wave_index = -1;
            }
    


        }
        else
        {
            if (cur_wave.curtime <= 0.0f)
            {
                cur_wave = null;

            }
            else
            {
                cur_wave.curtime -= Time.deltaTime;

            }


        }





    }






    public void SpawnEnemy()
    {
        for(int i = 0; i < level; i++)
        {
            // Pick a random Enemy prefab to instantiate
            int ndx = Random.Range(0, cur_wave.enemygo.Count);
            GameObject go = Instantiate<GameObject>(cur_wave.enemygo[ndx]);

            // Position the ENemy above the screen with a random x position
            float enemyPadding = enemyDefaultPadding;
            if (go.GetComponent<BoundsCheck>() != null)
            {
                enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
            }

            // Set the initial position for the spawned Enemy
            Vector3 pos = Vector3.zero;
            float xMin = -bndCheck.camWidth + enemyPadding;
            float xMax = bndCheck.camWidth - enemyPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = bndCheck.camHeight + enemyPadding;
            go.transform.position = pos;


        }


        //// Invoke SpawnEnemy() again
        //Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }



    //public void SpawnEnemy()
    //{
    //    // Pick a random Enemy prefab to instantiate
    //    int ndx = Random.Range(0, prefabEnemies.Length);
    //    GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

    //    // Position the ENemy above the screen with a random x position
    //    float enemyPadding = enemyDefaultPadding;
    //    if (go.GetComponent<BoundsCheck>() != null)
    //    {
    //        enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
    //    }

    //    // Set the initial position for the spawned Enemy
    //    Vector3 pos = Vector3.zero;
    //    float xMin = -bndCheck.camWidth + enemyPadding;
    //    float xMax = bndCheck.camWidth - enemyPadding;
    //    pos.x = Random.Range(xMin, xMax);
    //    pos.y = bndCheck.camHeight + enemyPadding;
    //    go.transform.position = pos;

    //    // Invoke SpawnEnemy() again
    //    Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    //}

    public void DelayedRestart(float delay)
    {
        // Invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        // Reload _Scene_0 to restart the game
        SceneManager.LoadScene("_Scene_0");
    }
    ///<summary>
    ///Static function that gets a WeaponDefinition from the WEAP_DICT static
    ///protected field of the main class.
    /// </summary>
    /// <returns>The WeaponDefinition or, if there is no WeaponDefinition with
    /// the WeaponType passed in, returns a new WeaponDefinition with a
    /// WeaponType of none..</returns>
    /// <param name="wt">The WeaponType of the desired WeaponDefinition</param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        // Check to make sure that the key exists in the Dictionary
        // Attempting to retrieve a key that didn't exist would throw an error,
        // so the following if statement is important.
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        // This returns a new WeaponDefinition with a type of WeaponType.none,
        // which means it has failed to find the right WeaponDefinition
        return new WeaponDefinition();
    }
}
