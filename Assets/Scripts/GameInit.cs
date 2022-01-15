using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{

    public BoardMaker boardMaker;
    public int dungeonDepth = 4;

    // Start is called before the first frame update
    void Start()
    {
        boardMaker.BoardInit(dungeonDepth);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
