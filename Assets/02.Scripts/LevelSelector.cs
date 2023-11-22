using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private string stageName;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private Image fadePanel;

    private float _fadeTime = 1f;

    private void Awake()
    {
        Fade.Instance.FadeIn(fadePanel, 0.1f);
        Fade.Instance.FadeOut(fadePanel, _fadeTime);
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
        Fade.Instance.FadeIn(fadePanel, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(stageName);
    }
}
