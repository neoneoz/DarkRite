using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkMovement : GridMovement
{

    protected override bool ExpandSearch(BGrid from, BGrid to)
    {
        // Skip if the distance in height between the two tiles is more than the unit can jump
        if ((Mathf.Abs(from.height - to.height) > jumpHeight))
            return false;

        // Skip if the tile is occupied by an enemy
        if (to.content != null)
            return false;

        return base.ExpandSearch(from, to);
    }

    public override IEnumerator Traverse(BGrid tile)
    {
        //Debug.Log("traversing");
        GetComponent<Unit>().Place(tile);
        //unit.Place(tile);
        
        // Build a list of way points from the unit's 
        // starting tile to the destination tile
        List<BGrid> targets = new List<BGrid>();
        while (tile != null)
        {
            targets.Insert(0, tile);
            tile = tile.prev;
            //Debug.Log(targets.Count);
        }
        StartCoroutine(MoveUnitCoroutine(targets));
        //GetComponent<Unit>().tile = tile;
        // Move to each way point in succession
        //for (int i = 1; i < targets.Count; ++i)
        //{
        //    BGrid from = targets[i - 1];
        //    BGrid to = targets[i];

        //   // Directions dir = from.GetDirection(to);
        //   // if (unit.dir != dir)//change sprite dir here
        //        //yield return StartCoroutine(Turn(dir));

        //    //if (from.height == to.height)
        //        yield return StartCoroutine(Walk(to));
        //    //else
        //    //    yield return StartCoroutine(Jump(to));
        //}
        // GameObject director = GameObject.Find("BattleDirector");
         yield return null;
         //director.GetComponent<Bdirector>().ResetMovePhase();
    }


    IEnumerator MoveUnitCoroutine(List<BGrid> waypoints)
    {
        for (int i = 0; i < waypoints.Count; i++)
        {

            Vector3 waypoint = waypoints[i].transform.position;
            while (Vector3.Distance(transform.position, waypoint) > .05f)
            {
                transform.position = Vector3.Lerp(transform.position, waypoint,10* Time.deltaTime);
                GetComponent<Unit>().healthbar.placehealthbar(transform.position);
                yield return null;
            }
        }
    }



    IEnumerator Walk(BGrid target)
    {
        transform.position = target.transform.position;
        return null;

        //Tweener tweener = transform.MoveTo(target.transform.position, 0.5f, EasingEquations.Linear);
        //while (tweener != null)
            //yield return null;
    }



    //IEnumerator Jump(Tile to)
    //{
    //    Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);

    //    Tweener t2 = jumper.MoveToLocal(new Vector3(0, Tile.stepHeight * 2f, 0), tweener.easingControl.duration / 2f, EasingEquations.EaseOutQuad);
    //    t2.easingControl.loopCount = 1;
    //    t2.easingControl.loopType = EasingControl.LoopType.PingPong;

    //    while (tweener != null)
    //        yield return null;
    //}
}