using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class LevelUpControl : MonoBehaviour {

	public GameObject[] mages;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LevelUpdate(){
		FileStream textFile = new FileStream ("Assets/Resources/Mage/MahouLevel.txt", FileMode.Open, FileAccess.Write);
		StreamWriter sw = new StreamWriter (textFile);
		for (int i = 0; i < mages.Length; i++) {
			Mage mage = mages [i].GetComponent<Mage> ();

			sw.WriteLine (mage.level + "\t" + mage.name);
			Debug.Log (mage.level + "\t" + mage.name);
		}
		sw.Close ();
	}
}
