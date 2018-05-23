#if UNITY_5_3_OR_NEWER || UNITY_5

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PoolDebugger))]
public class PoolDebuggerCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PoolDebugger myScript = (PoolDebugger)target;
        if(GUILayout.Button("Fetch Object Created"))
        {
            myScript.FetchObjectCreated();
        }

        if(GUILayout.Button("Fetch Object Reused"))
        {
            myScript.FetchObjectReused();
        }

        if(GUILayout.Button("Fetch Object Recycled"))
        {
            myScript.FetchObjectRecycled();
        }
    }
}
#endif