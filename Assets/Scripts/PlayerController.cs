using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public GameObject game_controller;

  public Text debug_log;

  string log_value;



	// Use this for initialization
	void Start () {
    log_value = "Testing123";
    debug_log.GetComponent<Text>().text = log_value;
	}

	// Update is called once per frame
	void Update () {

	}

  // Emit Events
  void EmitToGameController(string Event) {
    //TODO: Make this access the Game Controller
    // For now just log to screen
    log_value = "EVENT "+Event+" Emitted";
    debug_log.GetComponent<Text>().text = log_value;
    Debug.Log("Hit "+Event);
  }

  public void NotifyCollision(string object_name) {
    EmitToGameController(object_name);
  }
}
