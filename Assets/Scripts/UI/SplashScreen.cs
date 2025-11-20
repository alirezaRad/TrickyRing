using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private float splashDuration = 2f;

        void Start()
        {
            StartCoroutine(FadeInOut());
        }

        IEnumerator FadeInOut()
        {
            yield return StartCoroutine(Fade(0f, 1f));
            yield return new WaitForSeconds(splashDuration);
            yield return StartCoroutine(Fade(1f, 0f));
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        IEnumerator Fade(float startAlpha, float endAlpha)
        {
            float elapsedTime = 0f;
            canvasGroup.alpha = startAlpha;
            
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
                yield return null;
            }
            
            canvasGroup.alpha = endAlpha;
        }
    }
}
