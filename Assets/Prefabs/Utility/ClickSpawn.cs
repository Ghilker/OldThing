using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
/// <summary>
/// 
/// </summary>
[ExecuteInEditMode]
public class ClickSpawn : MonoBehaviour
{
    public GameObject toInstantiate;
    public GameObject[] allObjects;

    private void OnEnable()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }
        SceneView.duringSceneGui += OnScene;
    }

    void OnScene(SceneView scene)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 2)
        {
            allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject gameObject in allObjects)
            {
                BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
                CircleCollider2D circle = gameObject.GetComponent<CircleCollider2D>();
                if (box)
                {
                    box.offset = Vector2.zero;
                }
                if (circle)
                {
                    circle.offset = Vector2.zero;
                }
            }
            /*Vector3 mousePos = e.mousePosition;
            Vector3 worldPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            worldPosition.z = 0;
            Vector3Int worldPositionInt = Vector3Int.FloorToInt(worldPosition);
            Instantiate(toInstantiate, worldPositionInt, Quaternion.identity);*/
            e.Use();
        }
    }
}
#endif