using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{

    public BoardMaker boardMaker;

    // Start is called before the first frame update
    void Start()
    {
        boardMaker.BoardInit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
