using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    private Transform window = null;
    public Transform Window
    {
        get
        {
            return window;
        }
    }
    [SerializeField]
    private Transform background = null;
    public Transform Background
    {
        get
        {
            return background;
        }
    }

    [SerializeField]
    private Image portrait = null, advanceDialogueButton = null;
    public Image Portrait
    {
        get
        {
            return portrait;
        }
    }
    public Image AdvanceDialogueButton
    {
        get
        {
            return advanceDialogueButton;
        }
    }

    [SerializeField]
    private TextMeshProUGUI speakerName = null, dialogue = null;
    public TextMeshProUGUI SpeakerName
    {
        get
        {
            return speakerName;
        }
    }
    public TextMeshProUGUI Dialogue
    {
        get
        {
            return dialogue;
        }
    }
}
