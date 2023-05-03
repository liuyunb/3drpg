using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMeau : MonoBehaviour
{
    Button newGame;
    Button continueGame;
    Button quitGame;

    PlayableDirector director;

    private void Awake()
    {
        newGame = transform.GetChild(1).GetComponent<Button>();
        continueGame = transform.GetChild(2).GetComponent<Button>();
        quitGame = transform.GetChild(3).GetComponent<Button>();
        director = FindObjectOfType<PlayableDirector>();

        newGame.onClick.AddListener(PlayTimeLine);
        continueGame.onClick.AddListener(ContinueGame);
        quitGame.onClick.AddListener(QuitGame);

        director.stopped += NewGame;

    }

    public void PlayTimeLine()
    {
        director.Play();
    }

    public void NewGame(PlayableDirector director)
    {
        SceneController.Instance.StartNewGame();
        Debug.Log("开始游戏");
    }

    public void ContinueGame()
    {
        if(SceneController.Instance.isPause)
            SceneController.Instance.Continue();
        Debug.Log("继续游戏");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Over");
    }
}
