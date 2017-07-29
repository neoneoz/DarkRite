using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class DialogueReader : MonoBehaviour {

    string path;
    string jsonString;
    private JsonData dialogueData;

	// Use this for initialization
	void Awake () {
        path = Application.streamingAssetsPath + "/Dialogues.json";
        jsonString = File.ReadAllText(path);
        dialogueData = JsonMapper.ToObject(jsonString);
    }
	
    public void LoadDialogue(Dictionary<int, List<string>> dictionary)
    {
       for(int iter = 0; iter < dialogueData.Count; iter++)
        {
            JsonData arrayOfDialogues = new JsonData();

            arrayOfDialogues = dialogueData[iter]["Dialogues"];

            List<string> tempStrList = new List<string>();

            for(int i = 0; i < arrayOfDialogues.Count; i++)
            {
                tempStrList.Add((string)arrayOfDialogues[i]);
            }

            dictionary.Add((int)dialogueData[iter]["ID"], tempStrList);
        }
    }

}
