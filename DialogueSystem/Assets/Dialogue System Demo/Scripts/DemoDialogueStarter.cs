using UnityEngine;

public class DemoDialogueStarter : MonoBehaviour
{
    [SerializeField]
    private Dialogue demoDialogue = null;
    [SerializeField]
    private DialogueCanvas demoCanvas = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!DialogueManager.DialogueIsPlaying())
            {
                DialogueManager.PlayDialogue(demoDialogue, demoCanvas, this);
            }
        }
    }
}
