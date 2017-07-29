using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HPbarHandler : MonoBehaviour {

    //public List<Unit> enemy, player;
	// Use this for initialization
   public GameObject hpbar;
   public List<healthbar> barlist;
   public Vector3 offset;
	void Start () {
    
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateHPbar(Unit newunit)
    {
            GameObject newhp = Instantiate(hpbar);
            newunit.healthbar = newhp.GetComponent<healthbar>();
            barlist.Add(newhp.GetComponent<healthbar>());
            newhp.transform.SetParent(SceneData.sceneData.canvas.transform,false);
                //.parent = SceneData.sceneData.canvas.transform;
            newunit.healthbar.placehealthbar(newunit);
            newunit.healthbar.setvalue(newunit.m_iHealth, newunit.m_iMaxHealth);
           // placeHPbar(newunit);
    }


}
