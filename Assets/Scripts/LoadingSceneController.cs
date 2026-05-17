/*
    최초 작성일:26/05/17
    최종 변경일:26/05/17
    
    수정자
    - 김남우
    -
    
    목적
    - LoadingScene의 로직을 개발하기 위해 실제 씬 로딩과 최소 로딩 시간 기준으로
      진행도를 계산하여 UI에 표시하는 로직을 설계
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text progressText;

    [Header("Loading Option")]
    [SerializeField] private float minimumLoadingTime = 1.5f;

    private void Start()
    {
        minimumLoadingTime = 3f;
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        string targetSceneName = SceneLoader.TargetScene.ToString();

        AsyncOperation operation = SceneManager.LoadSceneAsync(targetSceneName);

        if (operation == null)
        {
            Debug.LogError($"Scene load failed. Target Scene: {targetSceneName}");
            yield break;
        }

        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (true)
        {
            elapsedTime += Time.deltaTime;

            // 실제 씬 로딩 진행도
            // allowSceneActivation이 false면 progress가 0.9에서 멈추므로 0.9로 나눠서 0~1로 변환
            float realLoadProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // 최소 로딩 시간 기준 진행도
            float timeProgress = minimumLoadingTime <= 0f
                ? 1f
                : Mathf.Clamp01(elapsedTime / minimumLoadingTime);

            // 둘 중 더 낮은 값을 보여줌
            // 실제 로딩이 끝났어도 시간이 덜 지났으면 시간 기준으로 올라감
            // 시간이 다 지났어도 실제 로딩이 덜 됐으면 실제 로딩 기준으로 올라감
            float displayProgress = Mathf.Min(realLoadProgress, timeProgress);

            UpdateLoadingUI(displayProgress);

            bool isRealLoadComplete = realLoadProgress >= 1f;
            bool isMinimumTimeComplete = timeProgress >= 1f;

            if (isRealLoadComplete && isMinimumTimeComplete)
            {
                UpdateLoadingUI(1f);

                yield return new WaitForSeconds(0.1f);

                operation.allowSceneActivation = true;

                yield break;
            }

            yield return null;
        }
    }

    private void UpdateLoadingUI(float progress)
    {
        if (progressBar != null)
        {
            progressBar.value = progress;
        }

        if (progressText != null)
        {
            int percent = Mathf.RoundToInt(progress * 100f);
            progressText.text = $"{percent}%";
        }
    }
}