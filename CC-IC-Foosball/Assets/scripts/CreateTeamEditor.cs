using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
[CustomEditor(typeof(CreateTeam))]

public class CreateTeamEditor : Editor
{
    // SerializedProperty damageProp;
    // SerializedProperty armorProp;
    // SerializedProperty gunProp;
    SerializedProperty greenPlayerPrefab;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        // damageProp = serializedObject.FindProperty ("damage");
        // armorProp = serializedObject.FindProperty ("armor");
        // gunProp = serializedObject.FindProperty ("gun");
        greenPlayerPrefab = serializedObject.FindProperty("greenPlayerPrefab");

    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update ();

        // // Show the custom GUI controls.
        // EditorGUILayout.IntSlider (damageProp, 0, 100, new GUIContent ("Damage"));

        // // Only show the damage progress bar if all the objects have the same damage value:
        // if (!damageProp.hasMultipleDifferentValues)
        //     ProgressBar (damageProp.intValue / 100.0f, "Damage");

        // EditorGUILayout.IntSlider (armorProp, 0, 100, new GUIContent ("Armor"));

        // // Only show the armor progress bar if all the objects have the same armor value:
        // if (!armorProp.hasMultipleDifferentValues)
        //     ProgressBar (armorProp.intValue / 100.0f, "Armor");

        EditorGUILayout.PropertyField (greenPlayerPrefab, new GUIContent ("Green Player Prefab"));

        // Add a button to instantiate the prefab
        if (GUILayout.Button("Instantiate Prefab"))
        {
            // objectReferenceValue gets the actual Unity object assigned to this SerializedProperty.
            // In this case, it retrieves the prefab assigned in the inspector.
            GameObject prefab = greenPlayerPrefab.objectReferenceValue as GameObject;
            if (prefab != null)
            {
                // PrefabUtility is an Editor class that provides functions for working with prefabs in the editor.
                // PrefabUtility.InstantiatePrefab creates an instance of the prefab in the scene, preserving prefab links.
                // Note: PrefabUtility only works in the Unity Editor, not at runtime.

                // Generate a random position within a defined range
                Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(0f, 5f), Random.Range(-5f, 5f));

                // Generate a random rotation
                Quaternion randomRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

                // Instantiate the prefab in the scene with random position and rotation
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.transform.position = randomPosition;
                instance.transform.rotation = randomRotation;
            }
            else
            {
                Debug.LogWarning("Please assign a prefab to 'Green Player Prefab'.");
            }
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties ();
    }
}
