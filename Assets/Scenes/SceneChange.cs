using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneChange : MonoBehaviour
{
    public Image FadeImage;        // Assign in Inspector
    public float fadeOutDuration = 1f;
    public float fadeInDuration = 1.5f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        // Fade OUT (transparent → black)
        yield return StartCoroutine(Fade(0f, 1f, fadeOutDuration));

        // Load scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Small buffer (prevents visual popping in VR)
        yield return new WaitForSeconds(0.2f);

        // Fade IN (black → transparent)
        yield return StartCoroutine(Fade(1f, 0f, fadeInDuration));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color color = FadeImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            FadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure final value is exact
        FadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}