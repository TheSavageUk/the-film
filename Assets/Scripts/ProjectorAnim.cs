// ProjectorAnim.cs - Projector animation
// Copyright (c) 2015, Savage Software

// Includes

using UnityEngine;
using System.Collections;

// Projector class

public class ProjectorAnim:MonoBehaviour
{
	// Public attributes

	public string url=null;

	// States

	private const int S_IDLE=0;
	private const int S_LOADING=1;
	private const int S_STARTING=2;
	private const int S_RUNNING=3;
	private const int S_STOPING=4;

	// Timing

	private const float T_STARTING=1.0f;
	private const float T_STOPPING=2.0f;
	
	// Lighing

	private const float MAIN_BASE=8.0f;
	private const float MAIN_SCALE=1.8f;
	
	private const float GLOW_BASE=4.0f;
	private const float GLOW_SCALE=2.0f;
	
	private const float FLARE_BASE=0.45f;
	private const float FLARE_SCALE=0.1f;

	// Sounds

	private const int SOUND_STARTING=0;
	private const int SOUND_RUNNING=1;
	private const int SOUND_STOPING=2;
	private const int SOUND_AUDIO=3;
	
	// Local data

	private GameObject oMain=null;
	private GameObject oGlow=null;
	private GameObject oFlare1=null;
	private GameObject oFlare2=null;
	private GameObject oView=null;

	private Light main=null;
	private Light glow=null;

	private Renderer flare1=null;
	private Renderer flare2=null;

	private Material view=null;
	
	private AudioSource[] sounds;

	private int state=S_IDLE;
	private float time=0.0f;

	private WWW request=null;

	private MovieTexture movieVisual=null;
	private AudioClip movieAudio=null;
	
	// Methods

	// Start
	// Called when this object starts

	void Start()
	{
		// find lights
		oMain=GameObject.Find("Projector/Main");
		main=oMain.GetComponent<Light>();

		oGlow=GameObject.Find("Projector/Glow");
		glow=oGlow.GetComponent<Light>();

		// find flares
		oFlare1=GameObject.Find("Projector/Flare1");
		flare1=oFlare1.GetComponent<Renderer>();

		oFlare2=GameObject.Find("Projector/Flare2");
		flare2=oFlare2.GetComponent<Renderer>();

		// get screen view
		oView=GameObject.Find("Screen/View");
		view=oView.GetComponent<Renderer>().material;

		// get sounds
		sounds=gameObject.GetComponents<AudioSource>();

		// initially stopped
		Stopped();

		// load the movie
		Load(url);
	}

	// Update
	// Called to update on each frame

	void Update()
	{
		// what state?
		switch(state)
		{
			case S_IDLE:
				// nothing
				break;

			case S_LOADING:
				// loading movie
				if(request!=null &&
					request.movie.isReadyToPlay)
					Starting();
				break;

			case S_STARTING:
				// projector starting
				if(Time.time>=time+T_STARTING)
					Run();
				break;

			case S_RUNNING:
				// projector running
				Running();
				break;

			case S_STOPING:
				// projector stopping
				if(Time.time>=time+T_STOPPING)
					Stopped();
				break;
		}
	}

	// Local functions

	//
	//

	private void Load(string url)
	{
		// load up url
		request=new WWW(url);

		// loading it
		state=S_LOADING;
	}

	//
	//

	private void Starting()
	{
		// turn lights and flares on
		oMain.SetActive(true);
		oGlow.SetActive(true);

		oFlare1.SetActive(true);
		oFlare2.SetActive(true);

		// play audio
		sounds[SOUND_STARTING].Play();

		// start it
		state=S_STARTING;
		time=Time.time;
	}

	//
	//

	private void Run()
	{
		// setup from request
		movieVisual=request.movie;
		movieAudio=movieVisual.audioClip;

		view.mainTexture=movieVisual;
		sounds[SOUND_AUDIO].clip=movieAudio;

		oView.SetActive(true);

		movieVisual.Play();
		sounds[SOUND_AUDIO].Play();

		// play audio
		sounds[SOUND_RUNNING].Play();

		// run it
		state=S_RUNNING;
	}

	//
	//

	private void Running()
	{
		// set light intensity
		float r=Random.value;

		main.intensity=MAIN_BASE-(r*MAIN_SCALE);
		glow.intensity=GLOW_BASE+(r*GLOW_SCALE);

		// set flare transparency
		Color c=flare1.material.color;
		c.a=FLARE_BASE+(r*FLARE_SCALE);

		flare1.material.color=c;
		flare2.material.color=c;
	}

	//
	//

	private void Stop()
	{
		// stop movie
		movieVisual.Stop();
		sounds[SOUND_AUDIO].Stop();

		// remove view
		oView.SetActive(false);

		// play audio
		sounds[SOUND_RUNNING].Stop();
		sounds[SOUND_STOPING].Play();

		// stop it
		state=S_STOPING;
		time=Time.time;
	}

	//
	//

	private void Stopped()
	{
		// turn lights and flares on
		oMain.SetActive(false);
		oGlow.SetActive(false);
		
		oFlare1.SetActive(false);
		oFlare2.SetActive(false);

		// remove view
		oView.SetActive(false);

		// remove linkage
		view.mainTexture=null;
		sounds[SOUND_AUDIO].clip=null;

		movieVisual=null;
		movieAudio=null;

		request=null;

		// stopped it
		state=S_IDLE;
	}
}
