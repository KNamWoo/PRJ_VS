/*
    최초 작성일:26/05/17
    최종 변경일:26/05/25
    
    수정자
    - 김남우
    -
    
    목적
    - 게임의 전반적인 ManageMent를 위해
    - Esc 메뉴와 하위 메뉴 관리
*/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public        GameObject           escapeMenu;

    [Header("Esc 메뉴 위에 뜨는 창들")]
    public GameObject volumeSetting;
    public GameObject equipmentDetail; // InfoMenu

    [Header("뒤쪽 Esc 메뉴 클릭 방지용")]
    public GameObject modalBlocker;

    InputAction cancelAct;

    private bool isPaused = false;

    // volumeSetting, equipmentDetail 같은 현재 열린 하위 메뉴를 저장
    private readonly Stack<GameObject> uiStack = new Stack<GameObject>();

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        cancelAct = InputSystem.actions.FindAction("Cancel");

        if (cancelAct != null)
        {
            cancelAct.Enable();
        }
        else
        {
            Debug.LogWarning("Cancel InputAction을 찾지 못했습니다.");
        }

        InitUI();
    }

    void Update()
    {
        if (cancelAct != null && cancelAct.WasPressedThisFrame())
        {
            Esc();
        }
    }

    private void InitUI()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (escapeMenu != null)
        {
            escapeMenu.SetActive(false);
        }

        if (volumeSetting != null)
        {
            volumeSetting.SetActive(false);
        }

        if (equipmentDetail != null)
        {
            equipmentDetail.SetActive(false);
        }

        if (modalBlocker != null)
        {
            modalBlocker.SetActive(false);
        }

        uiStack.Clear();
    }

    public void Btn()
    {
        SceneLoader.LoadScene(SceneName.GameBoss);
    }

    public void Esc()
    {
        if (SceneLoader.ThisScene() == "MainMenu")
        {
            return;
        }

        // 1. volumeSetting, equipmentDetail 같은 하위 메뉴가 열려 있으면
        // 그 메뉴만 닫고 끝낸다.
        // 그래서 게임 재개까지 가지 않는다.
        if (uiStack.Count > 0)
        {
            CloseTopMenu();
            return;
        }

        // 2. 하위 메뉴는 없고 escapeMenu만 열려 있으면 재개
        if (isPaused)
        {
            Resume();
            return;
        }

        // 3. 아무것도 안 열려 있으면 Esc 메뉴 열기
        Pause();
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (escapeMenu != null)
        {
            escapeMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        CloseAllMenus();

        if (escapeMenu != null)
        {
            escapeMenu.SetActive(false);
        }
    }

    public void OpenVolumeSetting()
    {
        OpenMenu(volumeSetting);
    }

    public void OpenEquipmentDetail()
    {
        OpenMenu(equipmentDetail);
    }

    public void CloseCurrentMenu()
    {
        if (uiStack.Count > 0)
        {
            CloseTopMenu();
        }
    }

    private void OpenMenu(GameObject menu)
    {
        if (menu == null)
        {
            Debug.LogWarning("열려고 하는 메뉴가 비어 있습니다.");
            return;
        }

        // 혹시 일시정지 상태가 아닌데 하위 메뉴가 열리는 경우 방어
        if (!isPaused)
        {
            Pause();
        }

        // 뒤쪽 escapeMenu 클릭 방지
        if (modalBlocker != null)
        {
            modalBlocker.SetActive(true);
        }

        menu.SetActive(true);
        uiStack.Push(menu);
    }

    private void CloseTopMenu()
    {
        if (uiStack.Count <= 0)
        {
            return;
        }

        GameObject topMenu = uiStack.Pop();

        if (topMenu != null)
        {
            if(topMenu == volumeSetting)
            {
                AudioSettingsManager.instance.CancelSettings();
            }
            topMenu.SetActive(false);
        }

        // 하위 메뉴가 전부 닫혔으면 blocker도 끈다.
        if (uiStack.Count == 0)
        {
            if (modalBlocker != null)
            {
                modalBlocker.SetActive(false);
            }
        }
    }

    private void CloseAllMenus()
    {
        while (uiStack.Count > 0)
        {
            GameObject menu = uiStack.Pop();

            if (menu != null)
            {
                menu.SetActive(false);
            }
        }

        if (modalBlocker != null)
        {
            modalBlocker.SetActive(false);
        }
    }

    public void GameQuit()
    {
        EditorApplication.isPlaying = false; // 에디터에서는 플레이 모드를 종료
        Application.Quit(); // 빌드된 게임에서는 애플리케이션 종료
    }
}