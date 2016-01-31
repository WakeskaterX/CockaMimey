using UnityEngine;
using System.Collections;

public class Contact_Reset : MonoBehaviour {

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
      game_cont.NotifyCollision(col_name);
    }
  }
}

