using UnityEngine;
using System.Collections;

public class UnityChanController : MonoBehaviour 
{

	private CharacterController controller = null;
	private float moveSpeed = 3.0f;   // 移動速度
	private float jumpSpeed = 10.0f;  // ジャンプ時の上方向への速度
	private float rotateSpeed = 3.0f; // 旋回速度
	private Vector3 moveDirection = Vector3.zero;
	private Animator animator = null; // Animator

	// Animatorの移動とジャンプステートを取得
	private int moveState = Animator.StringToHash("Base Layer.Move");

	void Awake()
	{
		controller = GetComponent<CharacterController> ();
		animator = GetComponent<Animator> ();
	}

	void Update () 
	{
		if (controller == null) {
			return;
		}

		float prevY = moveDirection.y;
		// キーボードからの入力を取得
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis ("Vertical");

		// 現在のAnimatorのステート値を取得
		AnimatorStateInfo currentBaseState = animator.GetCurrentAnimatorStateInfo(0);

		// 旋回
		transform.eulerAngles = new Vector3 (
			transform.eulerAngles.x,
			transform.eulerAngles.y + h * rotateSpeed,
			transform.eulerAngles.z
		);
		moveDirection = new Vector3 (h, 0, v);
		// キャラクターのローカル空間での方向に変換
		moveDirection = transform.TransformDirection(moveDirection);
		// 移動速度をかける
		moveDirection *= moveSpeed;
		// 地面に設置しているかつAnimatorのステートがMove状態（ジャンプ中にジャンプはできない）
		if (controller.isGrounded && currentBaseState.fullPathHash == moveState) {
			// 状態遷移中でないか確認する
			if (!animator.IsInTransition (0)) {
				// ボタンが押されたらジャンプ
				if (Input.GetButton ("Jump")) {
					// 上方向にジャンプする
					moveDirection.y = jumpSpeed;
					animator.SetBool ("Jump", true);
				}
			}
		} else {
			// ジャンプパラメータクリア
			animator.SetBool("Jump", false);
		}
		// 重力計算をする
		moveDirection.y += prevY;
		moveDirection.y += Physics.gravity.y * Time.deltaTime;
		// 移動処理
		controller.Move(moveDirection * Time.deltaTime);
		// 入力値の値を渡す
		animator.SetFloat("Speed", Mathf.Abs(v));
	}
}
