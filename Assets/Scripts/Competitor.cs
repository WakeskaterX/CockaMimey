using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Competitor : MonoBehaviour {

  public GameObject head;
  public GameObject upper_neck;
  public GameObject game_controls;
  bool bird_active = false;     //Is this the competitors Turn?
  bool moving = false;          //Is the bird taking an action and moving?
  bool moving_back = false;
  public float lerp_time = 2f;
  public float move_time = 1f;
  public float lerp_angle = 40;

  static System.Random rand =  new System.Random();

  string current_action = "-";

  //Private Variables
  Queue<string> actions_to_take;
  List<string> actions_to_send_back;

  Vector3 start_head_rot;
  GameControls game_cont;
  float rot_x, rot_y, rot_z;
  float lerp_amount = 0f;

	// Use this for initialization
  void Awake() {
    start_head_rot = head.transform.rotation.eulerAngles;
  }

  void Start() {
    start_head_rot = head.transform.rotation.eulerAngles;
    game_cont = game_controls.GetComponent<GameControls>();
  }

	// Update is called once per frame
	void Update () {
    if (bird_active) {
      lerp_amount += Time.deltaTime * lerp_time;
      GameObject object_to_rot;
      Vector3 init_vec = new Vector3();
      Vector3 dest_vec = new Vector3();
      object_to_rot = head;

      //Moving & Moving Back Logic
      if (moving) {
        init_vec = start_head_rot;
        dest_vec = new Vector3(rot_x, rot_y, rot_z);
      } else if (moving_back) {
        init_vec = new Vector3(rot_x, rot_y, rot_z);
        dest_vec = start_head_rot;
      }
      UpdateRotation(init_vec, dest_vec, object_to_rot);
    }
	}

  void UpdateRotation (Vector3 initial_rotation, Vector3 destination_rotation, GameObject object_to_rotate) {
    var update_x = Mathf.LerpAngle(initial_rotation.x, destination_rotation.x, lerp_amount);
    var update_y = Mathf.LerpAngle(initial_rotation.y, destination_rotation.y, lerp_amount);
    var update_z = Mathf.LerpAngle(initial_rotation.z, destination_rotation.z, lerp_amount);
    object_to_rotate.transform.rotation = Quaternion.Euler(new Vector3(update_x, update_y, update_z));
  }

  public void StartCompetitorsTurn(string[] actions) {
    //Add Final Action
    var new_action = GenerateAction();
    actions_to_send_back = new List<string>(actions);
    actions_to_send_back.Add(new_action);
    actions_to_take = new Queue<string>(actions_to_send_back);
    bird_active = true;
    MakeNextAction();
  }

  void MakeNextAction () {
    try {
      current_action = actions_to_take.Dequeue();
      TakeAction(current_action);
    } catch (InvalidOperationException ioe) {
      //Turn Over
      moving = false;
      moving_back = false;
      bird_active = false;
      Debug.Log("Finished");
      game_cont.CompetitorTurnDone(actions_to_send_back.ToArray());
      //TODO: Emit back to game controller
    }
  }

  void TakeAction(string action) {
    //Move the head according to the
    moving = true;
    moving_back = false;
    lerp_amount = 0f;
    switch (action) {
      case "LR":
        rot_y = start_head_rot.y + lerp_angle;
        rot_x = start_head_rot.x;
        rot_z = start_head_rot.z;
        break;
      case "LL":
        rot_y = start_head_rot.y - lerp_angle;
        rot_x = start_head_rot.x;
        rot_z = start_head_rot.z;
        break;
      case "LU":
        rot_y = start_head_rot.y;
        rot_x = start_head_rot.x + (lerp_angle * .9f);
        rot_z = start_head_rot.z;
        break;
      case "LD":
        rot_y = start_head_rot.y;
        rot_x = start_head_rot.x - (lerp_angle * .9f);
        rot_z = start_head_rot.z;
        break;
      case "TR":
        rot_y = start_head_rot.y + (lerp_angle * 1.8f);
        rot_x = start_head_rot.x - (lerp_angle * .9f);
        rot_z = start_head_rot.z + (lerp_angle * 1.8f);
        break;
      case "TL":
        rot_y = start_head_rot.y - (lerp_angle * 1.8f);
        rot_x = start_head_rot.x - (lerp_angle * .9f);
        rot_z = start_head_rot.z - (lerp_angle * 1.8f);
        break;
    }
    Invoke("MoveBack", move_time * .75f);
  }

  void MoveBack() {
    Debug.Log("Moving Back");
    moving = false;
    moving_back = true;
    lerp_amount = 0f;
    Invoke("StopMoving", move_time);
  }

  void StopMoving() {
    Debug.Log("Moving Stopped");
    moving = false;
    moving_back = false;
    //Set the Rotation back to the original rotation
    head.transform.rotation = Quaternion.Euler(start_head_rot);
    MakeNextAction();
  }

  string GenerateAction() {
    string[] action_strings = new string[] {"LR", "LL", "LU", "LD", "TR", "TL"};
    int r = rand.Next(action_strings.Length);
    return action_strings[r];
  }


}
