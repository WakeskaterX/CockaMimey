using UnityEngine;
using System.Collections;

public class Contacts : MonoBehaviour {

  public GameObject player;
  public string col_name;

  PlayerController player_script;

	// Use this for initialization
	void Start () {
    player_script = player.GetComponent<PlayerController>();
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
