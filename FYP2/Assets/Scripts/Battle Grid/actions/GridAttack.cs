using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class GridAttack : MonoBehaviour {

	// Use this for initialization
    public GameObject Animation;
    public int cast_range;
    public int potency;
    public bool ally_cast;
    public string actionname;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public virtual List<BGrid> GetTilesInRange(Gridarray board)
    {
        BGrid tile = GetComponent<Unit>().tile;
        List<BGrid> retValue = board.Search(tile, ExpandSearch);
        //skysk printrange(retValue);
        Filter(retValue);
        if (!ally_cast)
            retValue.Remove(GetComponent<Unit>().tile);

        return retValue;
    }
    public virtual List<BGrid> GetTilesInRange(Gridarray board,Unit unit)
    {
        BGrid tile = unit.tile;
        List<BGrid> retValue = board.Search(tile, ExpandSearch);
        //skysk printrange(retValue);
        Filter(retValue);

        //if (!ally_cast)
        //    retValue.Remove(GetComponent<Unit>().tile);

        return retValue;
    }
    protected virtual bool ExpandSearch(BGrid from, BGrid to)
    {
        return (from.distance + 1) <= cast_range;
    }
    protected virtual void Filter(List<BGrid> tiles)
    {
        //for (int i = tiles.Count - 1; i >= 0; --i)
        //    if (tiles[i].unit != null)
        //        tiles.RemoveAt(i);
    }

    public virtual void PlayAnimation(BGrid target)
    {
        Instantiate(Animation, target.transform.position, Quaternion.identity);
    }
    public virtual void DoAttack(BGrid target)
    {
        PlayAnimation(target);

        if(target.unit !=null)
        {
            //Debug.Log("found target");
            target.unit.takedamage(potency);
            DoEffect();
        }
        //target.unit.
    }
    public virtual void DoEffect()//put the action's effect here;
    {

    }
}
