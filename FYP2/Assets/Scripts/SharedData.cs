using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Members of class can be accessed in Unity editor
[System.Serializable]

public class SharedData : MonoBehaviour {

    public static SharedData instance = null;
    public Player player;
    public GameObject game;
    public Canvas UICanvas;
    public List<Unit> player_party;
    public Encounter EncounterManager;
	// Use this for initialization
	void Start ()
    {
	    if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }else
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
    void Update()
    {
    }

}
