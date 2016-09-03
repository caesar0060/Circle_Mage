using UnityEngine;
using System.Collections;

public class MonsterControl : MonoBehaviour {
    [HideInInspector]
	public float timer = 0;			  //モンスターの出る間隔のカウンター
    public GameObject ControlPoint;   //Controllerの場所
    public GameObject[] stages;       //Stageのデータの配列


	// Use this for initialization
	void Start () {
       int stageNum;
	if(LoadLevel.Instance !=null)
         stageNum = LoadLevel.Instance.stageNum;
    else
        stageNum = 0;	
	 GameObject stageCortroller = Instantiate(stages[stageNum], 
            ControlPoint.transform.position,Quaternion.identity) as GameObject;
    }
	// Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
}
