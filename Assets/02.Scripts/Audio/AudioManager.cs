using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class Volume
{
    public float bgmVolume = 0.2f;
    public float sfxVolume = 0.2f;
}

public class AudioManager : Singleton<AudioManager>
{
    public enum BGM
    {
        Start, Stage1, Stage2, Stage3, Boss
    }

    public enum Sfx
    {
        GameClear, GameOver,
        PlayerAttack, PlayerHit, PlayerJump, PlayerDash,
        PigAttack,
        BossHit, BossAttack, CrabbyAttack, CaptainAttack, SeaShellAttack, ToothAttack,
        UIClick, LifeUpdate,
    }

    public Volume nowVoluem = new Volume(); // ����
    public string path; // ���
    string fileName = "voluem Save";

    [Header("BGM")]
    [SerializeField] private AudioClip[] bgmClips;
    public AudioSource bgmAudioSource;

    [Header("SFX")]
    [SerializeField] private AudioClip[] sfxClips;
    private GameObject[] audioManagers;
    private AudioSource[] sfxAudioSources;
    private int channelIndex;

    protected override void Awake()
    {
        path = Application.persistentDataPath + "/";

        #region Json
        string data = JsonUtility.ToJson(nowVoluem);
        File.WriteAllText(path + fileName, data);
        #endregion

        bgmAudioSource = transform.Find("BGM").GetComponent<AudioSource>();
        sfxAudioSources = transform.Find("SFX").GetComponents<AudioSource>();
        channelIndex = sfxAudioSources.Length;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

        audioManagers = GameObject.FindGameObjectsWithTag("AudioManager");

        if (audioManagers.Length >= 2)
        {
            for (int i = 1; i < audioManagers.Length; i++)
            {
                Destroy(audioManagers[i]);
            }
        }
    }

    public void PlayBGM(BGM bgm)
    {
        if (bgmAudioSource.clip == bgmClips[(int)bgm])
            return;

        bgmAudioSource.clip = bgmClips[(int)bgm];
        bgmAudioSource.Play();
    }

    public void PlaySFX(Sfx sfx, float pitch = 1)
    {
        for (int index = 0; index < sfxAudioSources.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxAudioSources.Length;

            if (sfxAudioSources[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxAudioSources[loopIndex].clip = sfxClips[(int)sfx];
            sfxAudioSources[loopIndex].pitch = pitch;
            sfxAudioSources[loopIndex].Play();
            break;
        }
    }

    public void SaveData() // ����
    {
        try
        {
            string data = JsonUtility.ToJson(nowVoluem);
            File.WriteAllText(path + fileName, data);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        #region ����� ��
        // AudioManager.Instance.nowVoluem.���ϴ� ���� = �����Ϸ��� ������;
        // AudioManager.Instance.SaveData();
        #endregion
    }

    public void LoadData() // �ҷ�����
    {
        try
        {
            string data = File.ReadAllText(path + fileName);
            nowVoluem = JsonUtility.FromJson<Volume>(data);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        #region ����� ��
        // AudioManager.Instance.LoadData();
        // ���� ���� = AudioManager.Instance.nowVoluem.�������� ����;
        #endregion
    }
}
