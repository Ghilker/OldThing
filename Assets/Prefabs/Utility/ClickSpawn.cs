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
    public RoomGenerator gen;

    private void OnEnable()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }
        SceneView.duringSceneGui += OnScene;
    }

    void OnScene(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 2)
        {
            Vector3 mousePos = e.mousePosition;
            Vector3 worldPosition = GetCurrentMousePositionInScene();
            gen.GenerateRoom(worldPosition);
            //Debug.DrawLine(gameObject.transform.position, actualScreenPosition, Color.blue, 10);
            e.Use();
        }
    }
    Vector3 GetCurrentMousePositionInScene()
    {
        Vector3 mousePosition = Event.current.mousePosition;
        var placeObject = HandleUtility.PlaceObject(mousePosition, out var newPosition, out var normal);
        return placeObject ? newPosition : HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10);
    }
}
#endif