/*
    최초 작성일:26/05/09
    최종 변경일:26/05/17
    
    수정자
    - 김남우
    -
    
    목적
    - AudioMixer를 활용하여 게임의 음량 설정을 관리하는 AudioSettingsManager 개발
	- GameSettingsData와 GameSettingsStorage를 활용하여 음량 설정을 저장/불러오기 기능 구현
*/

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
	
	// 현재 설정된 음량 값을 외부에서 읽을 수 있도록 프로퍼티로 제공
	public float MasterVolume => settings.masterVolume;
	public float BGMVolume    => settings.bgmVolume;
	public float SFXVolume    => settings.sfxVolume;
	public float UIVolume     => settings.uiVolume;

	// 설정할때 취소하면 되돌릴 수 있도록 임시로 저장하는 변수들
	public float mv; // mastervolume
	public float bg; // bgmvolume
	public float sf; // sfxvolume
	public float ui; // uivolume

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

		if(audioMixer == null)
		{
			Debug.LogError("[AudioSettingsManager] AudioMixer is null!");
			enabled = false;
			return;
		}
		
		settings = GameSettingsStorage.Load() ?? new GameSettingsData();
		settings.masterVolume = Mathf.Clamp01(settings.masterVolume);
		settings.bgmVolume    = Mathf.Clamp01(settings.bgmVolume);
		settings.sfxVolume    = Mathf.Clamp01(settings.sfxVolume);
		settings.uiVolume     = Mathf.Clamp01(settings.uiVolume);
		Debug.Log($"[AudioSettingsManager] Loaded settings: Master={settings.masterVolume}, BGM={settings.bgmVolume}, SFX={settings.sfxVolume}, UI={settings.uiVolume}");
		
		Debug.Log("Settings Path: " +GameSettingsStorage.GetFilePath());
	}

	private void Start()
	{
		ApplyAllVolumes();
	}

	// Master Volume을 설정
	public void SetMasterVolume(float value)
	{
		mv = value;
		SetMixerVolume(MasterVolumeParam, value);
	}

	// BGM Volume을 설정
	public void SetBGMVolume(float value)
	{
		bg = value;
		SetMixerVolume(BGMVolumeParam, value);
	}

	// SFX Volume을 설정
	public void SetSFXVolume(float value)
	{
		sf = value;
		SetMixerVolume(SFXVolumeParam, value);
	}
	
	// UI Volume을 설정
	public void SetUIVolume(float value)
	{
		ui = value;
		SetMixerVolume(UIVolumeParam, value);
	}

	public void SaveSettings()
	{
		settings.masterVolume = mv;
		settings.bgmVolume = bg;
		settings.sfxVolume = sf;
		settings.uiVolume = ui;
		GameSettingsStorage.Save(settings);
	}

	public void CancelSettings()
	{
		/*mv = settings.masterVolume;
		bg = settings.bgmVolume;
		sf = settings.sfxVolume;
		ui = settings.uiVolume;*/
		SetMixerVolume(MasterVolumeParam, mv=settings.masterVolume);
		SetMixerVolume(BGMVolumeParam, bg=settings.bgmVolume);
		SetMixerVolume(SFXVolumeParam, sf=settings.sfxVolume);
		SetMixerVolume(UIVolumeParam, ui=settings.uiVolume);
	}

	public void ApplyAllVolumes()
	{
		SetMixerVolume(MasterVolumeParam, settings.masterVolume);
		SetMixerVolume(BGMVolumeParam, settings.bgmVolume);
		SetMixerVolume(SFXVolumeParam, settings.sfxVolume);
		SetMixerVolume(UIVolumeParam, settings.uiVolume);
	}

	// audiomixer의 volume 값은 0~1이 아닌 -80 ~ 0db 식으로 가므로 0 ~ 1 값을 db로 변환해야 함
	public void SetMixerVolume(string parameterName, float value)
	{
		if(audioMixer == null)
		{
			Debug.LogError("[AudioSettingsManager] AudioMixer is null! Cannot set volume.");
			return;
		}

		value = Mathf.Clamp01(value);
		
		// 슬라이더의 값이 최소가 되면 음량을 완전히 줄이기 위해 -80db로 설정
		// 0 ~ 1 값을 db로 변환 (log10(0.5) * 20 = -6db, log10(1) * 20 = 0db)
		float volumeDb = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;

		audioMixer.SetFloat(parameterName, volumeDb);
	}
}