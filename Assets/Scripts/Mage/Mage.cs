using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]

public class Mage : MonoBehaviour {

	public int level;			//魔法のレベル
	public int cost;			//消費MP
	public float castTime;		//呪文を唱える時間
	public float finishTime;	//魔法の終わる時間
	private bool onEffect = false;	//魔法が作用中かどうか
	public int power;				//魔法の攻撃力
	public float XZ_Blowing;		//XZ方向に飛ばす力
	public float Y_Blowing;			//Y方向に飛ばす力
	public float breakPower;		//モンスターの動きを止める力
	public string name = "";
	private float timer = 0;	
	
	private CircelControl circelControl;	//circelControlを格納する変数
	private Collider mahouCollider;

	// Use this for initialization
	void Start () {
		if(this.GetComponent<AudioSource>().isPlaying)
			this.GetComponent<AudioSource>().Stop ();
		circelControl = GameObject.Find ("GameRoot").GetComponent<CircelControl> ();
		mahouCollider = this.GetComponentInChildren<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
			timer += Time.deltaTime;
		if (timer >= castTime && onEffect == false && timer < finishTime) {
			mahouCollider.enabled = true;
			onEffect = true;
			if (!this.GetComponent<AudioSource> ().isPlaying)
				this.GetComponent<AudioSource> ().Play ();
		} else if (timer >= finishTime && onEffect == true)
			mahouCollider.enabled = false;
		}
	/// <summary>
	/// 魔法の効果が終わったら、魔法を削除する
	/// </summary>
	public void remove(){
		circelControl.RemoveCirstal (this.transform.root.gameObject);
	}
	/// <summary>
	/// 敵の位置による移動する
	/// </summary>
	/// <returns>The position.</returns>
	/// <param name="monster">Monster.</param>
	public IEnumerator movePosition(GameObject monster){
		while (true) {
			if (this.gameObject == null)
				break;
			this.transform.position = new Vector3 (monster.transform.position.x, this.transform.position.y, monster.transform.position.z);
			yield return null;
		}
	}
    void OnDestroy()
    {
		StopAllCoroutines ();
        if (name != "")
            getBtn(name).GetComponent<CoolDown>().isCoolDown = true;
    }
    GameObject getBtn(string name)
    {
        GameObject btn = GameObject.Find(name);
        return btn;
    }
}
