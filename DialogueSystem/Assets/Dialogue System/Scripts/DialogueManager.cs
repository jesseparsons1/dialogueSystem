using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static bool inDialog = false;
    private static MonoBehaviour currentlyRunningOnInstance;

    public static bool DialogueIsPlaying()
    {
        return inDialog;
    }

    public static void PlayDialogue(Dialogue dialogue, DialogueCanvas dialogueCanvas, MonoBehaviour runOnInstance)
    {
        if (!inDialog)
        {
            currentlyRunningOnInstance = runOnInstance;
            runOnInstance.StartCoroutine(PlayDialogueCor(dialogue, dialogueCanvas, runOnInstance));
        }
        else
        {
            Debug.LogError("PlayConversation() called by " + runOnInstance + " but is already running dialogue on " + currentlyRunningOnInstance + ".");
        }
    }
    
    private static IEnumerator PlayDialogueCor(Dialogue dialogue, DialogueCanvas dialogueCanvasPrefab, MonoBehaviour runOnInstance)
    {
        inDialog = true;

        DialogueCanvas dialogueCanvas = Instantiate(dialogueCanvasPrefab);

        //Object pool
        DialogueBox pooledBox_BL = null, pooledBox_BR = null, pooledBox_TL = null, pooledBox_TR = null;

        //If speakers persist, keep track of where the last speaker was and their portrait
        Image previousSpeakerPortrait_BL = null, previousSpeakerPortrait_BR = null, previousSpeakerPortrait_TL = null, previousSpeakerPortrait_TR = null;

        //Iterate over number of dialogue boxes needed
        int i = 0, iMax = dialogue.dialogueTexts.Count;

        //Scroll text for i'th dialogue box
        while (i < iMax)
        {
            DialogueBox curDialogueBox = null;

            switch (dialogue.boxPositions[i])
            {
                //Create new box or use pooled one
                case Dialogue.BoxPositions.bottomLeft:
                    if (pooledBox_BL == null)
                    {
                        pooledBox_BL = Instantiate(dialogueCanvas.DialogueBoxPrefab_BL, dialogueCanvas.ShakePivot);
                        pooledBox_BL.GetComponent<RectTransform>().position = dialogueCanvas.DialogueBoxPosistion_BL.position;
                    }
                    pooledBox_BL.gameObject.SetActive(true);
                    curDialogueBox = pooledBox_BL;
                    if (dialogue.previousSpeakerPortraitPersists) {
                        curDialogueBox.Portrait.transform.SetParent(dialogueCanvas.ShakePivot);
                        curDialogueBox.Portrait.transform.SetAsFirstSibling();
                    }
                    break;
                case Dialogue.BoxPositions.bottomRight:
                    if (pooledBox_BR == null)
                    {
                        pooledBox_BR = Instantiate(dialogueCanvas.DialogueBoxPrefab_BR, dialogueCanvas.ShakePivot);
                        pooledBox_BR.GetComponent<RectTransform>().position = dialogueCanvas.DialogueBoxPosistion_BR.position;
                    }
                    pooledBox_BR.gameObject.SetActive(true);
                    curDialogueBox = pooledBox_BR;
                    if (dialogue.previousSpeakerPortraitPersists)
                    {
                        curDialogueBox.Portrait.transform.SetParent(dialogueCanvas.ShakePivot);
                        curDialogueBox.Portrait.transform.SetAsFirstSibling();
                    }
                    break;
                case Dialogue.BoxPositions.topLeft:
                    if (pooledBox_TL == null)
                    {
                        pooledBox_TL = Instantiate(dialogueCanvas.DialogueBoxPrefab_TL, dialogueCanvas.ShakePivot);
                        pooledBox_TL.GetComponent<RectTransform>().position = dialogueCanvas.DialogueBoxPosistion_TL.position;
                    }
                    pooledBox_TL.gameObject.SetActive(true);
                    curDialogueBox = pooledBox_TL;
                    if (dialogue.previousSpeakerPortraitPersists)
                    {
                        curDialogueBox.Portrait.transform.SetParent(dialogueCanvas.ShakePivot);
                        curDialogueBox.Portrait.transform.SetAsFirstSibling();
                    }
                    break;
                case Dialogue.BoxPositions.topRight:
                    if (pooledBox_TR == null)
                    {
                        pooledBox_TR = Instantiate(dialogueCanvas.DialogueBoxPrefab_TR, dialogueCanvas.ShakePivot);
                        pooledBox_TR.GetComponent<RectTransform>().position = dialogueCanvas.DialogueBoxPosistion_TR.position;
                    }
                    pooledBox_TR.gameObject.SetActive(true);
                    curDialogueBox = pooledBox_TR;
                    if (dialogue.previousSpeakerPortraitPersists)
                    {
                        curDialogueBox.Portrait.transform.SetParent(dialogueCanvas.ShakePivot);
                        curDialogueBox.Portrait.transform.SetAsFirstSibling();
                    }
                    break;
            }

            //Update current box's information
            curDialogueBox.Portrait.sprite = (dialogue.portraitSprites[i] != null) ? dialogue.portraitSprites[i] : null;
            curDialogueBox.Portrait.color = new Color(255f, 255f, 255f, (dialogue.portraitSprites[i] != null) ? 255f : 0f);
            float xScaleAbs = Mathf.Abs(curDialogueBox.Portrait.rectTransform.localScale.x);
            curDialogueBox.Portrait.rectTransform.localScale = new Vector3(dialogue.flipPortraits[i] ? -xScaleAbs : xScaleAbs, curDialogueBox.Portrait.rectTransform.localScale.y, curDialogueBox.Portrait.rectTransform.localScale.z);
            curDialogueBox.SpeakerName.text = dialogue.speakerNames[i];
            curDialogueBox.Dialogue.text = dialogue.dialogueTexts[i];
            curDialogueBox.Dialogue.maxVisibleCharacters = 0;
            curDialogueBox.AdvanceDialogueButton.color = new Color(255f, 255f, 255f, 0f);

            yield return new WaitForSeconds(dialogue.textStartDelay);

            //Shake camera
            if (dialogue.shakeAtStarts[i])
            {
                Shake(dialogueCanvas.ShakePivot ,dialogue.shakeStrengths[i], runOnInstance);
            }

            //Shake camera
            if (dialogue.initialDelays[i] > 0)
            {
                yield return new WaitForSeconds(dialogue.initialDelays[i]);
            }

            //Iterate over number of characters
            int totalCharacters = dialogue.dialogueTexts[i].Length;
            float elapsedTime = 0f;
            while (!ListenForKeyDown(dialogue.advanceDialogueKey) && curDialogueBox.Dialogue.maxVisibleCharacters != totalCharacters)
            {
                //If character interval has elapsed
                if (elapsedTime > dialogue.characterInterval)
                {
                    elapsedTime = 0f;

                    //Add character
                    curDialogueBox.Dialogue.maxVisibleCharacters += 1;
                    if (dialogue.textScrollSound != null && dialogueCanvas.AudioSource != null)
                    {
                        if (!(curDialogueBox.Dialogue.text[curDialogueBox.Dialogue.maxVisibleCharacters - 1].ToString() == " "))
                        {
                            dialogueCanvas.AudioSource.PlayOneShot(dialogue.textScrollSound);
                        }
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            //If skipped dialogue, play sound
            if (ListenForKeyDown(dialogue.advanceDialogueKey) && dialogue.textSkipSound != null && dialogueCanvas.AudioSource != null)
            {
                dialogueCanvas.AudioSource.PlayOneShot(dialogue.textSkipSound);
            }

            //Scroll ended, set all character visisble  
            curDialogueBox.Dialogue.maxVisibleCharacters = totalCharacters;

            //Wait to prevent immediately advancing to next box
            yield return new WaitForEndOfFrame();

            //Show advance icon
            if (dialogue.advanceDialoguePromptSprite != null)
            {
                curDialogueBox.AdvanceDialogueButton.color = new Color(255f, 255f, 255f, 255f);
                curDialogueBox.AdvanceDialogueButton.sprite = dialogue.advanceDialoguePromptSprite;
            }

            //Wait for advance press
            while (!ListenForKeyDown(dialogue.advanceDialogueKey))
            {
                yield return null;
            }

            //Play sound once pressed
            if (dialogue.textSkipSound != null)
            {
                dialogueCanvas.AudioSource.PlayOneShot(dialogue.textSkipSound);
            }

            //Hide window for between speaker delay
            curDialogueBox.Window.gameObject.SetActive(false);
            if (i != iMax - 1)
            {
                if (dialogue.speakerNames[i] != dialogue.speakerNames[i + 1] && dialogue.betweenSpeakerDelay > 0)
                {
                    yield return new WaitForSeconds(dialogue.betweenSpeakerDelay);
                }
            }
            curDialogueBox.Window.gameObject.SetActive(true);

            //Pool box
            curDialogueBox.gameObject.SetActive(false);

            //Step forward to next box
            i++;
        }

        //Dialogue has ended

        //Hide leftover portraits
        if (previousSpeakerPortrait_BL != null)
        {
            previousSpeakerPortrait_BL.gameObject.SetActive(false);
            previousSpeakerPortrait_BR.gameObject.SetActive(false);
            previousSpeakerPortrait_TL.gameObject.SetActive(false);
            previousSpeakerPortrait_TR.gameObject.SetActive(false);
        }

        Destroy(dialogueCanvas.gameObject, .4f);

        currentlyRunningOnInstance = null;
        inDialog = false;
    }

    public static bool ListenForKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    private static Coroutine shakeCor;
    private static MonoBehaviour shakeRunningOnInstance;

    private static void Shake(Transform shakePivot, Vector2 shakeParameters, MonoBehaviour runOnInstance)
    {
        StopShake();
        shakeRunningOnInstance = runOnInstance;
        shakeCor = runOnInstance.StartCoroutine(ShakeCor(shakePivot, shakeParameters.x, shakeParameters.y));
    }

    private static void StopShake()
    {
        if (shakeCor != null)
        {
            shakeRunningOnInstance.StopCoroutine(shakeCor);
        }
    }

    private static IEnumerator ShakeCor(Transform shakePivot, float duration, float magnitude)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            //Get randomOffset
            Vector2 randomOffset = Random.insideUnitCircle * magnitude;

            //Offset transform by random offset
            shakePivot.localPosition = randomOffset;

            //Increment time
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        shakePivot.localPosition = Vector2.zero;
    }
}
