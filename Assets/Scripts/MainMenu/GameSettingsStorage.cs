/*
    최초 작성일:26/05/09
    최종 변경일:26/05/17
    
    수정자
    - 김남우
    -
    
    목적
    - 게임 설정 데이터를 저장하고 불러오는 기능을 개발하기 위해
	  GameSettingsData 클래스와 연동하여 JSON 형식으로 파일에 저장하는 로직을 설계
*/

using System.IO;
using UnityEngine;

public static class GameSettingsStorage
{
	private static readonly string FilePath =
		Path.Combine(Application.persistentDataPath, "Settings.json");

	public static GameSettingsData Load()
	{
		try
		{
			if(!File.Exists(FilePath))
			{
				var defaults = new GameSettingsData();
				Save(defaults);
				return defaults;
			}

			string json = File.ReadAllText(FilePath);

			if(string.IsNullOrWhiteSpace(json))
			{
				var defaults = new GameSettingsData();
				Save(defaults);
				return defaults;
			}

			var data = JsonUtility.FromJson<GameSettingsData>(json);
			if(data == null)
				throw new InvalidDataException("Settings file is empty");
			data.masterVolume = Mathf.Clamp01(data.masterVolume);
			data.bgmVolume    = Mathf.Clamp01(data.bgmVolume);
			data.sfxVolume    = Mathf.Clamp01(data.sfxVolume);
			data.uiVolume     = Mathf.Clamp01(data.uiVolume);

			return data;
		} catch (System.Exception e)
		{
			Debug.LogWarning($"[GameSettingsStorage] Load failed, fallback to defaults: {e.Message}");
			var defaults = new GameSettingsData();
			Save(defaults);
			return defaults;
		}
	}

	public static void Save(GameSettingsData data)
	{
		string json = JsonUtility.ToJson(data, true);
		File.WriteAllText(FilePath, json);
	}

	public static string GetFilePath()
	{
		return FilePath;
	}
}