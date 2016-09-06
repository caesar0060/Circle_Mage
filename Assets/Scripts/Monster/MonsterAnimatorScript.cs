using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class MonsterAnimatorScript : MonoBehaviour {

	private PlayerStatus playerStatus;
	private MonsterStatus monsterStatus;
	//モンスターが持っている武器
	public GameObject waepon;
    private NaviControl naviControl;

	// Use this for initialization
	void Start () {
		playerStatus = GameObject.Find ("Player").GetComponent<PlayerStatus> ();
		monsterStatus = this.transform.parent.gameObject.GetComponent<MonsterStatus> ();
        naviControl = GameObject.Find("GameRoot").GetComponent<NaviControl>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayerDamage(){
        if(!playerStatus.isWin)
		    playerStatus.p_HP -= monsterStatus.power;
	}

	/// <summary>
	/// クリア
	/// </summary>
	public void StageClear(){
        if (this.tag == "Boss")
        {
			Debug.Log ("win");
			playerStatus.isWin = true;
            GameObject.FindGameObjectWithTag("ClearImage").GetComponent<Image>().enabled = true;
            naviControl.ShowNavi("ゲームクリア", 3, "");
            StartCoroutine(MoveScene(1));
        }
        else
            DestroyMonster();
	}
	/// <summary>
	/// Moves the scene.
	/// </summary>
	/// <returns>The scene.</returns>
	/// <param name="num">Number.</param>
	public IEnumerator MoveScene(int num){
		Debug.Log ("123");
		yield return new WaitForSeconds (3);
		SceneManager.LoadScene (num);
	}
	
	/// <summary>
	/// Destroies the monster.
	/// </summary>
	public void DestroyMonster(){
		Destroy(this.transform.parent.gameObject);
	}
	/// <summary>
	/// Move animation.
	/// </summary>
	public void Move(){
		if (monsterStatus.m_status != MonsterStatus._status.DEAD &&
			monsterStatus.m_status != MonsterStatus._status.AIR) {
			monsterStatus.m_status = MonsterStatus._status.MOVE;
			this.GetComponent<Animator> ().SetTrigger ("Walk");
		}
	}
	//目的地に到着したら、攻撃開始
	void OnTriggerStay(Collider other){
		switch (other.tag) {
		case "MonsterAttack":
			if (monsterStatus.m_status == MonsterStatus._status.MOVE) {
				StartCoroutine (monsterStatus.routine);
				monsterStatus.m_status = MonsterStatus._status.ATTACK;
			}
			break;
		}
	}
	void OnTriggerExit(Collider other){
		switch (other.tag) {
		case "MonsterAttack":
			StopCoroutine (monsterStatus.routine);
			break;
		}
	}
	/// <summary>
	/// 矢を撃つ
	/// </summary>
	public void ShootArrow(){
		GameObject clone = Instantiate (waepon, this.transform.position, Quaternion.identity) as GameObject;
		clone.transform.SetParent (this.transform);
	}
}
