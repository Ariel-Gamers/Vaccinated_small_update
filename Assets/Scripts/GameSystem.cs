using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class GameSystem : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] int threats_in_level;
    [SerializeField] float time_to_spawn = 5;
    [SerializeField] float time;
    [SerializeField] float energy_timer;
    [SerializeField] float total_energy;
    float energy_level;
    [SerializeField] List<GameObject> infectious_cells;
    [SerializeField] List<GameObject> immunity_cells;
    [SerializeField] List<PolygonCollider2D> body_parts;
    [SerializeField] List<GameObject> active_threats; 
    float min_X, min_Y, max_X, max_Y;
    TMP_Text energyTMP;
    Button button;
    GameObject col;




    void Start()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        min_X = bounds.min.x;
        min_Y = bounds.min.y;
        max_X = bounds.max.x;
        max_Y = bounds.max.y;
        energy_timer = 0;
        energy_level = 0;


        energyTMP = GameObject.Find("Energy").GetComponent<TMP_Text>();
        threats_in_level = 30;
        col = GameObject.Find("Polygon_Collider");
        body_parts.Add(GameObject.Find("Body").GetComponent<PolygonCollider2D>());
        body_parts.Add(GameObject.Find("Head").GetComponent<PolygonCollider2D>());
        //StartCoroutine("RemoveNull()");
    }


    public void createCell(int type)
    {
        if(energy_level < 5)
        {
            return;
        }
        energy_level -= 5;
        GameObject go = immunity_cells.ElementAt(type);
        Vector3 v = new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), -1);
        while(true)
        {
            foreach (PolygonCollider2D pgc in body_parts)
            {
                if (pgc.OverlapPoint(v))
                {
                    Instantiate(go, v, Quaternion.identity);
                    Debug.Log("Overlap true: " + v.ToString());
                    return;
                }
                else
                {
                    v.x = Random.Range(min_X, max_X);
                    v.y = Random.Range(min_Y, max_Y);
                }
            }
        }
    }

    void InitThreat()
    {
        if(threats_in_level <= 0)
        {
            return;
        }
        threats_in_level--;
        int threat_to_spawn = (int)Random.Range(0, 2);
        if(threats_in_level > 26)
        {
            threat_to_spawn = 1;
        }
        GameObject go = infectious_cells.ElementAt(threat_to_spawn);
        Vector3 v = new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), -1);
        while (true)
        {
            foreach (PolygonCollider2D pgc in body_parts)
            {
                if (pgc.OverlapPoint(v))
                {
                    active_threats.Add(Instantiate(go, v, Quaternion.identity));
                    Debug.Log("Overlap true: " + v.ToString());
                    return;
                }
                else
                {
                    v.x = Random.Range(min_X, max_X);
                    v.y = Random.Range(min_Y, max_Y);
                }
            }
        }
    }

    public GameObject getNearestThreat(Transform cell_position, Type type)
    {
        float magnitude = float.MaxValue;
        GameObject ans = null;
        foreach(GameObject threat in active_threats)
        {
            Type threat_type = threat.GetComponent<Cell>().getType();
            float distance = Vector2.Distance(threat.transform.position, cell_position.position);
            if(type == Type.Neutrophil && threat_type == Type.Rhinovirus)
            {
                distance += 100; // arbitrary high amount
            }
            else if (type == Type.KillerT && threat_type == Type.Influenza)
            {
                distance += 100;
            }
            if(distance < magnitude)
            {
                Debug.Log("Found non-null threat");
                ans = threat;
                magnitude = distance;
            }
        }
        return ans;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        energy_timer += Time.deltaTime;


        if(time > time_to_spawn)
        {
            InitThreat();
            time_to_spawn = Random.Range(0.1f, 7);
            time = 0;
        }

        if(energy_timer > 2) // get energy every 2 sec
        {
            energy_level += Random.Range(2, 5);
            energy_timer = 0;
        }
        energyTMP.text = "ENERGY: " + energy_level;

        active_threats.RemoveAll(item => item == null);
        if(active_threats.Count > 12)
        {
            Application.Quit();
        }
        if(threats_in_level < 0)
        {
            Application.Quit();

        }

    }
}
