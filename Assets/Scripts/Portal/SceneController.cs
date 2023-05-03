using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : SingleTon<SceneController>,IEndGameObserver
{

    public GameObject PlayerPrefab;
    public SceneFader FaderPrefab;

    GameObject player;

    string sceneName = "lastScene";
    bool gameFinished;
    [HideInInspector]
    public bool isPause;
    public bool canPause;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.RegisterObserver(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Main")
        {
            isPause = true;
            canPause = true;
        }
    }

    private void FixedUpdate()
    {
        if (isPause && SceneManager.GetActiveScene().name != "Main" && canPause)
        {
            canPause = false;
            Pause();
        }
    }
    public void OnTransition(TransitionTo origin)
    {
        switch (origin.sceneType)
        {
            case TransitionTo.SceneType.SameScene: StartCoroutine(Transition(origin.destination));
                break;
            case TransitionTo.SceneType.DifferentScene:
                StartCoroutine(Transition(origin.sceneName, origin.destination));
                break;
        }

    }
    //同场景
    IEnumerator Transition(TransitionDestination.TransitionType transitionType)
    {
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance?.SaveData();
        QuestManager.Instance?.SaveData();

        player = GameManager.Instance.playerData.gameObject;
        var playerAgent = player.GetComponent<NavMeshAgent>();
        playerAgent.enabled = false;
        var destination = GetDestination(transitionType);
        if (destination != null)
        {
            player.transform.SetPositionAndRotation(destination.transform.position, destination.transform.rotation);
        }
        else
            player.transform.SetPositionAndRotation(transform.position, transform.rotation);
        playerAgent.enabled = true;
        yield return null;
    }
    //不同场景
    IEnumerator Transition(string sceneName, TransitionDestination.TransitionType transitionType)
    {
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance?.SaveData();
        QuestManager.Instance?.SaveData();


        yield return SceneManager.LoadSceneAsync(sceneName);
        var destination = GetDestination(transitionType);
        yield return player = Instantiate(PlayerPrefab, destination.transform.position, destination.transform.rotation);
        player.SetActive(true);
        SaveManager.Instance.LoadPlayerData();
        yield return null;
    }

    public TransitionDestination GetDestination(TransitionDestination.TransitionType transitionType)
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destination == transitionType)
                return item;
        }

        return null;
    }

    public void StartNewGame()
    {
        isPause = false;
        gameFinished = false;
        PlayerPrefs.DeleteAll();
        StartCoroutine(LoadLevel("Game"));
    }

    public void Pause()
    {
        StartCoroutine(PauseGame());
    }

    public void Continue()
    {
        isPause = false;
        StartCoroutine(LoadLevel(PlayerPrefs.GetString(sceneName)));
    }
    //IEnumerator LoadFirstLevel()
    //{

    //    yield return SceneManager.LoadSceneAsync("Game");
    //    var destination = GetDestination(TransitionDestination.TransitionType.Enter);
    //    player = Instantiate(PlayerPrefab, destination.transform.position, destination.transform.rotation);
    //    SaveManager.Instance.SavePlayerData();
    //    yield return null;
    //}

    IEnumerator PauseGame()
    {
        var fade = Instantiate(FaderPrefab);
        yield return fade.FadeOut(2f);
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveData();
        QuestManager.Instance.SaveData();


        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        yield return SceneManager.LoadSceneAsync("Main");
        yield return fade.FadeIn(2f);
        print("暂停");
    }

    //IEnumerator ContinueGame()
    //{
    //    yield return SceneManager.LoadSceneAsync(PlayerPrefs.GetString(sceneName));
    //    var destination = GetDestination(TransitionDestination.TransitionType.Enter);
    //    player = Instantiate(PlayerPrefab, destination.transform.position, destination.transform.rotation);
    //    SaveManager.Instance.LoadPlayerData();
    //    yield return null;
    //}

    IEnumerator LoadLevel(string scene)
    {
        var fade = Instantiate(FaderPrefab);
        yield return fade.FadeOut(2f);
        yield return SceneManager.LoadSceneAsync(scene);
        var destination = GetDestination(TransitionDestination.TransitionType.Enter);
        yield return player = Instantiate(PlayerPrefab, destination.transform.position, destination.transform.rotation);
        player.SetActive(true);
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance?.SaveData();
        yield return fade.FadeIn(2f);
        yield return null;
    }

    public void EndNotify()
    {
        if(!gameFinished)
        {
            gameFinished = true;
            Pause();
        }
    }
}
