using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Mirror_Knight : Enemy {

    public override BGrid GetMoveTargetTile()
    {
        GetComponent<GridMovement>().range = GetComponent<GridMovement>().range+1;//temp increase range for checkking
        List<BGrid> walkrange = GetMovementRange();

        for (int i = 0; i < walkrange.Count; i++ )//if a player unit is within raange
        {
            if (!walkrange[i].unit)
                continue;

            if (!walkrange[i].unit.player)
                continue;

            BGrid target;
            targetunit = walkrange[i];
            target = GetAdjacentTile(targetunit, walkrange);
            if(target != null)
            {
                
                GetComponent<GridMovement>().range = GetComponent<GridMovement>().range - 1;
                return target;
            }
            //if(myList.Contains(x))
            //if(wa)
        }
        targetunit = null;
        GetComponent<GridMovement>().range = GetComponent<GridMovement>().range - 1;
        int randomtile = Random.Range(0, walkrange.Count);
        return walkrange[randomtile];//return a random tile from movble tiles
        //return SceneData.sceneData.gridarray.GetGridAt(GetComponent<Unit>().tile.index +new Vector2(0,-1)).GetComponent<BGrid>();
       



            //if ((SceneData.sceneData.player.tile.index - GetComponent<Unit>().tile.index).sqrMagnitude < GetComponent<GridMovement>().range * GetComponent<GridMovement>().range)
            //{
            //can move into attackk range now




            //}

    }
}
