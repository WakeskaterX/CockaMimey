using UnityEngine;
using System.Collections;

public class FemaleMovement : MonoBehaviour {

  public float y_rot = -80f;
  public float y_rot_main = 0f;
  public float lerp_time = 2f;
  public GameObject particle_obj;
  bool moving = false;
  float last_y = 0f;
  float dest_y = 0f;
  float lerp_amount = 0f;
  ParticleSystem ps;

	// Use this for initialization
	void Start () {
    ps = particle_obj.GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update () {
    lerp_amount += Time.deltaTime * lerp_time;
    if (moving) {
      float update_y = Mathf.LerpAngle(last_y, dest_y, lerp_amount);
      transform.rotation = Quaternion.Euler(new Vector3(0, update_y, 0));
    }
	}

  void StartMoving(float last, float dest) {
    lerp_amount = 0;
    last_y = last;
    dest_y = dest;
    moving = true;
    Invoke("StopMoving", lerp_time * 2f);
  }

  void StopMoving() {
    moving = false;
  }

  public void TurnToCompetitor() {
    float start_y = transform.rotation.eulerAngles.y;
    StartMoving(start_y, y_rot);
  }

  public void TurnToPlayer() {
    float start_y = transform.rotation.eulerAngles.y;
    StartMoving(start_y, y_rot_main);
  }

  public void PlayParticles() {
    ps.Play();
  }
}
