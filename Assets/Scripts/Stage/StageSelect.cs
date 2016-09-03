using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageSelect : MonoBehaviour {

	private Vector3 pos;
	public float cameraSpeed;
	public Image  playerImage;

	TextControl textControl;

	// Use this for initialization
	void Start () {
		textControl = GameObject.Find ("TextController").GetComponent<TextControl> ();
		//魔法やPlayerのレベルを取得する
		GameObject.Find ("LevelController").GetComponent<LoadLevel> ().LevelUpdate ();
		GameObject.Find ("LevelController").GetComponent<LoadLevel> ().StatusUpdate ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!textControl.isScenario) {
			if (Input.GetMouseButtonDown (0)) {
				int layerMask = (1 << 10);		//背景
				//タッチした点を取得する
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
					pos = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
				}
			}

			if (Input.GetMouseButton (0)) {
				int layerMask = (1 << 10);		//背景
				//新しい位置の座標を取得
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
					if ((hit.point - pos).sqrMagnitude > 4) {
						//正規化で指ぼ移動方向を取得する
						Vector3 touchNormalized = (hit.point - pos).normalized;
						if (touchNormalized.y < -0.5f ||touchNormalized.y > 0.5f) {
							if (this.transform.position.y <= 7.5 &&this.transform.position.y >= -7.5) {
								touchNormalized.x = 0;
								touchNormalized.z = 0;
								touchNormalized.y *= -1;
								this.GetComponent<Rigidbody> ().AddForce (touchNormalized * cameraSpeed);
								pos = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
							}
						} 
					}
				}
			}
			if (this.transform.position.y > 7.5) {
				this.transform.position = new Vector3 (this.transform.position.x, 7.5f,
					this.transform.position.z);
				StopMoving ();
			} else if (this.transform.position.y < -7.5) {
				this.transform.position = new Vector3 (this.transform.position.x, -7.5f,
					this.transform.position.z);
				StopMoving ();
			}
			this.GetComponent<Rigidbody> ().velocity *= 0.9f;
			this.GetComponent<Rigidbody> ().angularVelocity *= 0.9f; 

		}
	}
	/// <summary>
	/// シーンを変える
	/// </summary>
	/// <param name="num">Number.</param>
	public void SceneChange(int num){
        if (!textControl.isScenario)
        {
            LoadLevel.Instance.stageNum = num;
            SceneManager.LoadScene(2);
        }
    }
    public void Book(int num)
    {
        if (!textControl.isScenario)
            SceneManager.LoadScene(num);
    }
	/// <summary>
	/// Stops the camera moving.
	/// </summary>
	void StopMoving(){
		this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		this.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
	}
}
