using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameControls : MonoBehaviour {

  public GameObject main_player;
  public GameObject alt_player;
  public GameObject female;
  public GameObject camera_obj;

  public GameObject contact_rig;
  public float max_action_time = 2F;
  public Text debug_text;

  //AUDIO FILES
  public AudioClip[] bird_noises;
  public AudioClip[] female_positive;
  public AudioClip[] female_negative;
  public AudioClip narrator_lose;
  public AudioClip narrator_win;
  public AudioClip narrator_start;

  public AudioClip[] narration_series;
  public float[] narration_wait_lengths;
  int narration_index = 0;

  public AudioClip ending_song;

  public int moves_to_win = 6;

  bool player_turn_active = false;
  bool player_ready = false;
  bool reset_ready = true;
  bool competitor_turn_active = false;
  bool waiting_for_player_entry = false;
  Competitor competitor_script;
  string current_event = "--";
  string desired_event = "NN";
  List<string> existing_chain;
  Queue<string> event_chain = new Queue<string>();
  float action_time = 0F;
  System.Random rand = new System.Random();

  AudioSource audio_source;
  AudioSource female_source;
  AudioSource camera_music;

	// Use this for initialization
	void Start () {
    competitor_script = alt_player.GetComponent<Competitor>();
    audio_source = GetComponent<AudioSource>();
    female_source = female.GetComponent<AudioSource>();
    camera_music = camera_obj.GetComponent<AudioSource>();
    ResetGame();
	}

	// Update is called once per frame
	void Update () {
    if (player_turn_active && player_ready) {
      float current_time = Time.time;
      if (current_time - action_time > max_action_time) {
        ProcessFailure();
      }
    }
	}

  /**
   * Public Functions
   */
  //Player Collision Event - accessed by the player script
  public void PlayerCollisionEvent (string player_event) {
    WriteDebug("Col Event: "+player_event);
    if (player_turn_active && player_ready && reset_ready) {
      WriteDebug("Active Collision Event: "+player_event);
      current_event = player_event;
      reset_ready = false;
      if (waiting_for_player_entry) {
        existing_chain.Add(player_event);
        ProcessChainSuccess();
      } else if (String.Equals(current_event, desired_event)) {
        ProcessSuccess();
      }
    }
  }

  //Notify of a Reset Collision
  public void NotifyCollision(string name) {
    if (String.Equals(name, "RESET")) {
      //Player Position is Reset and we can now have the players turn
      if (player_turn_active) {
        if (!player_ready) {
          //First Time Reset Timer
          player_ready = true;
          WriteDebug("First Reset, Turn Started");
          StartTimer();
        } else {
          WriteDebug("Player Is Still Ready... right?");
        }
        reset_ready = true;
        WriteDebug("Reset Ready");
      }
    }
  }

  //Notify of Exiting a Reset
  public void NotifyCollisionExit() {
    if (player_turn_active && player_ready) {
      WriteDebug("Collision Exit Notify!");
      reset_ready = true;
    }
  }

  //Player & Competitor Turns
  public void StartPlayerTurn (string[] events_to_do) {
    female.transform.rotation = Quaternion.Euler(0, 0, 0);
    WriteDebug("Player Turn Started");
    player_turn_active = true;
    waiting_for_player_entry = false;
    existing_chain = new List<string>(events_to_do);
    event_chain = new Queue<string>(existing_chain);
    try {
      desired_event = event_chain.Dequeue();
    } catch (InvalidOperationException ioe) {
      desired_event = "-";
      waiting_for_player_entry = true;
    }
    StartTimer();
  }

  public void StartCompetitorTurn () {
    female.transform.rotation = Quaternion.Euler(0, -80, 0);
    competitor_turn_active = true;
    competitor_script.StartCompetitorsTurn(existing_chain.ToArray());
  }

  public void CompetitorTurnDone (string[] new_event_chain) {
    competitor_turn_active = false;
    WriteDebug("New Event Chain: "+String.Join(",",new_event_chain));
    StartPlayerTurn(new_event_chain);
  }

  //Process Successful Match - Reset Match Timer
  //Move to next in the array of match IDs
  void ProcessSuccess() {
    StartTimer();
    PlayRandomBird();
    try {
      current_event = "-";
      desired_event = event_chain.Dequeue();
      WriteDebug("SUCCESS!");
    } catch (InvalidOperationException ioe) {
      //Caught the last item
      desired_event = "N";
      WriteDebug("FINAL MOVE");
      waiting_for_player_entry = true;
    }
  }

  void WriteDebug(string txt) {
    Debug.Log(txt + " -- player_turn_active: "+player_turn_active+", player_ready: "+player_ready+ ", reset_ready: "+reset_ready);
    //debug_text.GetComponent<Text>().text = txt;
  }

  void TurnPlayerOff() {
    reset_ready = false;
    player_turn_active = false;
    player_ready = false;
    waiting_for_player_entry = false;
  }

  //Won the level
  void ProcessChainSuccess() {
    if (existing_chain.Count < moves_to_win) {
      WriteDebug("Won Turn!");
      PlayRandomMatePositive();
      TurnPlayerOff();
      Invoke("StartCompetitorTurn", 2f);
    } else {
      WriteDebug("Won Game!");
      PlayRandomMatePositive();
      TurnPlayerOff();
      camera_music.Stop();
      audio_source.clip = narrator_win;
      audio_source.Play();
      Invoke("EndingSong", 11f);
    }
  }

  //Fail the User
  void ProcessFailure() {
    PlayRandomMateNegative();
    WriteDebug("You Lose");
    audio_source.clip = narrator_lose;
    audio_source.PlayDelayed(2f);
    TurnPlayerOff();
    Invoke("ResetGame", 20f);
  }

  void ResetGame() {
    existing_chain = new List<string>();
    narration_index = 0;
    camera_music.Play();
    Invoke("IntroNarration", 4f);
  }

  void EndingSong() {
    audio_source.clip = ending_song;
    audio_source.Play();
    Invoke("ResetGame", 50f);
  }

  void IntroNarration() {
    if (narration_index >= narration_series.Length) {
      Invoke("StartCompetitorTurn", 6f);
    } else {
      audio_source.clip = narration_series[narration_index];
      audio_source.Play();
      float wait_time = narration_wait_lengths[narration_index];
      narration_index++;
      Invoke("IntroNarration", wait_time);
    }
  }

  //Update Timer
  void StartTimer() {
    action_time = Time.time;
  }

  //SOUND Commands
  void PlayRandomBird() {
    var a = rand.Next(bird_noises.Length);
    audio_source.clip = bird_noises[a];
    audio_source.Play();
  }

  void PlayRandomMatePositive() {
    var a = rand.Next(female_positive.Length);
    female_source.clip = female_positive[a];
    female_source.Play();
  }

  void PlayRandomMateNegative() {
    var a = rand.Next(female_negative.Length);
    female_source.clip = female_negative[a];
    female_source.Play();
  }
}
