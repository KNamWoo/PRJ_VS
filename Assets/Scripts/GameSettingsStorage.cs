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