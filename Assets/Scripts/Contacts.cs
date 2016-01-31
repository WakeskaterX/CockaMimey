using UnityEngine;
using System.Collections;

public class Contacts : MonoBehaviour {

  public GameObject game_controller;
  public string col_name;

  GameControls game_cont;
  Renderer obj_renderer;

	// Use this for initialization
	void Start () {
    game_cont = game_controller.GetComponent<GameControls>();
    obj_renderer = GetComponent<Renderer>();
    obj_renderer.enabled = false;
	}

	// Update is called once per frame
	void Update () {

	}

  void OnTriggerEnter(Collider col) {
    if (col.gameObject.name == "Character") {
      //Debug.Log("Collision with Character: "+col_name+" with "+col.gameObject.name);
      game_cont.PlayerCollisionEvent(col_name);
    }
  }

  void OnTriggerExit(Collider col) {
    if (col_name == "TR" || col_name == "TL") {
      Debug.Log("COLLISION EXIT:"+col_name);
      game_cont.NotifyCollisionExit();
    }
  }
}
