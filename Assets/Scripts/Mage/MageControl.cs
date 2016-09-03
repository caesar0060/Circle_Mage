using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MageControl : MonoBehaviour {
				

	//Mageのprefabsを保存する配列
	public GameObject[] mages;

	private CircelControl circelControl;		
	public GameObject player;					
	public GameObject damageText;		
	public GameObject breakText;
	public GameObject criticalText;
	private PlayerStatus playerStatus;
    private NaviControl naviControl;

	private float cost;		//魔法を使う必要なMP

	// Use this for initialization
	void Start () {
		circelControl = GameObject.Find ("GameRoot").GetComponent<CircelControl> ();
        naviControl = GameObject.Find("GameRoot").GetComponent<NaviControl>();
		playerStatus = GameObject.Find ("Player").GetComponent<PlayerStatus> ();
        for (int i = 0; i < mages.Length; i++)
        {
            mages[i].GetComponent<MageLevelControl>().LoadLevelData(mages[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
		
	/// <summary>
	/// Explosion
	/// </summary>
	public void Fire(){
		UseMahou (0);
	}
	/// <summary>
	/// Ice Needle
	/// </summary>
	public void IceNeedle(){
		UseMahou (1);
	}
	/// <summary>
	/// 雷
	/// </summary>
	public void Kaminari(){
		cost = mages [2].GetComponent<Mage> ().cost;
		//get all enemy
		GameObject[] enemys = GameObject.FindGameObjectsWithTag ("Monster");
		if (enemys.Length > 0) {
			circelControl.HideButtonUI ();
			int hit = 0;
			//reset enemy array order by distance
			CheckNearMonster (ref enemys);
			for (int i =0; i< circelControl.crystals.Count; i++) {
				if (circelControl.crystals [i].GetComponent<Circel>().c_State == Circel.STATE.CHOOSED) {
					if(Check_MP_enough (ref playerStatus.p_MP, cost)){
						circelControl.crystals [i].GetComponent<Circel> ().c_State = Circel.STATE.USING;
						int level = circelControl.crystals [i].GetComponent<Circel> ().circelLevel;
						circelControl.crystals [i].transform.GetChild (0).gameObject.SetActive (false);
						circelControl.crystals [i].transform.GetChild (level).gameObject.SetActive (false);
						for (int j =0; j<3; j++) {
							if (enemys [hit].GetComponent<MonsterStatus> ().m_status != MonsterStatus._status.DEAD) {
								GameObject kaminariClone = Instantiate (mages [2], enemys [hit].transform.position, Quaternion.identity) as GameObject; 
								kaminariClone.transform.position = new Vector3 (enemys [hit].transform.position.x,
									enemys [hit].GetComponent<MonsterStatus> ().monsterSize,
									enemys [hit].transform.position.z);
								kaminariClone.transform.parent = circelControl.crystals [i].transform;
								StartCoroutine (kaminariClone.GetComponent<Mage> ().movePosition (enemys [hit]));
							} else
								j--;
							hit++;
							if (hit >= enemys.Length)
								break;
						}
						if (hit >= enemys.Length)
							break;
					}
				}
			}
		}
	}

	/// <summary>
	/// Mana Burn
	/// </summary>
	public void ManaBurn(){
		UseMahou (3);
	}

	/// <summary>
	/// モンスターをプレイヤーとの距離が近い順で取得する
	/// </summary>
	/// <param name="enemys">Enemys.</param>
	private void CheckNearMonster(ref GameObject[] enemys){
		if (enemys.Length > 3) {
			for (int i = 0; i< enemys.Length; i++) {
				for (int j = i+1; j<enemys.Length; j++) {
					if ( Vector3.Distance (player.transform.position, enemys [i].transform.position )
					                       > Vector3.Distance (player.transform.position, enemys [j].transform.position)) {
						GameObject tempObject = enemys [i];
						enemys [i] = enemys [j];
						enemys [j] = tempObject;
					}
				}
			}
		}
	}

	//
	public void Linghning_Kill(){
		UseMahou (4);
	}

	//
	public void HartBreaker(GameObject btn){
		btn.GetComponent<CoolDown>().StartCoolDown();

		cost = mages [7].GetComponent<Mage> ().cost;
		for (int i =0; i< circelControl.crystals.Count; i++) {
			if(circelControl.crystals [i].GetComponent<Circel>().c_State == Circel.STATE.CHOOSED){
				if(Check_MP_enough (ref playerStatus.p_MP, cost)){
					Vector3 pos = new Vector3 (player.transform.position.x, 0, circelControl.crystals [i].transform.position.z);
					GameObject HartClone = Instantiate(mages[7], pos, Quaternion.identity) as GameObject;
					HartClone.transform.parent = circelControl.crystals[i].transform;
					circelControl.crystals [i].GetComponent<Circel> ().c_State = Circel.STATE.USING;
					int level = circelControl.crystals [i].GetComponent<Circel> ().circelLevel;
					circelControl.crystals [i].transform.GetChild (0).gameObject.SetActive (false);
					circelControl.crystals [i].transform.GetChild (level).gameObject.SetActive (false);
				}
			}
		}
		circelControl.HideButtonUI ();
	}

	//
	public void IceRiver(){
		UseMahou (8);
	}

	/// <summary>
	/// メテオ
	/// </summary>
	public void Meteo(){
		UseMahou (5);
	}

	/// <summary>
	/// ブラックホール
	/// </summary>
	public void AbyssEye(GameObject btn){
        btn.GetComponent<CoolDown>().StartCoolDown();
		UseMahou (6);
	}

	/// <summary>
	/// Removes the damage text.
	/// </summary>
	/// <param name="textClone">Text clone.</param>
	public void RemoveDamageText(GameObject textClone){;
		Destroy (textClone.gameObject);
	}

	/// <summary>
	/// MPが足りるかどうかをチェックする
	/// </summary>
	/// <returns><c>true</c>, if M p enough was checked, <c>false</c> otherwise.</returns>
	/// <param name="MP">MP</param>
	/// <param name="cost">Need MP</param>
	bool Check_MP_enough(ref float MP, float cost){
		cost -= (int)(cost * (playerStatus.costDownRate/100));
        if (MP < cost)
        {
            naviControl.ShowNavi("MP不足", 2, "");
            return false;
        }
        else
        {
            MP -= cost;
            return true;
        }
	}
	/// <summary>
	/// 魔法を発動
	/// </summary>
	/// <param name="index">魔法配列の何番目</param>
	/// <param name="cristal">Cristal.</param>
	void UseMahou(int index){
		circelControl.HideButtonUI ();
		cost = mages [index].GetComponent<Mage> ().cost;
		for (int i = 0; i < circelControl.crystals.Count; i++) {
			if (circelControl.crystals [i].GetComponent<Circel>().c_State == Circel.STATE.CHOOSED) {
				if (Check_MP_enough (ref playerStatus.p_MP, cost)) {
					GameObject meteoClone = Instantiate (mages [index], circelControl.crystals [i].transform.position, Quaternion.identity) as GameObject;
					meteoClone.transform.parent = circelControl.crystals [i].transform;
					circelControl.crystals [i].GetComponent<Circel> ().c_State = Circel.STATE.USING;
					int level = circelControl.crystals [i].GetComponent<Circel> ().circelLevel;
					circelControl.crystals [i].transform.GetChild (0).gameObject.SetActive (false);
					circelControl.crystals [i].transform.GetChild (level).gameObject.SetActive (false);
				}
			}
		}
	}
}
