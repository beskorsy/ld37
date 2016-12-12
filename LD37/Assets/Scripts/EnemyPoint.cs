using UnityEngine;
using System.Collections;

public class EnemyPoint : MonoBehaviour {
    
	void Start () {
        GameManager.instance.AddEnemyPointToList(this);
	}
	
}
