using MoviePlay;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// �̳� ��ק�ӿ�
/// </summary>
public class VideoSliderEvent : MonoBehaviour, IDragHandler, IEndDragHandler
{

    [SerializeField]
    public ToPlayVideo toPlayVideo;        // ��Ƶ���ŵĽű�

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {


    }

    /// <summary>
    /// �� Slider ��ӿ�ʼ��ק�¼�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {

        toPlayVideo.videoPlayer.Pause();
        SetVideoTimeValueChange();
    }

    /// <summary>
    /// ��ǰ�� Slider ����ֵת��Ϊ��ǰ����Ƶ����ʱ��
    /// </summary>
    private void SetVideoTimeValueChange()
    {

        toPlayVideo.videoPlayer.time = toPlayVideo.videoTimeSlider.value * toPlayVideo.videoPlayer.clip.length;
        //toPlayVideo.videoPlayer.time = toPlayVideo.videoTimeSlider.value;
    }

    /// <summary>
    /// �� Slider ��ӽ�����ק�¼�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {

        toPlayVideo.videoPlayer.Play();
    }
}