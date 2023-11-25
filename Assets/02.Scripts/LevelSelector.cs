using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private string stageName;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private Image fadePanel;

    private float _fadeTime = 1f;

    private void Awake()
    {
        if (fadePanel != null)
        {
            fadePanel.rectTransform.DOAnchorPosX(-90, 0.1f);
            fadePanel.rectTransform.DOAnchorPosX(2100, 1f);
        }
    }

    private void Start()
    {
        stageText.text = stageName;
    }

    public void OpenScene()
    {
        StartCoroutine(OpenSceneRoutine(_fadeTime));
    }

    private IEnumerator OpenSceneRoutine(float fadeTime)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.UIClick);

        fadePanel.rectTransform.DOAnchorPosX(-90, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        switch (stageName)
        {
            case "Tutorial Stage":
                AudioManager.Instance.PlayBGM(AudioManager.BGM.Stage1);
                break;
            case "Stage 1":
                AudioManager.Instance.PlayBGM(AudioManager.BGM.Stage1);
                break;
            case "Stage 2":
                AudioManager.Instance.PlayBGM(AudioManager.BGM.Stage2);
                break;
            case "Stage 3":
                AudioManager.Instance.PlayBGM(AudioManager.BGM.Stage3);
                break;
        }
        SceneManager.LoadScene(stageName);
    }

    public void ButtonClickSound()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.UIClick);
    }
}
