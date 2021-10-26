using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public string dialogueBoxName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            GameObject.Find(dialogueBoxName).GetComponent<DialogueManager>().StartDialogue(dialogue);
        }
    }
}
