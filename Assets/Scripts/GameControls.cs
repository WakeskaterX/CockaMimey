using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameControls : MonoBehaviour {

  public GameObject main_player;
  public GameObject alt_player;
  public GameObject potential_mate;
  public float max_action_time = 2F;


  bool player_turn_active = false;
  string current_event = "--";
  string desired_event = "NN";
  Queue<string> event_chain = new Queue<string>();
  float action_time = 0F;



	// Use this for initialization
	void Start () {
    StartTimer();
    player_turn_active = true;
    string[] starting = {"LR","LL","LU","LD"};
    StartPlayerTurn(starting);
	}

	// Update is called once per frame
	void Update () {
    if (player_turn_active) {
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
    current_event = player_event;
    if (player_turn_active && String.Equals(current_event, desired_event)) {
      ProcessSuccess();
    }
  }

  public void StartPlayerTurn (string[] events_to_do) {
    player_turn_active = true;
    event_chain = new Queue<string>(events_to_do);
    desired_event = event_chain.Dequeue();
    StartTimer();
  }

  //Process Successful Match - Reset Match Timer
  //Move to next in the array of match IDs
  void ProcessSuccess() {
    try {
      current_event = "-";
      desired_event = event_chain.Dequeue();
      StartTimer();
    } catch (InvalidOperationException ioe) {
      //Caught the last item
      ProcessChainSuccess();
    }

  }

  //Won the level
  void ProcessChainSuccess() {
    Debug.Log("You Win");
    player_turn_active = false;
  }

  //Fail the User
  void ProcessFailure() {
    player_turn_active = false;
    Debug.Log("You Lose");
  }

  void StartTimer() {
    action_time = Time.time;
  }
}
