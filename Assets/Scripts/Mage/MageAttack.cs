using UnityEngine;
using System.Collections;

public class MageAttack : MonoBehaviour {

	[HideInInspector]
	public int hitNum;				//攻撃の当たり判定数
	private MageControl mageControl;
	private Mage mage ;
	[HideInInspector]
	public int range;
	private PlayerStatus playerStatus;

	// Use this for initialization
	void Start () {
		mageControl = GameObject.Find ("GameRoot").GetComponent<MageControl> ();
		playerStatus = GameObject.Find ("Player").GetComponent<PlayerStatus> ();
		GameObject obj = gameObject.transform.root.gameObject;
		mage = obj.GetComponentInChildren<Mage> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	/// <summary>
	/// モンスターに当たったら
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other){
		int damage = (int)((playerStatus.powerRate/100) * mage.power) + mage.power;
		if(other.tag == "Monster"){
			MonsterStatus monsterStatus = other.gameObject.GetComponent<MonsterStatus>();
			if (monsterStatus.m_status == MonsterStatus._status.AIR) {
				ShowCriticalText (other.gameObject);
				damage += (int)(damage * (playerStatus.CriticalRate / 100));
			}
			monsterStatus.HP -= damage;
			if (monsterStatus.HP > 0) {
				BreakMonster (other.gameObject);
				BlowOffMonster (this.gameObject, other.gameObject);
			}
			ShowDamageText(other.gameObject, damage.ToString());
			
		}
	}
		

	/// <summary>
	/// 魔法のダメージを表す
	/// </summary>
	/// <param name="monster">Monster.</param>
	void ShowDamageText(GameObject monster, string damage){
		GameObject textClone = Instantiate(mageControl.damageText, new Vector3(monster.transform.position.x,
		                                                                       monster.transform.position.y +2,
		                                                                       monster.transform.position.z), Quaternion.Euler(0,270,0)) as GameObject;
		textClone.GetComponentInChildren<TextMesh>().text = damage;
		iTween.MoveBy (textClone, iTween.Hash ("y", textClone.transform.position.y + 2,
		                                       "islocal", true,
			                                     "easeTupe", "easeInOutExpo",
			                                     "time", 1,
			                                     "oncomplete", "RemoveDamageText",
			                                     "oncompletetarget", GameObject.Find("GameRoot"),
			                                     "oncompleteparams", textClone));
	}
	/// <summary>
	///クリティカルのテキストを表す
	/// </summary>
	/// <param name="monster">Monster.</param>
	void ShowCriticalText(GameObject monster){
		GameObject textClone = Instantiate(mageControl.criticalText, new Vector3(monster.transform.position.x,
			monster.transform.position.y +2.5f,
			monster.transform.position.z), Quaternion.Euler(0,270,0)) as GameObject;
		iTween.MoveBy (textClone, iTween.Hash ("y", textClone.transform.position.y + 2,
			"islocal", true,
			"easeTupe", "easeInOutExpo",
			"time", 1,
			"oncomplete", "RemoveDamageText",
			"oncompletetarget", GameObject.Find("GameRoot"),
			"oncompleteparams", textClone));
	}
	/// <summary>
	///Berakのテキストを表す
	/// </summary>
	/// <param name="monster">Monster.</param>
	void ShowBreakText(GameObject monster){
		GameObject textClone = Instantiate(mageControl.breakText, new Vector3(monster.transform.position.x,
			monster.transform.position.y +2.5f,
			monster.transform.position.z), Quaternion.Euler(0,270,0)) as GameObject;
		iTween.MoveBy (textClone, iTween.Hash ("y", textClone.transform.position.y + 2,
			"islocal", true,
			"easeTupe", "easeInOutExpo",
			"time", 1,
			"oncomplete", "RemoveDamageText",
			"oncompletetarget", GameObject.Find("GameRoot"),
			"oncompleteparams", textClone));
	}
	/// <summary>
	/// モンスターを飛ばす
	/// </summary>
	/// <param name="mageObject">Mage object.</param>
	/// <param name="monster">Monster.</param>
	void BlowOffMonster(GameObject mageObject, GameObject monster){
		MonsterStatus ms = monster.GetComponent<MonsterStatus> ();
		if (ms.m_status != MonsterStatus._status.DEAD) {
			float rivise = 1;
			float mYResist = ms.Y_Blowing_Resist;
			float mXZResist = ms.XZ_Blowing_Resist;
			switch(monster.GetComponent<MonsterStatus> ().m_status){
			//ブレック中のモンスターの抵抗力を半分になる
			case MonsterStatus._status.BREAK:
				mYResist = ms.BY_Resist;
				mXZResist = ms.BXZ_Resist;
				break;
			//モンスターは空中の時、飛ばす力を半分になる
			case MonsterStatus._status.AIR:
				rivise = 0.5f;
				break;
			}
			Vector3 posNormaliz = (monster.transform.position - mageObject.transform.position).normalized;
			//魔法の飛ばす力引くモンスターの抵抗力
			float y = (mage.Y_Blowing - mYResist) * rivise;
			if (y > 0)
				posNormaliz.y = y;
			else
				posNormaliz.y = 0;
			float xz = (mage.XZ_Blowing - mXZResist) * rivise;
			if (xz < 0)
				xz = 0;
			posNormaliz.x *= xz;
			posNormaliz.z *= xz;
			monster.GetComponent<Rigidbody> ().AddForce (posNormaliz);
		}
	}
    void BreakMonster(GameObject monster)
    {
		MonsterStatus ms = monster.GetComponent<MonsterStatus> ();
        if (ms.m_status != MonsterStatus._status.DEAD)
        {
            if (ms.m_status == MonsterStatus._status.MOVE ||
                ms.m_status == MonsterStatus._status.ATTACK)
            {
				ms.break_Save += mage.breakPower;
				if (ms.break_Save >= ms.break_Blowing_Resist)
                {
                    ms.m_status = MonsterStatus._status.BREAK;
					StopCoroutine (monster.GetComponent<MonsterStatus> ().routine);
					ShowBreakText (monster);
                    monster.GetComponentInChildren<Animator>().SetTrigger("Break");
					ms.break_Save = 0;
                }
            }
        }
    }
}
