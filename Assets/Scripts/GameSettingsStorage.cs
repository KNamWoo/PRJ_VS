using System.IO;
using UnityEngine;

public static class GameSettingsStorage
{
	private static readonly string FilePath =
		Path.Combine(Application.persistentDataPath, "Settings.json");

	public static GameSettingsData Load()
	{
		if(!File.Exists(FilePath))
		{
			GameSettingsData defaultData = new GameSettingsData();
			Save(defaultData);
			return defaultData;
		}

		string json = File.ReadAllText(FilePath);

		if(string.IsNullOrWhiteSpace(json))
		{
			return new GameSettingsData();
		}

		return JsonUtility.FromJson<GameSettingsData>(json);
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