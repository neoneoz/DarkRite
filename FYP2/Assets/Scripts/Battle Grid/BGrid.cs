using UnityEngine;
using System.Collections;

public class BGrid : MonoBehaviour {

	// Use this for initialization
    public int height;
    public GameObject content;
    public Unit unit = null;
    public Vector2 index;
    [HideInInspector]
    public BGrid prev;
    [HideInInspector]
    public int distance;
    public enum Gridstate
    {  
        MOVE,
        PATH,
        ATTACK,
        TARGET,
        VISIBLE,
        INACTIVE,
        
    }
    public Gridstate gridstate; 
    //public Vector3 pos;
	void Start () {
        gridstate = Gridstate.VISIBLE;
        //index = new Vector2(0, 0);
        
	}
	public void SetBGridIndex(Vector2 newindex)
    {
        index = newindex;
    }
    public void SetBGridPos(Vector3 newpos)
    {
        transform.position = newpos;
    }
	// Update is called once per frame




	void Update () {
	
	}
}
