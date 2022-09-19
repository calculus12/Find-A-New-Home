using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이거 이용하면 씬별로 음악 다르게 설정할 수 있음
//using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{   
    public static SoundManager instance;
    //myAudio는 효과음 재생용
    AudioSource myAudio;
    //BGMAudio는 계속해서 play
    private AudioSource BGMAudio;

    public AudioClip CoinAudio;
    public AudioClip JumpAudio;
    public AudioClip CollideAudio;
    public AudioClip BGM;
    public AudioClip SlideAudio;
    public AudioClip PurchaseAudio;
    public AudioClip ClickAudio;

    //기본 BGMvolume 크기
    public float masterVolumeBGM = 1f;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame updat
    void Start()
    {

        myAudio = GetComponent<AudioSource>();
        BGMAudio = GameObject.Find("BGMplayer").GetComponent<AudioSource>();
        PlayBGM();
    }
    //bgm 볼륨 조절 가능
    public void PlayBGM(float volume=1f)
    {
        BGMAudio.loop = true;
        BGMAudio.volume = volume*masterVolumeBGM;
        BGMAudio.clip = BGM;
        BGMAudio.Play();
    }
    // Update is called once per frame
    public void PlayCoinSound()
    {
        myAudio.PlayOneShot(CoinAudio);
    }
    public void PlaySlideSound()
    {
        myAudio.PlayOneShot(SlideAudio);
    }
    public void PlayJumpSound()
    {
        myAudio.PlayOneShot(JumpAudio);
    }
    public void PlayCollideSound()
    {
        myAudio.PlayOneShot(CollideAudio);
    }
    public void PlayClickSound()
    {
        myAudio.PlayOneShot(ClickAudio);
    }
    
}