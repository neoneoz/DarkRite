using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SceneData : MonoBehaviour
{
    public Unit player;
    public static SceneData sceneData;
    public Gridarray gridarray;
    public bool mouseinput = true;
    public GameObject panel;
    public Camera camera;
    public HPbarHandler barhandler;
    public Canvas canvas;
	// Use this for initialization
    void Awake()
    {
        sceneData = this;
    }
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
