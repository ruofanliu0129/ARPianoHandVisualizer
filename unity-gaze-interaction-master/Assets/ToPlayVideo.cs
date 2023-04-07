using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace MoviePlay
{

    public class ToPlayVideo : MonoBehaviour
    {
        //region parameters
        #region 参数
        public VideoPlayer videoPlayer;
        //videoPlayer.playbackSpeed=5.0F;

        public RawImage rawImage;

        public string[] movieUrl;

        internal bool isPlay = false;

        public Image bofangButtonImage;

        public Sprite[] thisSprite;
        //Slider
        public Slider videoTimeSlider;

        #endregion

        //region basic methods
        #region 基本方法 
        // Use this for initialization
        void Start()
        {

            BeginString();
        }

        // Update is called once per frame
        void Update()
        {

            PlayMovie();
        }

        #endregion

        //region private methods
        #region 私有方法
        /// <summary>
        /// initialize
        /// </summary>
        void BeginString()
        {

            //videoPlayer = GameObject.Find("moviePlayManager").GetComponent<VideoPlayer>();
            //thisAudioSource = GameObject.Find("moviePlayManager").GetComponent<AudioSource>();
            //gameManager = GameObject.Find("Canvas").GetComponent<ThisGameManager>();
            //movieUrl[0] = Application.streamingAssetsPath + "/视频/0.mp4";
            isPlay = false;
        }

        /// <summary>
        /// play video
        /// </summary>
        void PlayMovie()
        {

            //如果videoPlayer没有对应的视频texture，则返回
            if (videoPlayer.texture == null)
            {

                return;
            }
            //render videoplayer.texture to UGUI's RawImage
            rawImage.texture = videoPlayer.texture;
            videoTimeSlider.value = (float)(videoPlayer.time / videoPlayer.clip.length);
            //videoTimeSlider.value = (float)(videoPlayer.time);
            if (isPlay)
            {

                if (videoPlayer.frame == (long)videoPlayer.frameCount)
                {

                    //gameManager.ToMovieEnd();
                }
            }
        }

        /// <summary>
        /// 播放视频0逻辑
        /// </summary>
        public void ToPlayThis0()
        {

            //videoPlayer.url = movieUrl[0];
            StartCoroutine(ToPlay0());
        }

        /// <summary>
        /// 播放视频0的协程
        /// </summary>
        /// <returns></returns>
        IEnumerator ToPlay0()
        {

            yield return new WaitForSeconds(0.1f);
            BeginPlay();
        }

        /// <summary>
        /// 重播
        /// </summary>
        public void ToReStartVideo()
        {

            videoPlayer.Play();
            isPlay = true;
        }

        /// <summary>
        /// 开始播放视频
        /// </summary>
        void BeginPlay()
        {

            if (!isPlay)
            {

                videoPlayer.Play();
                isPlay = true;
            }
        }

        /// <summary>
        /// 停止播放视频
        /// </summary>
        public void StopPlay()
        {

            if (isPlay)
            {

                videoPlayer.Stop();
                isPlay = false;
            }
        }

        /// <summary>
        /// 暂停播放视频逻辑
        /// </summary>
        public void PauseThis()
        {

            if (isPlay)
            {

                videoPlayer.Pause();
                isPlay = false;
                //bofangText.text = "播放";
                bofangButtonImage.sprite = thisSprite[1];
                //Debug.Log("123");
            }
            else
            {

                videoPlayer.Play();
                isPlay = true;
                //bofangText.text = "暂停";
                bofangButtonImage.sprite = thisSprite[0];
            }
        }


        public void ToAddVideo()
        {

            videoPlayer.time += 0.01f * videoPlayer.clip.length;
            //Debug.Log(videoPlayer.time.ToString());
        }
        public void ToDelVideo()
        {

            videoPlayer.time -= 0.01f * videoPlayer.clip.length;
            //Debug.Log(videoPlayer.time.ToString());
        }

        #endregion
    }
}