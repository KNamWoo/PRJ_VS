/*
    최초 작성일:26/05/17
    최종 변경일:26/05/17
    
    수정자
    - 김남우
    -
    
    목적
    - 게임의 전반적인 ManageMent를 위해
*/

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject escapeMenu;
    InputAction cancelAct;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        cancelAct = InputSystem.actions.FindAction("Cancel");
    }

    void Update()
    {
        if(cancelAct.IsPressed()){
            Esc();
        }
    }

    public void Btn()
    {
        SceneLoader.LoadScene(SceneName.GameBoss);
    }

    public void Esc()
    {
        if(SceneLoader.ThisScene() == "MainMenu")
        {
            return;
        }
        
        escapeMenu.SetActive(!escapeMenu.activeSelf);
    }
}
