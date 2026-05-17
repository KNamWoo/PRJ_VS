/*
    최초 작성일:26/05/09
    최종 변경일:26/05/17
    
    수정자
    - 김남우
    -
    
    목적
    - 저장할 데이터 명시
*/

using System;

[Serializable]
public class GameSettingsData
{
    public float masterVolume = 1f;
    public float bgmVolume    = 1f;
    public float sfxVolume    = 1f;
    public float uiVolume     = 1f;
}
