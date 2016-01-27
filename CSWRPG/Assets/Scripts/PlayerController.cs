using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;

	private Animator anim;
	private Rigidbody2D myRigidbody;

	private bool playerMoving;
	private Vector2 lastMove;

	private static bool playerExists;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D> ();

		if (!playerExists) {
			playerExists = true;
			DontDestroyOnLoad (transform.gameObject);
		} else {
			Destroy(gameObject);
		}

	}

	public void setLastMove(Vector2 last){
		lastMove = last;
	}
	
	// Update is called once per frame
	void Update () {
		playerMoving = false;

		float horizInput = Input.GetAxisRaw ("Horizontal");
		float vertInput = Input.GetAxisRaw ("Vertical");

		if (horizInput != 0 || vertInput != 0) {
			playerMoving = true;
			lastMove = new Vector2(horizInput, vertInput);
		} else {
			playerMoving = false;
		}

		myRigidbody.velocity = new Vector2 (horizInput*moveSpeed,vertInput*moveSpeed);
		//transform.Translate(new Vector3(horizInput*translateConstant, vertInput*translateConstant, 0f));


		anim.SetFloat("MoveX", horizInput);
		anim.SetFloat ("MoveY", vertInput);
		anim.SetBool ("PlayerMoving", playerMoving);
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);
	}
}
