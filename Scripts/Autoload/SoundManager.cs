using Godot;
using System;

public partial class SoundManager : Node
{
	public static SoundManager Instance;
	Random rand = new Random();

	AudioStreamPlayer musicPlayer;
	AudioStreamPlayer soundPlayer;

	public override void _Ready()
	{
		Instance = this;
		musicPlayer = (AudioStreamPlayer)this.GetChild(0);
		soundPlayer = (AudioStreamPlayer)this.GetChild(1);
	}

	public void PlayMusic(AudioStreamWav track){
		musicPlayer.Stop();
		musicPlayer.Stream = track;
		musicPlayer.Play();
	}

	// used for menu sounds
	public void PlaySound(AudioStreamWav sound){
		soundPlayer.Stream = sound;
		soundPlayer.Play();
	}

	// this method adds the audioshot as a child to the node
	public void PlaySoundOnNode(AudioStreamWav sound, Node2D node, float db){ // optional volume adjustment with (db) parameter
		// add new custom AudioStreamPlayer2D to node,
		//AudioShot newAudioShot = (AudioShot)Database.Instance.audioShot.Instantiate();
		//newAudioShot.AddToGroup("AudioShots");
		//node.AddChild(newAudioShot);
		
		// then play sound from that node,
		//newAudioShot.VolumeDb = db;
		//newAudioShot.Stream = sound;
		//newAudioShot.Play();
		// the Audioshot script should then queueFree the node when audio is finished playing
	}

	// this method creates an audioshot at the node's location as a child of the scene
	public void PlaySoundAtNode(AudioStreamWav sound, Node2D node, float db){ // optional volume adjustment with (db) parameter
		// add new custom AudioStreamPlayer2D to node,
		//AudioShot newAudioShot = (AudioShot)Database.Instance.audioShot.Instantiate();
		//newAudioShot.AddToGroup("AudioShots");
		//GetNode<Node2D>("/root/Scene").AddChild(newAudioShot);
		//newAudioShot.Position = node.Position;
		// then play sound from that node,
		//newAudioShot.VolumeDb = db;
		//newAudioShot.Stream = sound;
		//newAudioShot.Play();
		// the Audioshot script should then queueFree the node when audio is finished playing
	}

	public void CutSoundsOnNode(Node2D node){
		// get all sound source children of player and remove them
		var currentSounds = GetTree().GetNodesInGroup("AudioShots");
		foreach (var sound in currentSounds){
			if (sound.GetParent() == node){
				sound.QueueFree();
			}
		}
	}
}
