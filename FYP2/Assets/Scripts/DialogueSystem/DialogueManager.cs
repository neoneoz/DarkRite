using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    Queue<string> dialogues;
    public GameObject dialogueBox;
    public DialogueReader reader;
    [SerializeField]
    public Dictionary<int, List<string>> dialoguesDictionary;
    private void Start()
    {

        dialogues = new Queue<string>();
        dialoguesDictionary = new Dictionary<int, List<string>>();
        reader.LoadDialogue(dialoguesDictionary);
    }

    public void AdvanceText()
    {
        if (dialogues.Count - 1 >= 0)
            dialogueBox.GetComponentInChildren<UnityEngine.UI.Text>().text = dialogues.Dequeue();
        else
        {
            dialogueBox.SetActive(false);
        }
    }

    public void SetText(int index)
    {
        dialogues.Clear();
        foreach (string dialogue in dialoguesDictionary[index])
        {
            Debug.Log(dialogue);
            dialogues.Enqueue(dialogue);
        }
    }

}
