using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{

    public VideoPlayer videoPlayer;

    public void PlayVideo()
    {
        //videoPlayer= video.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component is not assigned.");
            return;
        }

        // Ensure looping is disabled
        videoPlayer.isLooping = false;

        // Start playing the video
        videoPlayer.Play();

    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video has ended.");
        // Add any additional logic to handle the end of the video
    }
}

