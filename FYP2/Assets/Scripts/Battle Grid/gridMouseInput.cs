using UnityEngine;
using System.Collections;

public class gridMouseInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {

        switch(GetComponent<BGrid>().gridstate)
        {
            case BGrid.Gridstate.MOVE :
                //Debug.Log("pathing");
                SceneData.sceneData.gridarray.RenderPathForGrid(GetComponent<BGrid>());
                break;
            case BGrid.Gridstate.ATTACK:
                //Debug.Log("pathing");
                SceneData.sceneData.gridarray.SetGridTrget(GetComponent<BGrid>());
                break;
        }
        
    }
    void OnMouseExit()
    {
        //Debug.Log("de-pathing");
        switch (GetComponent<BGrid>().gridstate)
        {
            case BGrid.Gridstate.PATH:
                //Debug.Log("de-pathing");
                SceneData.sceneData.gridarray.RenderPathForGrid(GetComponent<BGrid>(), true);
                break;
            case BGrid.Gridstate.TARGET:
                SceneData.sceneData.gridarray.SetGridTrget(GetComponent<BGrid>(),true);
                //Debug.Log("pathing");
               // SceneData.sceneData.gridarray.RenderPathForGrid(GetComponent<BGrid>());
                break;
           // case BGrid.Gridstate.MOVE:
               // Debug.Log("de-pathing");
               // SceneData.sceneData.gridarray.RenderPathForGrid(GetComponent<BGrid>(), true);
               // break;
            default:

                break;
        }


    }

    void OnMouseDown()
    {
        //Debug.Log("de-pathing");
        GameObject director = GameObject.Find("BattleDirector");
        switch (GetComponent<BGrid>().gridstate)
        {
            case BGrid.Gridstate.PATH:
                //Debug.Log("accepted path");
                
                director.GetComponent<Bdirector>().MoveCurrentCharacter(this.GetComponent<BGrid>());
                //SceneData.sceneData.gridarray.RenderPathForGrid(GetComponent<BGrid>(), true);
                break;
            case BGrid.Gridstate.TARGET:
                SceneData.sceneData.gridarray.SetGridTrget(GetComponent<BGrid>(),true);
                director.GetComponent<Bdirector>().CastCurrentAction(this.GetComponent<BGrid>());
            // case BGrid.Gridstate.MOVE:
            // Debug.Log("de-pathing");
            // SceneData.sceneData.gridarray.RenderPathForGrid(GetComponent<BGrid>(), true);
            break;

        }


    }
}
