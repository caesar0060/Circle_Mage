using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyPoint : MonoBehaviour {

    public Gizmos[] gizmos;
	public int m_level = 1;					//ボスのレベル
    public GameObject[] Monsters;       //モンスターのプレハブの配列
    public int createMonsters = 0;      //モンスター出現回数
    GameObject[] existEnemys;
    public float interval = 3.0f;       //モンスター出現間隔
   
    public float startTime;             //モンスターを出す
	private bool isStart = false;		//モンスターを出しているかどうか
    public int monsterCount;           //出現する回数
    private int counter = 0;
    public GameObject HP_Bar;			//モンスターのHPゲージ
    private MonsterControl monsterControl;
    private IEnumerator routine;	//コルーチン


    // Use this for initialization
    void Start()
    {
        routine = Appearance();
        monsterControl = GameObject.Find("GameRoot").GetComponent<MonsterControl>();
        //配列の確保
        existEnemys = new GameObject[createMonsters];
        
    }

    // Update is called once per frame
    void Update()
    {
        if (monsterControl.timer >= startTime && !isStart)
        {
            isStart = true;
            StartWave();
        }
    }

    public void Generate(int num)
    {
        for (int i = 0; i < existEnemys.Length; i++)
        {
            if (existEnemys[i] == null)
            {
                Vector3 pos = new Vector3(0, 0, 0);
               // pos = new Vector3(Random.Range(6,-45), 0, Random.Range(-12,10));
                pos = this.transform.position;
                if(num == -1 ||num >= Monsters.Length )
                    num = Random.Range(0, Monsters.Length);
				existEnemys [i] = Instantiate (Monsters [num], pos, Quaternion.identity) as GameObject;
				MonsterStatus ms = existEnemys [i].GetComponent<MonsterStatus> ();
				ms.m_Level = m_level;
				ms.LoadMonsterData ();
                //モンスターの高さ
				float monsterHeight = ms.monsterSize;
                pos.y += monsterHeight;
                //モンスターの上にHPゲージをつける
                GameObject HP_Bar_Clone = Instantiate(HP_Bar, pos, Quaternion.identity) as GameObject;
                HP_Bar_Clone.transform.SetParent(existEnemys[i].transform);
				ms.HP_Slider = existEnemys [i].GetComponentInChildren<Slider> ();
				ms.HP_Slider.maxValue = ms.HP;
                counter++;
                if (counter >= monsterCount)
                    StopCoroutine(routine);
                return;
            }
        }
    }
    /// <summary>
    /// Waveをスタート
    /// </summary>
    public void StartWave()
    {
        //コルーチンスタート
        StartCoroutine(routine);
    }
    //モンスターの出現コルーチン
    private IEnumerator Appearance()
    {
        while (true)
        {
            Generate(-1);
            yield return new WaitForSeconds(interval);
        }
    }

    void OnDrawGizmos()
    {
        Vector3 a = new Vector3(0, 1, 0);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + a, 1.0f);
    }
}
