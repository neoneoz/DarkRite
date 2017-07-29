using UnityEngine;
using System.Collections;

public class TransitionArea : MonoBehaviour {

	// Use this for initialization
    public float movetimer;
    public GameObject nextarea;
    public Vector3 direction;
    public bool triggered,isExit;


	void Start () {
        movetimer = 0;
        triggered = false;
        isExit = false;
	}
	
	// Update is called once per frame
	void Update () {



 
        if(isExit)
        {   
            LeaveTriggerArea();
            return;    
        }



        if (triggered && SharedData.instance.player.GetComponent<Movement>().forcemoveplayer(this.transform.position))//upon being triggered move player 
        {

            TransitionArea next = nextarea.transform.GetChild(0).GetComponent<TransitionArea>();
            next.isExit = true;
            SharedData.instance.player.GetComponent<Movement>().changeArea(next);
        }
	}
    void OnTriggerEnter2D(Collider2D other)//eneter transition area
    {
        
        if (other.gameObject.name == "the_player")
        {
            triggered = true;
            other.gameObject.GetComponent<Movement>().m_input = false;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "the_player")
        {
            movetimer = 0;
            triggered = false;
        }

    }

    void LeaveTriggerArea()//shift the player out of the transition area he just arrived at;
    {
        if (SharedData.instance.player.GetComponent<Movement>().forcemoveplayer(this.transform.position + -direction * 40))
        {
            //Debug.Log("exit sucess");
            SharedData.instance.player.GetComponent<Movement>().m_input = true;
            isExit = false;
        }
	
    }
}
