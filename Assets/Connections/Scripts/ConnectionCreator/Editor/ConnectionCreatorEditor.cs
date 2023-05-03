using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (ConnectionCreator), true)]
public class ConnectionCreatorEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector ();

        ConnectionCreator connectionCreator = target as ConnectionCreator;

        GUILayout.Space (20);
        if (GUILayout.Button ("Reconnect"))
        {
            if (connectionCreator)
                connectionCreator.Reconnect ();
        }

        if (connectionCreator.Other)
        {
            GUILayout.Space (20);
            if (GUILayout.Button ("Copy from other"))
            {
                if (connectionCreator)
                    connectionCreator.CopyFromOther ();
            }
        }
    }
}