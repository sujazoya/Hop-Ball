using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DoOnEnable))]
public class DoOnEnableEditor : Editor
{
    public enum Do
    {
        None, Disable, Destroy, PlaySound
    }   
     public override void OnInspectorGUI()
    {
        DoOnEnable script = (DoOnEnable)target;

        script.@do = (DoOnEnable.Do)EditorGUILayout.EnumPopup("Do", script.@do);

        if (script.@do == DoOnEnable.Do.Disable)
        {
            script.after = EditorGUILayout.FloatField("After", script.after);          
        }
        else
        if (script.@do == DoOnEnable.Do.Destroy)
        {
            script.after = EditorGUILayout.FloatField("After:", script.after);          
        }
        else
        if (script.@do == DoOnEnable.Do.PlaySoundFromMusicManager)
        {
            script.after = EditorGUILayout.FloatField("After", script.after);
            script.soundName = EditorGUILayout.TextField("Sound Name", script.soundName);
            //script.testString = EditorGUILayout.TextField("Test string", script.testString);
            //script.testInt = EditorGUILayout.IntField("Test int", script.testInt);
        }
        else
        if (script.@do == DoOnEnable.Do.playSelfSound)
        {
            script.after = EditorGUILayout.FloatField("Increase scale by:", script.after);
            script.selfSound = (AudioClip)EditorGUILayout.ObjectField("Self Sound", script.selfSound, typeof(AudioClip), false);
            //script.testString = EditorGUILayout.TextField("Test string", script.testString);
            //script.testInt = EditorGUILayout.IntField("Test int", script.testInt);
        }

    }
}

