using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour {

	// Use this for initialization
    //public bool in_range;
    protected BGrid targetunit;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected List<BGrid> GetMovementRange()
    {
        return GetComponent<GridMovement>().GetTilesInRange(SceneData.sceneData.gridarray);
        //return tiles;
    }

    public virtual BGrid GetMoveTargetTile()
    {
        return null;
    }

    protected BGrid GetAdjacentTile(BGrid target,List<BGrid> checklist)
    {
        
        Vector2 tempindex;
        Gridarray temp = SceneData.sceneData.gridarray;
        tempindex =target.index + new Vector2(0,1);//check above;
        if (CheckInList(checklist, tempindex))
            return temp.GetGridAt(tempindex).GetComponent<BGrid>();
        tempindex = target.index + new Vector2(0, -1);//check below;
        if (CheckInList(checklist, tempindex))
            return temp.GetGridAt(tempindex).GetComponent<BGrid>();
        tempindex = target.index + new Vector2(-1, 0);//check left;
        if (CheckInList(checklist, tempindex))
            return temp.GetGridAt(tempindex).GetComponent<BGrid>();
        tempindex = target.index + new Vector2(1, 0);//check right;
        if (CheckInList(checklist, tempindex))
            return temp.GetGridAt(tempindex).GetComponent<BGrid>();
        else
            return null;

    }
    private bool CheckInList(List<BGrid> checklist, Vector2 index)
    {
        Gridarray temp = SceneData.sceneData.gridarray;
        if (temp.GetGridAt(index) == null)//grid does not exist
            return false;

        return (checklist.Contains(temp.GetGridAt(index).GetComponent<BGrid>()));
    }

    public virtual void CastAction()
    {
        if(targetunit != null)
        {
            GetComponent<GridAttack>().DoAttack(targetunit);
        }
    }
}
