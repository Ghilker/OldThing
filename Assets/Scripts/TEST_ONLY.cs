using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class TEST_ONLY : MonoBehaviour
{

    private Grid<GameObject> grid;

    public GameObject testObj;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = new Vector3(0, 0, 0);
        grid = new Grid<GameObject>(5, 5, 1, position, (Grid<GameObject> g, int x, int y) => null);
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                GameObject toInstantiate;
                if ((x == 0 || x == 4 || y == 0 || y == 4) && (x != 2 && y != 2))
                {
                    grid.SetGridObject(x, y, toInstantiate = Instantiate(testObj, new Vector3(x, y) + position, Quaternion.identity));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = MouseCamera.GetMouseWorldPosition();
        }
    }
}

public class HeatMapObject
{
    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapObject> grid;
    private int value;
    private int x;
    private int y;

    public HeatMapObject(Grid<HeatMapObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }
    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
