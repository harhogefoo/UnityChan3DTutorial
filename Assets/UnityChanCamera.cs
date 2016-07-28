using UnityEngine;
using System.Collections;

public class UnityChanCamera : MonoBehaviour 
{
	public float smooth = 3f;  // カメラモーションのスムーズ化用変数
	Transform targetTransform;

	void Start () 
	{
		// CameraTargetは空のオブジェクト
		targetTransform = GameObject.Find ("CameraTarget").transform;

		transform.position = targetTransform.position;
		transform.forward = targetTransform.forward;
	}

	void FixedUpdate () 
	{
		// ターゲットの位置に合わせてカメラの位置を更新する
		transform.position = Vector3.Lerp (
			transform.position,
			targetTransform.position,
			Time.fixedDeltaTime * smooth
		);
		transform.forward = Vector3.Lerp (
			transform.forward,
			targetTransform.forward,
			Time.fixedDeltaTime * smooth
		);
	}
}
