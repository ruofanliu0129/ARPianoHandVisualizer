using MoviePlay;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 继承 拖拽接口
/// </summary>
public class VideoSliderEvent : MonoBehaviour, IDragHandler, IEndDragHandler
{

    [SerializeField]
    public ToPlayVideo toPlayVideo;        // 视频播放的脚本

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {


    }

    /// <summary>
    /// 给 Slider 添加开始拖拽事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {

        toPlayVideo.videoPlayer.Pause();
        SetVideoTimeValueChange();
    }

    /// <summary>
    /// 当前的 Slider 比例值转换为当前的视频播放时间
    /// </summary>
    private void SetVideoTimeValueChange()
    {

        toPlayVideo.videoPlayer.time = toPlayVideo.videoTimeSlider.value * toPlayVideo.videoPlayer.clip.length;
        //toPlayVideo.videoPlayer.time = toPlayVideo.videoTimeSlider.value;
    }

    /// <summary>
    /// 给 Slider 添加结束拖拽事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {

        toPlayVideo.videoPlayer.Play();
    }
}