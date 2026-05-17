/*
    최초 작성일:26/05/17
    최종 변경일:26/05/17
    
    수정자
    - 김남우
    -
    
    목적
    - LoadingScene의 로직을 개발하기 위해 SceneName Enum의 관리와
      Loading씬으로 이동하는 로직을 설계
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    MainMenu,
    Loading,
    GameTop,
    GameBoss
}

public static class SceneLoader
{
    public static SceneName TargetScene{ get; private set; }

    public static void LoadScene(SceneName targetScene)
    {
        TargetScene = targetScene;
        SceneManager.LoadScene(SceneName.Loading.ToString());
    }
}
