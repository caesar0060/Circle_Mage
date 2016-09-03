using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossPoint : MonoBehaviour
{

    public Gizmos[] gizmos;
	public int b_level = 1;				//ボスのレベル
    public GameObject Monster;      //モンスターのプレハブ
    public float startTime;         //モンスターを出す
    private bool isStart = false;	//モンスターを出しているかどうか
    public GameObject HP_Bar;		//モンスターのHPゲージ
    private MonsterControl monsterControl;



    // Use this for initialization
    void Start()
    {
        monsterControl = GameObject.Find("GameRoot").GetComponent<MonsterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (monsterControl.timer >= startTime && !isStart)
        {
            isStart = true;
            Generate();
        }
    }

    public void Generate()
    {

        Vector3 pos = new Vector3(0, 0, 0);
        // pos = new Vector3(Random.Range(6,-45), 0, Random.Range(-12,10));
        pos = this.transform.position;
        GameObject bossClone = Instantiate(Monster, pos, Quaternion.identity) as GameObject;
		MonsterStatus bs = bossClone.GetComponent<MonsterStatus> ();
		bs.m_Level = b_level;
		bs.LoadMonsterData ();
        bossClone.transform.GetChild(0).gameObject.tag = "Boss";
        //モンスターの高さ
        float monsterHeight = bs.monsterSize;
        //モンスターの上にHPゲージをつける
		GameObject HP_Bar_Clone = Instantiate(HP_Bar, new Vector3(43f,16.4f,16.2f), Quaternion.Euler(22.2f,-115.8f,0)) as GameObject;
		bs.HP_Slider = HP_Bar_Clone.GetComponentInChildren<Slider> ();
		Sprite bossName = GameObject.FindGameObjectWithTag ("BossName").GetComponent<Image> ().sprite = 
			bossClone.GetComponentInChildren<BossNavi> ().nameSprite;	
		bs.HP_Slider.maxValue = bs.HP;
    }

    void OnDrawGizmos()
    {
        Vector3 a = new Vector3(0, 1, 0);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + a, 1.0f);
    }
}
