using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Encounter : MonoBehaviour {

	// Use this for initialization
    public List<Unit> encounterlist;
    public float distance,probability = 0;
    public float result,tilesize;
    public int min, max;
    public Unit unit;

	void Start () {
        tilesize = tilesize * 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void CheckEncounter(float dist)
    {
        
        if(distance < tilesize*40)
            distance += dist/2;

        probability =100* ( distance / (tilesize * 50));
        
        result = Random.Range(30, 370);
        //Debug.Log(result);
        if (result < probability)
        {
            Debug.Log("ENCOUNTERED MONSTER");
            distance = 0;
            GenerateEncounter();
        }
    



    }
    public void GenerateEncounter()
    {
        
        int count = Random.Range(min, max);
        for(int i = 0; i < count;i++)
        {
            encounterlist.Add(unit);
        }
        Scenemanager.instance.LoadBattleScene();

    }


}
