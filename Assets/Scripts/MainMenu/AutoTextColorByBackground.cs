using UnityEngine;
using UnityEngine.UI;

public class TextAutoReadableOppositeColor : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Text targetText;
	[SerializeField] private Image backgroundImage;

	[Header("Settings")]
	[SerializeField] private float changeSpeed = 5f;

	[Tooltip("글자색이 너무 어둡거나 밝을 때 보정할 기준")]
	[SerializeField, Range(0f, 1f)] private float brightnessThreshold = 0.5f;

	private Color targetColor;

	private void Awake()
	{
		if (targetText == null)
			targetText = GetComponent<Text>();
	}

	private void Update()
	{
		if (targetText == null || backgroundImage == null)
			return;

		targetColor = GetReadableOppositeColor(backgroundImage.color);

		targetText.color = Color.Lerp(
			targetText.color,
			targetColor,
			Time.deltaTime * changeSpeed
		);
	}

	private Color GetReadableOppositeColor(Color bg)
	{
		// 1차: RGB 반전색
		Color opposite = new Color(
			1f - bg.r,
			1f - bg.g,
			1f - bg.b,
			1f
		);

		// 반전색의 밝기 계산
		float brightness = GetBrightness(opposite);

		// 너무 어두우면 밝게 보정
		if (brightness < 0.25f)
		{
			opposite = Color.Lerp(opposite, Color.white, 0.45f);
		}
		// 너무 밝으면 어둡게 보정
		else if (brightness > 0.85f)
		{
			opposite = Color.Lerp(opposite, Color.black, 0.35f);
		}

		return opposite;
	}

	private float GetBrightness(Color color)
	{
		return color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
	}
}