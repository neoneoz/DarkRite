using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    public Image screenMask;
    Color fadeTo;
    private float fadeTime;
    private float fadeDuration;
    private bool isFading;

    void Update()
    {
        ColorChanger();
    }

    public void FadeOut(Color fadeFrom, Color fadeTo, float fadeDuration)
    {
        screenMask.color = fadeFrom;
        this.fadeTo = fadeTo;
        this.fadeDuration = fadeDuration;
        isFading = true;
    }

    void ColorChanger()
    {
        if (isFading)
        {

            screenMask.color = Color.Lerp(screenMask.color, fadeTo, fadeTime);

            if (fadeTime < 1)
            {
                fadeTime += Time.deltaTime / fadeDuration;
            }
            if (screenMask.color == fadeTo)
                isFading = false;
        }
    }

    public void StopFading()
    {
        isFading = false;
    }

    public void LoadNextScene(int ID)
    {
        SceneManager.LoadScene(ID);
    }

}