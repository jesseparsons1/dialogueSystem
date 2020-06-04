using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public KeyCode advanceDialogueKey;
    public Sprite advanceDialoguePromptSprite;
    public KeyCode fastFowardDialogueKey;
    public float textStartDelay;
    public float characterInterval;
    public float betweenSpeakerDelay;
    public bool previousSpeakerPortraitPersists;
    public AudioClip textScrollSound;
    public AudioClip textSkipSound;

    [System.Serializable]
    public enum BoxPositions
    {
        bottomLeft,
        bottomRight,
        topLeft,
        topRight
    }

    public List<Sprite> portraitSprites = new List<Sprite>();
    public List<bool> flipPortraits = new List<bool>();
    public List<string> speakerNames = new List<string>();
    public List<BoxPositions> boxPositions = new List<BoxPositions>();
    public List<string> dialogueTexts = new List<string>();
    public List<bool> shakeAtStarts = new List<bool>();
    public List<Vector2> shakeStrengths = new List<Vector2>();
    public List<float> initialDelays = new List<float>();

    public void InsertEmptyDialogueAt(int i)
    {
        portraitSprites.Insert(i, null);
        flipPortraits.Insert(i, false);
        speakerNames.Insert(i, "");
        boxPositions.Insert(i, BoxPositions.bottomLeft);
        dialogueTexts.Insert(i, "");
        shakeAtStarts.Insert(i, false);
        shakeStrengths.Insert(i, Vector2.zero);
        initialDelays.Insert(i, 0);
    }

    public void CopySpeaker(int copiedIndex, int to, bool copyDialogue = false)
    {
        portraitSprites.Insert(to, portraitSprites[copiedIndex]);
        flipPortraits.Insert(to, flipPortraits[copiedIndex]);
        speakerNames.Insert(to, speakerNames[copiedIndex]);
        boxPositions.Insert(to, boxPositions[copiedIndex]);
        dialogueTexts.Insert(to, copyDialogue ? dialogueTexts[copiedIndex] : "");
        shakeAtStarts.Insert(to, false);
        shakeStrengths.Insert(to, Vector2.zero);
        initialDelays.Insert(to, 0);
    }

    public void RemoveAt(int i)
    {
        portraitSprites.RemoveAt(i);
        flipPortraits.RemoveAt(i);
        speakerNames.RemoveAt(i);
        boxPositions.RemoveAt(i);
        dialogueTexts.RemoveAt(i);
        shakeAtStarts.RemoveAt(i);
        shakeStrengths.RemoveAt(i);
        initialDelays.RemoveAt(i);
    }
}
