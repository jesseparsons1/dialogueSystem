using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    int buttonWidth = 200;

    SerializedProperty advanceDialogueKeyProp;
    SerializedProperty advanceDialoguePromptSpriteProp;
    SerializedProperty advanceDialogueSoundProp;
    SerializedProperty textScrollSoundProp;
    SerializedProperty textStartDelayProp;
    SerializedProperty textCharacterIntervalProp;
    SerializedProperty betweenSpeakerDelayProp;
    SerializedProperty previousSpeakerPortraitPersistsProp;

    SerializedProperty portraitSpritesProp;
    SerializedProperty flipPortraitsProp;
    SerializedProperty speakerNamesProp;
    SerializedProperty boxPositionsProp;
    SerializedProperty dialogueTextsProp;
    SerializedProperty shakeAtStartsProp;
    SerializedProperty shakeStrengthsProp;
    SerializedProperty initialDelaysProp;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        advanceDialogueKeyProp = serializedObject.FindProperty("advanceDialogueKey");
        advanceDialoguePromptSpriteProp = serializedObject.FindProperty("advanceDialoguePromptSprite");
        advanceDialogueSoundProp = serializedObject.FindProperty("textSkipSound");
        textScrollSoundProp = serializedObject.FindProperty("textScrollSound");
        textStartDelayProp = serializedObject.FindProperty("textStartDelay");
        textCharacterIntervalProp = serializedObject.FindProperty("characterInterval");
        betweenSpeakerDelayProp = serializedObject.FindProperty("betweenSpeakerDelay");

        previousSpeakerPortraitPersistsProp = serializedObject.FindProperty("previousSpeakerPortraitPersists");
        portraitSpritesProp = serializedObject.FindProperty("portraitSprites");
        flipPortraitsProp = serializedObject.FindProperty("flipPortraits");
        speakerNamesProp = serializedObject.FindProperty("speakerNames");
        boxPositionsProp = serializedObject.FindProperty("boxPositions");
        dialogueTextsProp = serializedObject.FindProperty("dialogueTexts");
        shakeAtStartsProp = serializedObject.FindProperty("shakeAtStarts");
        shakeStrengthsProp = serializedObject.FindProperty("shakeStrengths");
        initialDelaysProp = serializedObject.FindProperty("initialDelays");
    }

    public override void OnInspectorGUI()
    {
        Dialogue dialogue = (Dialogue)target;

        //if (GUILayout.Button("Add Dialogue"))
        //{
        //    dialogue.InsertEmptyDialogue(dialogue.portraitTextures.Count);
        //}

        DrawDefaultInspector();
        
        EditorGUILayout.PropertyField(advanceDialogueKeyProp);
        EditorGUILayout.PropertyField(advanceDialoguePromptSpriteProp);
        EditorGUILayout.PropertyField(advanceDialogueSoundProp);
        EditorGUILayout.PropertyField(textScrollSoundProp);
        EditorGUILayout.PropertyField(textStartDelayProp);
        EditorGUILayout.PropertyField(textCharacterIntervalProp);
        EditorGUILayout.PropertyField(betweenSpeakerDelayProp);
        EditorGUILayout.PropertyField(previousSpeakerPortraitPersistsProp);
        
        //Iterate over each box
        for (int i = 0; i < dialogue.portraitSprites.Count; i++)
        {
            GUILayout.Space(10);

            //Draw box
            GUILayout.BeginVertical(EditorStyles.helpBox);
            
            if (portraitSpritesProp.isArray)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Portrait:");
                SerializedProperty portraitTextureProp = portraitSpritesProp.GetArrayElementAtIndex(i);
                portraitTextureProp.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(portraitTextureProp.objectReferenceValue, typeof(Sprite), false, GUILayout.Width(100), GUILayout.Height(100));
                GUILayout.EndHorizontal();
            }
            if (flipPortraitsProp.isArray)
            {
                SerializedProperty flipPortraitProp = flipPortraitsProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(flipPortraitProp, new GUIContent("Flip portrait?"));
            }
            if (speakerNamesProp.isArray)
            {
                SerializedProperty speakerNameProp = speakerNamesProp.GetArrayElementAtIndex(i);
                speakerNameProp.stringValue = EditorGUILayout.TextField("Speaker:", speakerNameProp.stringValue);
            }
            if (boxPositionsProp.isArray)
            {
                SerializedProperty dialogueBoxPositionProp = boxPositionsProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(dialogueBoxPositionProp, new GUIContent("Position:"));
            }
            if (dialogueTextsProp.isArray)
            {
                EditorGUILayout.LabelField("Dialogue:");
                Vector2 scrollPosition = Vector2.zero;
                GUILayout.BeginScrollView(scrollPosition);
                EditorStyles.textField.wordWrap = true;
                SerializedProperty dialogueTextProp = dialogueTextsProp.GetArrayElementAtIndex(i);
                dialogueTextProp.stringValue = EditorGUILayout.TextArea(dialogueTextProp.stringValue, GUILayout.ExpandHeight(true));
                GUILayout.EndScrollView();
            }
            if (shakeAtStartsProp.isArray)
            {
                SerializedProperty shakeAtStartProp = shakeAtStartsProp .GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(shakeAtStartProp, new GUIContent("Screen shake at start?"));
                
                if (dialogue.shakeAtStarts[i]) {
                    if (shakeStrengthsProp.isArray)
                    {
                        SerializedProperty shakeStrengthProp = shakeStrengthsProp.GetArrayElementAtIndex(i);

                        EditorGUILayout.PropertyField(shakeStrengthProp, new GUIContent("Shake Strength"));
                    }
                }
                
            }
            if (initialDelaysProp.isArray)
            {
                SerializedProperty initialDelayProp = initialDelaysProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(initialDelayProp, new GUIContent("Delay:"));
            }


            GUILayout.Space(10);

            BeginPaddedHorizontal();
            if (GUILayout.Button("Remove", GUILayout.Width(buttonWidth)))
            {
                dialogue.RemoveAt(i);
                break;
            }
            if (GUILayout.Button("Insert Speaker At End", GUILayout.Width(buttonWidth)))
            {
                dialogue.CopySpeaker(i, dialogue.portraitSprites.Count);
                break;
            }
            EndPaddedHorizontal();

            BeginPaddedHorizontal();
            if (GUILayout.Button("Insert Empty Dialogue Below", GUILayout.Width(buttonWidth)))
            {
                dialogue.InsertEmptyDialogueAt(i + 1);
                break;
            }
            if (GUILayout.Button("Insert Speaker Below", GUILayout.Width(buttonWidth)))
            {
                dialogue.CopySpeaker(i, i + 1);
                break;
            }
            EndPaddedHorizontal();

            BeginPaddedHorizontal();
            if (GUILayout.Button("Move Up", GUILayout.Width(buttonWidth)))
            {
                MoveUp(dialogue, i);
                break;
            }
            if (GUILayout.Button("Move Down", GUILayout.Width(buttonWidth)))
            {
                MoveUp(dialogue, i + 1);
                break;
            }
            EndPaddedHorizontal();

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
        
    }

    public void MoveUp(Dialogue dialogue, int i)
    {
        if (i < dialogue.dialogueTexts.Count)
        {
            dialogue.CopySpeaker(i, i - 1, true);
            dialogue.RemoveAt(i + 1);
        }
    }

    public void BeginPaddedHorizontal()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
    }

    public void EndPaddedHorizontal()
    {
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
