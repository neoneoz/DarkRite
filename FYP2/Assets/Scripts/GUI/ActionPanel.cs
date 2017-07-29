using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ActionPanel : MonoBehaviour {

	// Use this for initialization
    public GameObject actionbutton,togglebutton,skipbutton;
    public UnitActions unit;
    bool hidden = false;
    Vector3 mainpos,btnpos;
	void Start () {
        TogglePanel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    


    public void PopulatePanel(UnitActions newunit)
    {
        ClearPanel();
        unit = newunit;

        List<GridAttack> actionlist = unit.ActionList;
        for (int i = 0; i < actionlist.Count; i++)
        {
            if (actionlist[i] == null)
                continue;
            GameObject button = Instantiate(actionbutton);
            //button.transform.SetParent(SceneData.sceneData.canvas.transform, false);
            button.transform.SetParent(transform,false);
            //myBB.Label = "Button " + i;
            //myBB.transform.positon = new Vector3(100, i * 80, 0);
            button.GetComponent<Text>().text = actionlist[i].name.ToUpper();
        }

    }

    public void ClearPanel()//this deletes all children
    {
        //Debug.Log("count: " + transform.childCount);
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            //Debug.Log("current" + i);
            GameObject button = transform.GetChild(i).gameObject;
            Destroy(button);
            //Destroy(transform.GetChild(i).GetComponent<GameObject>());

        }
    }

    public void SetCurrentAction(GameObject button)
    {    
        int action_index = button.transform.GetSiblingIndex();
        unit.currentaction = unit.ActionList[action_index];
        GameObject director = GameObject.Find("BattleDirector");
        director.GetComponent<Bdirector>().listening = false;
    }

    public void TogglePanel()
    {
        Debug.Log("toggle");
        //mainpos = this.transform.parent.transform.position;
        GameObject Interface =  this.transform.parent.gameObject;
       //CanvasRenderer[] children = Interface.GetComponentsInChildren<CanvasRenderer>();
       // for (int i = 0; i < children.Length; i++)
       // {
       //     Debug.Log(i);
       //     if (!hidden)//hide panel            {
       //         children[i].GetComponent<CanvasRenderer>()

       //     else
       //     children[i].GetComponent<CanvasRenderer>().enabled = true;
       // }

        if (!hidden)
        {
            mainpos = Interface.transform.position;
            Interface.transform.position += new  Vector3(-10000, 0, 0);
            btnpos = togglebutton.transform.position;
            togglebutton.transform.position= new Vector3(20, togglebutton.transform.position.y, 0);
            hidden = true;
        }
        else
        {
            togglebutton.transform.position = btnpos;
            Interface.transform.position = mainpos;
            hidden = false;
            
        }


    }
    public void TogglePanel(bool hide)
    {
        if (hide == hidden)
            return;
        else
            TogglePanel();
    
    }
    public void SkipPhase()
    {
        ClearPanel();
        GameObject director = GameObject.Find("BattleDirector");
        director.GetComponent<Bdirector>().Skip();
    }
}
