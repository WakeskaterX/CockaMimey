using UnityEngine;
using System.Collections;

public class Contacts : MonoBehaviour {

  public GameObject player;
  public string col_name;

  PlayerController player_script;
  Renderer obj_renderer;

	// Use this for initialization
	void Start () {
    player_script = player.GetComponent<PlayerController>();
    obj_renderer = GetComponent<Renderer>();
    obj_renderer.enabled = false;
	}

	// Update is called once per frame
	void Update () {

	}

  void OnTriggerEnter(Collider col) {
    Debug.Log("COLLISION"+col.gameObject.name);
    if (col.gameObject.name == "Character") {
      player_script.NotifyCollision(col_name);
    }
  }
}
