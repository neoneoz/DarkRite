using UnityEngine;
using System.Collections;

public class Vitae : GridAttack
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void DoEffect()
    {
        //healing here
    }
     public override void DoAttack(BGrid target)
    {
        PlayAnimation(target);

        if (target.unit != null)
        {
            Debug.Log("healing");
            target.unit.takeheal(potency);
            DoEffect();
        }
    }
}
