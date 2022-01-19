using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameInit gameInit;

    public DungeonSize dungeonSize = DungeonSize.TINY;


    public Dropdown dropdownMenu;

    List<string> dungeonSizeDropdownList;
    public int sceneID = 1;

    private void Start()
    {
        dungeonSizeDropdownList = new List<string>() { "Tiny", "Small", "Medium", "Big", "Huge" };
        dropdownMenu.ClearOptions();
        dropdownMenu.AddOptions(dungeonSizeDropdownList);
    }

    public void PlayGame()
    {
        gameInit.dungeonDepth = (int)dungeonSize;
        gameInit.ChangeScene(sceneID);
    }

    public void OnDropdownSelection()
    {
        dungeonSize = (DungeonSize)(dropdownMenu.value + 2);
    }

}
