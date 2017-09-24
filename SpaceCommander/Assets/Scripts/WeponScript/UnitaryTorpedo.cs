﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceCommander.Weapons
{
    class UnitaryTorpedo : Torpedo
    {
        protected override void Start()
        {
            Speed = 20f;// скорость ракеты      
            TurnSpeed = 15f;// скорость поворота ракеты            
            DropImpulse = 2000f;//импульс сброса                  
            explosionRange = 5f; //расстояние детонации
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.up * DropImpulse, ForceMode.Impulse);
            Global = FindObjectOfType<GlobalController>();
            lt = 0;
        }
        public override void Explode()
        {
            GameObject blast = Instantiate(FindObjectOfType<GlobalController>().ExplosiveBlast, this.transform.position, this.transform.rotation);
            blast.GetComponent<Explosion>().StatUp(BlastType.UnitaryTorpedo);
            Destroy(gameObject);
        }
    }
}
