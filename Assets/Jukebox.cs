using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public float jb_MinPitch = 0.05f;
    public float jb_MaxPitch = 0.3f;

    public Countdown jb_MusicSwapTime = new(60f*5f);

    public AudioClip[] jb_music;
    public AudioSource jb_SS;

    private Camera jb_cam;
    // Start is called before the first frame update
    void Start()
    {
        jb_MusicSwapTime.Countdown_ForceSeconds(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (JB_AttachedToCam())
            return;

        if (jb_MusicSwapTime.CountdownReturn())
            JB_SwapMusic();
        if (jb_MusicSwapTime.CountdownReturnValue() < 1f)
            jb_SS.volume = jb_MusicSwapTime.CountdownReturnValue();
    }


	private bool JB_AttachedToCam()
	{
        if (jb_cam != null)
            return false;

        jb_cam = FindObjectOfType<Camera>();
        if(jb_cam != null)
        {
            transform.parent = jb_cam.transform;
            transform.localPosition = Vector3.zero;
        }
        return true;
	}

    [ContextMenu("Swap")]
	private void JB_SwapMusic()
	{
        jb_SS.volume = 1f;
        jb_MusicSwapTime.CountdownResetRandomized(0.3f);
        jb_SS.clip = jb_music[UnityEngine.Random.Range(0, jb_music.Length)];
		jb_SS.Play();
	}
}
