using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlatformBehaviour))]
public class PlatformInspector : Editor {

    public override void OnInspectorGUI() {
        base.DrawDefaultInspector();
    }
}