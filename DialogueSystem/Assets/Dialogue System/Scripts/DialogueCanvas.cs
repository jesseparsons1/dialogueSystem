using UnityEngine;

public class DialogueCanvas : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource = null;
    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }

    [SerializeField]
    private RectTransform dialogueBoxPosistion_BL = null, dialogueBoxPosition_BR = null, dialogueBoxPosition_TL = null, dialogueBoxPosition_TR = null;
    public RectTransform DialogueBoxPosistion_BL
    {
        get
        {
            return dialogueBoxPosistion_BL;
        }
    }
    public RectTransform DialogueBoxPosistion_BR
    {
        get
        {
            return dialogueBoxPosition_BR;
        }
    }
    public RectTransform DialogueBoxPosistion_TL
    {
        get
        {
            return dialogueBoxPosition_TL;
        }
    }
    public RectTransform DialogueBoxPosistion_TR
    {
        get
        {
            return dialogueBoxPosition_TR;
        }
    }

    [SerializeField]
    private DialogueBox dialogueBoxPrefab_BL = null, dialogueBoxPrefab_BR = null, dialogueBoxPrefab_TL = null, dialogueBoxPrefab_TR = null;
    public DialogueBox DialogueBoxPrefab_BL
    {
        get
        {
            return dialogueBoxPrefab_BL;
        }
    }
    public DialogueBox DialogueBoxPrefab_BR
    {
        get
        {
            return dialogueBoxPrefab_BR;
        }
    }
    public DialogueBox DialogueBoxPrefab_TL
    {
        get
        {
            return dialogueBoxPrefab_TL;
        }
    }
    public DialogueBox DialogueBoxPrefab_TR
    {
        get
        {
            return dialogueBoxPrefab_TR;
        }
    }

    [SerializeField]
    private Transform shakePivot = null;
    public Transform ShakePivot
    {
        get
        {
            return shakePivot;
        }
    }
}
