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

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Btn()
    {
        SceneLoader.LoadScene(SceneName.GameBoss);
    }
}
