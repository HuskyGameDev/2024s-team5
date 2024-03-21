using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stronghold.Base;

public class Data : MonoBehaviour{
    public static Data database;
    public List<TowerBase> towers = new List<TowerBase>();
    public List<EnemyBase> enemies = new List<EnemyBase>();
    public List<EntityBase> player = new List<EntityBase>();

    void Awake(){
        if (database == null){
            database = this;
        }else{
            Destroy(this);
        }
    }
}
