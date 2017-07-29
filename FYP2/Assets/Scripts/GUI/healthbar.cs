using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthbar : MonoBehaviour {

	// Use this for initialization
    //public Unit unit;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void placehealthbar(Unit unit)
    {
        //unit.healthbar.transform.position = WorldToCanvasPosition(SceneData.sceneData.canvas.GetComponent<RectTransform>(), SceneData.sceneData.camera, unit.transform.position + SceneData.sceneData.barhandler.offset);
        unit.healthbar.transform.position = SceneData.sceneData.camera.WorldToScreenPoint(unit.transform.position + SceneData.sceneData.barhandler.offset);
    }
    public void placehealthbar(Vector3 pos)
    {
        //transform.position = WorldToCanvasPosition(SceneData.sceneData.canvas.GetComponent<RectTransform>(), SceneData.sceneData.camera, pos + SceneData.sceneData.barhandler.offset);
        transform.position = SceneData.sceneData.camera.WorldToScreenPoint(pos + SceneData.sceneData.barhandler.offset);
    }
    public void setvalue(int health,int maxhealth)
    {
        transform.GetChild(1).GetComponent<Text>().text = health.ToString();//set text health value
        transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = ((float)health / (float)maxhealth);//set the hp in the bar according to hp%
        //Debug.Log("amount = " + (float)(health / maxhealth));
    }
    private Vector3 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
    {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 temp = camera.WorldToViewportPoint(position);

        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        //The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //actually its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
        //returned value will still be correct.
        //temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        //temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return new Vector3(temp.x, temp.y, 0);
    }
}
