using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonsterStatus : MonoBehaviour {
	//モンスターのステータスを表す列挙体
	public enum _status{
		NONE = -1,
		MOVE,
		ATTACK,
		STAND,
		AIR,
		BREAK,
		DEAD
	}
	//monsterの状態
	public _status m_status {
		get;
		set;
	}
	public int m_Level;				//モンスターのレベル
	public float HP;				//モンスターの生命値
	public int power;				//モンスターの力
	public int defence;				//モンスターの防御力
	public float monsterSpeed;		//モンスターの移動の速さ
	public float attackSpeed;		//モンスターの攻撃速度
	public float monsterSize;			//モンスターの体型
	public float XZ_Blowing_Resist;	//XY方向に飛ばす力の対抗力
	public float Y_Blowing_Resist;	//Z方向に飛ばす力の対抗力
	public float BXZ_Resist	;		//Break中XY方向に飛ばす力の対抗力
	public float BY_Resist;			//Break中Z方向に飛ばす力の対抗力
	public float break_Blowing_Resist;	//めまいの対抗力
	public float break_Save = 0;			//めまいの蓄積
	public int maseki;				//モンスターの持っている魔石の数
	[HideInInspector]
	public IEnumerator routine;	//攻撃のコルーチン
	[HideInInspector]
	public Slider HP_Slider;		//HPゲージ
	//Player
	GameObject player;
	public TextAsset monsterDataFile;

	// Use this for initialization
	void Start () {
		m_status = _status.NONE;
		player = GameObject.Find ("Player");
		//HPのゲージを取得

		routine = MonsterAttack ();
		RotateToPlayer (1);
	}
	
	// Update is called once per frame
	void Update () {
		if (HP_Slider != null) {
			HP_Slider.value = HP;
		}
		//前に動く
		if (m_status == _status.MOVE && HP > 0)
			MoveToPlayer ();
		//死ぬ
		if (HP <= 0 && m_status != _status.DEAD) {
			MonsterDead ();
		}
	}

	//接触があったら
	void OnCollisionEnter(Collision other){
		switch (other.gameObject.tag) {
		case "Floor":
			if (m_status != _status.DEAD) {
				if (m_status == _status.AIR) {
					m_status = _status.STAND;
					this.GetComponentInChildren<Animator> ().SetTrigger ("Stand");
					RotateToPlayer (0.5f);
				}
			}
            else
                MonsterDead();
			break;
		}
	}
	//離れたら
	void OnCollisionExit(Collision other){
		switch (other.gameObject.tag) {
		case "Floor":
            if (m_status != _status.DEAD)
            {
                m_status = _status.AIR;
				StopCoroutine (routine);
                this.GetComponentInChildren<Animator>().SetTrigger("Blow");
            }
            else
                MonsterDead();
			break;
		}
	}

	//攻撃スピードのごとに攻撃する
	public IEnumerator MonsterAttack(){
		while (true) {
            if (m_status == _status.DEAD)
            {
                StopCoroutine(routine);
                MonsterDead();
                break;
            }
			this.GetComponentInChildren<Animator>().SetTrigger("Attack");
			yield return new WaitForSeconds (attackSpeed);
		}
	}
	/// <summary>
	/// Monster死ぬ
	/// </summary>
	public void MonsterDead(){
		StopCoroutine (routine);
		m_status = _status.DEAD;
		//アニメのイベントに転移
		this.GetComponentInChildren<Animator> ().SetTrigger ("isDead");
		if (LoadLevel.Instance != null)
			LoadLevel.Instance.maseki += this.maseki;
	}
	/// <summary>
	/// Playerの方向に向く
	/// </summary>
	public void RotateToPlayer(float speed){
		Vector3 m_pos = this.transform.position;
		Vector3 targetPoint = player.transform.position;
		targetPoint.x += 30;
		transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (targetPoint - m_pos), speed);
	}
	/// <summary>
	/// Playerの方向に移動する
	/// </summary>
	void MoveToPlayer(){
		this.transform.position += transform.forward * monsterSpeed * Time.deltaTime;
		RotateToPlayer (0.3f);
	}
	/*public void LoadMonsterData(){
		//テキストダータお、文字列として取り込む
		string level_texts = monsterDataFile.text;

		//改行ごとに分割し、文字列の配列に入れる
		string[] lines = level_texts.Split('\n');

		//lines内お各行に対して、順番に処理していくループ
		foreach (var line in lines) {
			if (line == "") {	//行が空っぽなら
				continue;		//以下の処理はせずいループの先頭にジャンプ
			}
			string[] words = line.Split ();
			if (words[0].StartsWith ("#") || words[0].StartsWith ("@")) {	//ワードの先頭文字が＃か@なら
				continue;						//ループを脱出
			}
			if (m_Level == int.Parse (words [0])) {//レベルが違うなら
				int n = 0;

				//words内の各ワードに対して、順番に処理していくループ
				foreach (var word in words) {

					if (word == "") {				//ワードが空っぽなら
						continue;					//ループの先頭にジャンプ
					}


					//「n」の値を0,1,2,...と変化させていく
					//各ワードを数値に変換し、それぞれ格納する
					switch (n) {
					case 1:
						HP = float.Parse (word);
						break;
					case 2:
						power = int.Parse (word);
						break;
					case 3:
						defence = int.Parse (word);
						break;
					case 4:
						monsterSpeed = float.Parse (word);
						break;
					case 5:
						attackSpeed = float.Parse (word);
						break;
					case 6: 
						XZ_Blowing_Resist = float.Parse (word);
						break;
					case 7: 
						Y_Blowing_Resist = float.Parse (word);
						break;
					case 8:
						break_Blowing_Resist = float.Parse (word);
						break;
					case 9:
						maseki = int.Parse (word);
						break;
					case 10:
						BXZ_Resist = float.Parse (word);
						break;
					case 11:
						BY_Resist = float.Parse (word);
						break;
					default:
						break;
					}
					n++;
				}
				break;
			}
		}
	}*/
	/// <summary>
	/// Load monster data.
	/// </summary>
	public void LoadMonsterData(){
		HP *= (1 + (10 * m_Level / 100));
		power *= (1 + (10 * m_Level / 100));
		defence *= (1 + (10 * m_Level / 100));
	}
}
