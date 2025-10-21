using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
   private Button newGameBtn;
   private Button continueBtn;
   private Button quitBtn;
   private PlayableDirector director;

   private void Awake()
   {
      newGameBtn = transform.GetChild(1).GetComponent<Button>();
      continueBtn = transform.GetChild(2).GetComponent<Button>();
      quitBtn = transform.GetChild(3).GetComponent<Button>();
      
      newGameBtn.onClick.AddListener(PlayTimeLine);
      continueBtn.onClick.AddListener(Continue);
      quitBtn.onClick.AddListener(QuitGame);
      
      //通过TimeLine实现开始游戏动画
      director = FindFirstObjectByType<PlayableDirector>();
      //使用PlayableDirector自带的事件函数来添加NewGame方法
      director.stopped += NewGame;
   }

   private void PlayTimeLine()
   {
       director.Play();
   }

   private void NewGame(PlayableDirector obj)
   {
       PlayerPrefs.DeleteAll();
       //转换场景
       SceneController.Instance.TransitionToFirstLevel();
   }

   private void Continue()
   {
       //转换场景，读取进度
       SceneController.Instance.TransitionToLoadLevel();
   }
   private void QuitGame()
   {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else      
      Application.Quit();
#endif
      Debug.Log("游戏结束");
   }
}
