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

    private void Start()
    {
        stageText.text = stageName;
    }
    public void OpenScene()
    {
        SceneManager.LoadScene(stageName);
    }
}
