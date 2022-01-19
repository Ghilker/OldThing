using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour
{

    public BoardMaker boardMaker;
    public int dungeonDepth = 4;

    AsyncOperation sceneLoading;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(int sceneID)
    {
        sceneLoading = SceneManager.LoadSceneAsync(sceneID);
        StartCoroutine(SceneLoader());
    }

    IEnumerator SceneLoader()
    {

        float progressBar = Mathf.Clamp01(sceneLoading.progress / 0.9f);
        while (progressBar < 1 && !sceneLoading.isDone)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        OnSceneLoaded();
    }

    void OnSceneLoaded()
    {
        boardMaker = GameObject.FindGameObjectWithTag("BoardGenerator").GetComponent<BoardMaker>();
        boardMaker.BoardInit(dungeonDepth);
    }

}
