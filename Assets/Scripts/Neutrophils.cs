using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Neutrophils : Cell
{
    // Start is called before the first frame update
    GameObject target;
    GameSystem gs;

    GameObject ai;

    List<Type> strengths;
    void Start()
    {
        //StartCoroutine("getTarget");
        strengths = new List<Type>();
        gs = FindObjectOfType<GameSystem>();
        strengths.Add(Type.Rhinovirus);
        myType = Type.Neutrophil;

    }

    private IEnumerable getTarget()
    {
        while(target == null)
        {
            target = gs.getNearestThreat(transform, myType);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.CompareTag("BadCell"))
        {
            StartCoroutine(Engage(go));
        }

    }

    private IEnumerator Engage(GameObject go)
    {

        Cell c = go.GetComponent<Cell>();

        while(c != null)
        {
            if(timeSinceHit > 2)
            {
                c.fight(damage);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            target = gs.getNearestThreat(transform, myType);
            if(target != null)
            {
                GetComponent<AIDestinationSetter>().target = target.transform;
            }
        }
    }
}
