using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
	public static AudioSettingsManager instance;
	
	[SerializeField] private AudioMixer audioMixer; // audioMixer 참조

	private const string MasterVolumeParam = "MasterVolume"; // 전체 음량
	private const string BGMVolumeParam    = "BGMVolume"; // 배경음 음량
	private const string SFXVolumeParam    = "SFXVolume"; // 효과음 음량
	private const string UIVolumeParam    = "UIVolume"; // UI 음량

	private GameSettingsData settings;
	
	public float MasterVolume => settings.masterVolume;
	public float BGMVolume    => settings.bgmVolume;
	public float SFXVolume    => settings.sfxVolume;
	public float UIVolume     => settings.uiVolume;

	private void Awake()
	{
		// 소리 매니저 싱글톤 적용
		// 소리 매니저는 모든 씬에서 활용되기 때문에 지워지지 않게 설정함
		if(instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
		
		settings = GameSettingsStorage.Load();
		ApplyAllVolumes();
		
		Debug.Log("Settings Path: " +GameSettingsStorage.GetFilePath());
	}

	// Master Volume을 설정
	public void SetMasterVolume(float value)
	{
		settings.masterVolume = value;
		SetMixerVolume(MasterVolumeParam, value);
		GameSettingsStorage.Save(settings);
	}

	// BGM Volume을 설정
	public void SetBGMVolume(float value)
	{
		settings.bgmVolume = value;
		SetMixerVolume(BGMVolumeParam, value);
		GameSettingsStorage.Save(settings);
	}

	// SFX Volume을 설정
	public void SetSFXVolume(float value)
	{
		settings.sfxVolume = value;
		SetMixerVolume(SFXVolumeParam, value);
		GameSettingsStorage.Save(settings);
	}
	
	// UI Volume을 설정
	public void SetUIVolume(float value)
	{
		settings.uiVolume = value;
		SetMixerVolume(UIVolumeParam, value);
		GameSettingsStorage.Save(settings);
	}

	private void ApplyAllVolumes()
	{
		SetMixerVolume(MasterVolumeParam, settings.masterVolume);
		SetMixerVolume(BGMVolumeParam, settings.bgmVolume);
		SetMixerVolume(SFXVolumeParam, settings.sfxVolume);
		SetMixerVolume(UIVolumeParam, settings.uiVolume);
	}

	// audiomixer의 volume 값은 0~1이 아닌 -80 ~ 0db 식으로 가므로 0 ~ 1 값을 db로 변환해야 함
	private void SetMixerVolume(string parameterName, float value)
	{
		float volumeDb;
		
		// 슬라이더의 값이 최소가 되면 음량을 완전히 줄이기 위해 -80db로 설정
		if (value <= 0.0001f)
		{
			volumeDb = -80f;
		}
		else
		{
			// 0 ~ 1 값을 db로 변환 (log10(0.5) * 20 = -6db, log10(1) * 20 = 0db)
			volumeDb = Mathf.Log10(value) * 20f;
		}

		audioMixer.SetFloat(parameterName, volumeDb);
	}
}