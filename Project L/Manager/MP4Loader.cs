using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MP4Loader : MonoBehaviour
{
    [SerializeField] VideoClip[] videoes;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject rawIamge;

    public enum Video { BanditReader, GiantSpider}

    static MP4Loader instance = null;
    public static MP4Loader Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<MP4Loader>();
                if (!instance)
                    instance = new GameObject("MP4Player").AddComponent<MP4Loader>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
            Destroy(gameObject);

        rawIamge.SetActive(false);
    }

    public void LoadVideo(Video video)
    {
        SoundMgr.Instance.StopBackgroundAudio();
        SoundMgr.Instance.StopMoveSound();
        videoPlayer.clip = videoes[(int)video];
        videoPlayer.Play();
        

        StartCoroutine(OnPlay());
    }

    IEnumerator OnPlay()
    {
        Time.timeScale = 0;
        while (!videoPlayer.isPlaying) yield return null;
        rawIamge.SetActive(true);

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        Time.timeScale = 1;

        SoundMgr.Instance.PlayBackgroundAudio();
        rawIamge.SetActive(false);
    }
}
