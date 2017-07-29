using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Bdirector : MonoBehaviour {

	// Use this for initialization
    public enum Bstate//battle state cycles in downaward order
    {
        P_SET_MOVE,//move phase starts here, wait for player to move all allied char
        E_SET_MOVE,//AI here

        P_SET_ACTION,//action phase starts here, wait for player to assign action to all allied char
        E_SET_ACTION,//AI Action based on position/own health/wtv
        //check for victory/defeat at the end of ^this state

        END_DEFEAT,
        END_VICTORY,
    }
    public Bstate currentphase;
    public Unit currentunit,testenemy,player;
    //public Unit testenemy;
    public int phase_count,enemy_count;

    public List<Unit> player_party;
    public Queue<Unit> player_units;
    bool isqueue=false,init=false;
    public List<Unit> enemy_party;


    public Gridarray board;//thsi hoolds the level grid
    public HPbarHandler barhandler;
    List<BGrid> tiles;//this holds appopratie tile information for the current state 
    public List<Vector2> spawn_points;// this are the spawn indexes for friendly units

    public bool listening = false,testing;

	void Start () {

       
        if(testing)
        {
            SetTestStart();
            return;
        }
        Scenemanager.instance.SetBattleActive();
        Invoke("SetBattleStart", 0.001f);
        //SetBattleStart();
	}
	
	// Update is called once per frame
	void Update () {

        if (!init)
            return;

        //just check if there aare no enemies
        if (enemy_count == 0)
            currentphase = Bstate.END_VICTORY;

	    switch(currentphase)
        {
            case Bstate.P_SET_MOVE:
                if(!isqueue)
                {
                    for (int i = 0; i < player_party.Count; i++)
                    {
                        player_units.Enqueue(player_party[i]);
                    }
                    isqueue = true;
                    currentunit = player_units.Dequeue();
                }

                if(!listening)
                {
                    setCurrentMoveGrids();
                    listening = true;
                }
                //display mouse over paths

                //waiting for input

                break;
            case Bstate.E_SET_MOVE:
                for (int i = 0; i < enemy_party.Count; i++)
                {
                    //player_units.Enqueue(player_party[i]);
                    if (enemy_party[i].GetComponent<Unit>().isdead)
                        continue;

                    currentunit = enemy_party[i];
                    MoveCurrentCharacter(currentunit.GetComponent<Enemy>().GetMoveTargetTile());
                    //currentphase = Bstate.P_SET_ACTION;
                }
                currentphase = Bstate.P_SET_ACTION;
               break;
            case Bstate.P_SET_ACTION:
              
               if(!isqueue)
                {
                    for (int i = 0; i < player_party.Count; i++)
                    {
                        player_units.Enqueue(player_party[i]);

                    }
                    isqueue = true;
                    currentunit = player_units.Dequeue();
                    SetCurrentActionPanel();
                }
                //let the player select the action first
               if (!listening)
               {
                   SceneData.sceneData.panel.transform.GetChild(1).GetComponent<ActionPanel>().TogglePanel(false);
                   SetCurrentActionGrids();
                   listening = true;
               }
               break;
            case Bstate.E_SET_ACTION:
               for (int i = 0; i < enemy_party.Count; i++)
               {
                   //player_units.Enqueue(player_party[i]);
                   if (enemy_party[i].GetComponent<Unit>().isdead)
                       continue;

                   currentunit = enemy_party[i];
                   currentunit.GetComponent<Enemy>().CastAction();
               }
               currentphase = Bstate.P_SET_MOVE;
               break;
            case Bstate.END_VICTORY:
               board.DerenderGrids();
               ExitBattleScene();
                //send the player back to the screen
               break;
            case Bstate.END_DEFEAT:
               board.DerenderGrids();
               //send the player back to the main menu
               break;
        }


	}

    bool SetBattleStart()
    {
        //spawn all players and monsters at the correct location
        //WaitForSeconds()
       
        player_units = new Queue<Unit>();//init player controlled units queue
        //player_party = SharedData.instance.player_party;
        GenerateUnits();
        for (int i = 0; i < player_party.Count; i++)
        {
            //player_party[i] = Instantiate(player_party[i]);
            currentunit = player_party[i];
            //int point = Random.Range(0, spawn_points.Count);
            currentunit.tile = board.GetGridAt(spawn_points[i]).GetComponent<BGrid>();
            currentunit.transform.position = currentunit.tile.transform.position;
            currentunit.Place(currentunit.tile);
            barhandler.GenerateHPbar(currentunit);
        }
        for (int j = 0; j < enemy_party.Count; j++)
        {
            currentunit = enemy_party[j];
            currentunit.tile = board.GetRandomEmptyGrid();
            currentunit.transform.position = currentunit.tile.transform.position;
            currentunit.Place(currentunit.tile);
            barhandler.GenerateHPbar(currentunit);
        }
        enemy_count = enemy_party.Count;
        //barhandler.placeHPbars();
        //currentunit = testenemy;
        //currentunit.tile = board.GetGridAt(6,8).GetComponent<BGrid>();
        //currentunit.transform.position = currentunit.tile.transform.position;
        //currentunit.Place(board.GetGridAt(6, 8).GetComponent<BGrid>());



        currentphase = Bstate.P_SET_MOVE;//start every encounter with player move phase
        phase_count = 0;
        init = true;

        return true;
    }
    void GenerateUnits()
    {
        for (int i = 0; i < SharedData.instance.EncounterManager.encounterlist.Count; i++)
        {
            enemy_party.Add(Instantiate(SharedData.instance.EncounterManager.encounterlist[i]));
        }

        for (int i = 0; i < SharedData.instance.player_party.Count; i++)
        {
            player_party.Add(Instantiate(SharedData.instance.player_party[i]));
        }
    }

    void setCurrentMoveGrids()//sets and renders the movable grid for currentunit
    {
        //board.DerenderGrids();
        GridMovement mover = currentunit.GetComponent<GridMovement>();
        tiles = mover.GetTilesInRange(board);
        board.SetGridState(tiles,BGrid.Gridstate.MOVE);
    }
    void SetCurrentActionGrids()//sets and renders the availble grids for currentunit's current action
    {
        //GridAttack attack = currentunit.GetComponent<GridAttack>();
        board.DerenderGrids();
        GridAttack action = currentunit.GetComponent<UnitActions>().currentaction;
        if (action != null)
        {
            tiles = action.GetTilesInRange(board,currentunit);
            board.SetGridState(tiles, BGrid.Gridstate.ATTACK);
        }
    }
    void SetCurrentActionPanel()//sets the panel to the currentunit's availble actions
    {
        SceneData.sceneData.panel.transform.GetChild(1).GetComponent<ActionPanel>().PopulatePanel(currentunit.GetComponent<UnitActions>());
    }


    public void MoveCurrentCharacter(BGrid destination)
    {
        //Debug.Log("calling move funcc");
       // SceneData.sceneData.mouseinput = false;
        board.DerenderGrids();
        GridMovement m = currentunit.GetComponent<GridMovement>();
        StartCoroutine(m.Traverse(destination));
        //check if anymore friendly chaar
        if (player_units.Count != 0)
        {
            //Debug.Log("next allied unit");

            currentunit = player_units.Dequeue();
        }
        else
        {
            currentphase = Bstate.E_SET_MOVE;
            isqueue = false;
        }

        listening = false;

       // SceneData.sceneData.mouseinput = false;

       //move to next char

       //enable controls


    
        
        //currentunit.GetComponent<GridMovement>().Traverse(destination);
        //board.DerenderGrids();
        //currentunit
    }//move any current character
    public void CastCurrentAction(BGrid destination)
    {
        //need to store what action is being casted
        board.DerenderGrids();
        SceneData.sceneData.panel.transform.GetChild(1).GetComponent<ActionPanel>().ClearPanel();
        currentunit.GetComponent<UnitActions>().currentaction.DoAttack(destination);
        currentunit.GetComponent<UnitActions>().currentaction = null;
        if (player_units.Count != 0)
        {
            Debug.Log("next allied unit");
            currentunit = player_units.Dequeue();
            SetCurrentActionPanel();
        }
        else
        {
            currentphase = Bstate.E_SET_ACTION;
            isqueue = false;
        }
        listening = false;


    }//cast action(player units  only)
    public void Skip()
    {
        
        switch(currentphase)
        { 
            case (Bstate.P_SET_ACTION):
            board.DerenderGrids();
            currentunit.GetComponent<UnitActions>().currentaction = null;
            if (player_units.Count != 0)
            {
                Debug.Log("next allied unit");
                currentunit = player_units.Dequeue();
                SetCurrentActionPanel();
            }
            else
            {
                currentphase = Bstate.E_SET_ACTION;
                isqueue = false;
            }
            listening = false;
            break;
            case (Bstate.P_SET_MOVE):
            board.DerenderGrids();
            if (player_units.Count != 0)
            {
                //Debug.Log("next allied unit");

                currentunit = player_units.Dequeue();
            }
            else
            {
                currentphase = Bstate.E_SET_MOVE;
                isqueue = false;
            }

            listening = false;
            break;

        };
    }

    public void ResetMovePhase()
    {
        Debug.Log("reseting");
        SceneData.sceneData.mouseinput = true;
        setCurrentMoveGrids();
        
    }
    public void ExitBattleScene()
    {
        SharedData.instance.EncounterManager.encounterlist.Clear();
        Scenemanager.instance.UnloadBattleScene();
    }

    void SetTestStart()
    {
        player_units = new Queue<Unit>();//init player controlled units queue
        for (int i = 0; i < player_party.Count; i++)
        {
            //player_party[i] = Instantiate(player_party[i]);
            currentunit = player_party[i];
            //int point = Random.Range(0, spawn_points.Count);
            currentunit.tile = board.GetGridAt(spawn_points[i]).GetComponent<BGrid>();
            currentunit.transform.position = currentunit.tile.transform.position;
            currentunit.Place(currentunit.tile);
            barhandler.GenerateHPbar(currentunit);
        }
        for (int j = 0; j < enemy_party.Count; j++)
        {
            currentunit = enemy_party[j];
            currentunit.tile = board.GetRandomEmptyGrid();
            currentunit.transform.position = currentunit.tile.transform.position;
            currentunit.Place(currentunit.tile);
            barhandler.GenerateHPbar(currentunit);
        }
        enemy_count = enemy_party.Count;
        currentphase = Bstate.P_SET_MOVE;//start every encounter with player move phase
        phase_count = 0;
        init = true;
    }
}
