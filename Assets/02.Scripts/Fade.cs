using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Fade : Singleton<Fade>
{
    public void FadeIn(Image fadeImage, float fadeTime)
    {
        fadeImage.rectTransform.DOAnchorPosX(-90, fadeTime);
    }

    public void FadeOut(Image fadeImage, float fadeTime)
    {
        fadeImage.rectTransform.DOAnchorPosX(2100, fadeTime);
    }
}
