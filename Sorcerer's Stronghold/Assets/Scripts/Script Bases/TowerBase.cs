using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stronghold.Base{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TowerBase : EntityBase{
        public bool resourceTower = false;
    }
}
