using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stronghold.Base;

public class Data : MonoBehaviour{
    public static Data database;
    public ArrayList towers;
    public ArrayList enemies;
    public EntityBase player;

    void Awake(){
        if (database == null){
            database = this;
            towers = new ArrayList();
            enemies = new ArrayList();
        }else{
            Destroy(this);
        }
    }
}
