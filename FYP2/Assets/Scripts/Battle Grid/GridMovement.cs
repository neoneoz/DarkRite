using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GridMovement : MonoBehaviour {

	// Use this for initialization
    //public int m_gridX, m_gridY;//starting grid
    //public Gridarray array;
    void Start()
    {
        //transform.position = SceneData.sceneData.gridarray.GetGridPosAt(m_gridX, m_gridY);
       // GameObject ass = array.GetGridAt(m_gridX, m_gridY);
	    //starting grid here(snaap to starting grid at spaawn)
	}
    public int range;
    public int jumpHeight;
    protected Unit unit;
    protected Transform jumper;
    public abstract IEnumerator Traverse(BGrid tile);
    public virtual List<BGrid> GetTilesInRange(Gridarray board)
    {
        BGrid tile = GetComponent<Unit>().tile;
        List<BGrid> retValue = board.Search(tile, ExpandSearch);
       //skysk printrange(retValue);
        Filter(retValue);
        
        return retValue;
    }
    protected virtual bool ExpandSearch(BGrid from, BGrid to)
    {
        return (from.distance + 1) <= range;
    }
    protected virtual void Filter(List<BGrid> tiles)
    {
        //for (int i = tiles.Count - 1; i >= 0; --i)
        //    if (tiles[i].unit != null)
        //        tiles.RemoveAt(i);
    }
	// Update is called once per frame
	void Update () {
	
	}
    void printrange(List<BGrid> range)
    {
        for (int i = range.Count - 1; i >= 0; --i)
            Debug.Log(range[i].index);
    }


}
