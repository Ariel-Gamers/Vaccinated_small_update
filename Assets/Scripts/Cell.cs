using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * A class representing a cell - and everything a cell has.
 * 
 */



public class Cell : MonoBehaviour
{


    [SerializeField] protected float timeSinceHit;
    [SerializeField] protected Type myType = Type.Cell;
    [SerializeField] protected float health; // the health of the cell
    [SerializeField] protected float damage; // the damage it inflicts when in combat
    [SerializeField] GameObject cell_prefab; // the prefab/image/etc
    //[SerializeField] Vector3 target_position; // maybe won't use, idk
    [SerializeField] protected bool in_combat;
    [SerializeField] protected int type; // for now, negative types are enemies, with the strength of type x => -x
    [SerializeField] protected float multiplier; // multiplier damage depending on type
    [SerializeField] protected float time;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        //StartCoroutine(Updater);
    }




    
    virtual protected void Init()
    {
        timeSinceHit = 0;
        health = 100;
        damage = 1;
        type = 1;
        multiplier = 1.0f;
        in_combat = false;
    }

    public float getHealth()
    {
        return health;
    }
    internal void fight(float damage)
    {
        health -= damage;
    }

    public Type getType()
    {
        return myType;
    }

    //IEnumerator Updater()
    //{
    //    time += Time.deltaTime;
    //    timeSinceHit += Time.deltaTime;
    //    if (health <= 0)
    //    {
    //        Destroy(this);
    //    }
    //    yield return new WaitForSeconds(0.2f);
    //}
    // Update is called once per frame
    void LateUpdate()
    {
        time += Time.deltaTime;
        timeSinceHit += Time.deltaTime;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
