using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject playerPrefab;
    public CanvasFader canvasFaderPrefab;
    private GameObject player;
    private NavMeshAgent playerAgent;
    private bool fadeFinished;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinished = true;
    }
    public void TransitionToDesitination(PortalTransition portalTransition)
    {
        switch (portalTransition.transitionType)
        {
            case PortalTransition.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, portalTransition.destinationPoint));
                break;
            case PortalTransition.TransitionType.DifferentScene:
                StartCoroutine(Transition(portalTransition.sceneName, portalTransition.destinationPoint));
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationType destinationType)
    {
        //保存数据
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveData();
        QuestManager.Instance.SaveQuestSystem();
        
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            CanvasFader canvasFader = Instantiate(canvasFaderPrefab);
            yield return StartCoroutine(canvasFader.FadeOut(canvasFader.fadeOutTime));
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationType).transform.position,
                GetDestination(destinationType).transform.rotation);
            yield return StartCoroutine(canvasFader.FadeIn(canvasFader.fadeInTime));
            SaveManager.Instance.LoadPlayerData();
        }
        else
        {
            player = GameManager.Instance.playerStates.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            player.transform.
                SetPositionAndRotation(GetDestination(destinationType).transform.position,
                    GetDestination(destinationType).transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationType destinationType)
    {
        var destinations 
            = FindObjectsByType<TransitionDestination>(FindObjectsSortMode.None);
        for (int i = 0; i < destinations.Length; i++)
        {
            if (destinations[i].destinationType == destinationType)
            {
                return destinations[i];
            }
        }

        return null;
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMain("Main Menu"));
    }
    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadGame("Outside"));
    }

    public void TransitionToLoadLevel()
    {
        StartCoroutine(LoadGame(SaveManager.Instance.SceneName));
    }

    private IEnumerator LoadGame(string sceneName)
    {
        CanvasFader canvasFader = Instantiate(canvasFaderPrefab);
        if (sceneName != "")
        {
            yield return StartCoroutine(canvasFader.FadeOut(canvasFader.fadeOutTime));
            yield return SceneManager.LoadSceneAsync(sceneName);
        
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrances().position,
                GameManager.Instance.GetEntrances().rotation);
            //保存数据
            SaveManager.Instance.SavePlayerData();
            InventoryManager.Instance.SaveData();
        
            yield return StartCoroutine(canvasFader.FadeIn(canvasFader.fadeInTime));
            yield break;
        }
    }

    IEnumerator LoadMain(string name)
    {
        CanvasFader fader = Instantiate(canvasFaderPrefab);
        yield return StartCoroutine(fader.FadeOut(fader.fadeOutTime));
        yield return SceneManager.LoadSceneAsync(name);
        yield return StartCoroutine(fader.FadeIn(fader.fadeInTime));
    }

    public void EndNotify()
    {
        if (fadeFinished)
        {
            fadeFinished = false;
            StartCoroutine(LoadMain("Main Menu"));
        }
    }
}
