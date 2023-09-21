using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FPSFramework
{
    [CustomEditor(typeof(Actor))]
    public class ActorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Actor actor = (Actor)target;

            if (actor.GetComponentInParent<ActorManager>()) return;

            EditorGUILayout.HelpBox("Actor must have an actor manager in order to work if needed to respwan", MessageType.Error);
            if(GUILayout.Button("Fix Error"))
            {
                ActorManager actorManager = new GameObject($"{actor.name} Manager").AddComponent<ActorManager>();
                actorManager.ActorName = actor.Name;
                actorManager.randomNames.Add(actor.Name);

                actorManager.transform.Reset();
                actor.transform.parent = actorManager.transform;

                Selection.activeTransform = actorManager.transform;

                Undo.RecordObject(actorManager, "Created Actor Manager");
                Undo.RecordObject(actor.transform.parent, "Parented Actor");
            }
        }
    }
}