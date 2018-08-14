using UnityEngine.Audio;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour 
{

	// Use this for initialization

	public Sound[] sounds;

	public static SoundManager instance = null;
	
	
	void OnEnable()
	{
		Messenger<int>.AddListener("PlaySound", PlaySound );
		Messenger<int>.AddListener("RandomizePitch", RandomizePitch );
		Messenger<int>.AddListener("StopSound", PlaySound );
	}
	
	void OnDisable()
	{
		Messenger<int>.RemoveListener("PlaySound", PlaySound );
		Messenger<int>.RemoveListener("RandomizePitch", RandomizePitch );
		Messenger<int>.RemoveListener("StopSound", PlaySound );
	}
	void Awake () 
	{
		
		//Singleton
		if( instance == null )
			instance = this;
		else if( instance != this )
			Destroy( gameObject );

		
		
		DontDestroyOnLoad( gameObject );
		
		//Add audio source for each sound in the array
		foreach( Sound s in sounds )
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			s.source.playOnAwake = s.loop;
			s.source.mute = s.mute;
		}

	}
	

	public Sound FindSound( int id )
	{
		Sound s = Array.Find( sounds , sound => sound.id == id );
		
		return s;
	}

	public void PlaySound( int id )
	{

	
		Sound s = FindSound( id );

		if( s == null )
		{
			Debug.Log( "Sound not found..." );
		}
		else
		{
			s.source.Play();
		}	
	}

	public void RandomizePitch( int id )
	{
		Sound s = FindSound( id );

		if( s == null )
		{
			Debug.Log( "Sound Not Found..." );
		}
		else
		{
			s.source.pitch = ( UnityEngine.Random.Range(0.5f, .9f) );
			s.source.Play();
		}
	}

	public void StopSound( int id )
	{
		// Sound s = Array.Find( sounds , sound => sound.id == id );

		// if( s == null )
		// 	Debug.Log( "Sound not found..." );

		// s.source.Stop();
	}


	public void PlayMusic( int musicIndex )
	{

	}


	public void StopMusic( int musicIndex )
	{
		
	}

}
