using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MenuItems : MonoBehaviour {

    [MenuItem("OpenScene/Sample Scene")] // %1 for a scortcut strg+1
    static void LoadSampleScene() {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
    }

    [MenuItem("OpenScene/Another Scene %")]
    static void LoadAnotherScene() {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/AnotherScene.unity");
    }


}