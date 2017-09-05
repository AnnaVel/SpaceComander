﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DeusUtility.Random;

namespace SpaceCommander.Weapons
{
    public class AutoBuckshotCannon : RoundWeapon
    {
        private int bucksotRate;
        public override void StatUp()
        {
            type = WeaponType.ShootCannon;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            range = 200;
            dispersion = 0.6f;
            shildBlinkTime = 0.1f;
            firerate = 150;
            ammoCampacity = 30;
            ammo = AmmoCampacity;
            reloadingTime = 15;
            PreAiming = true;
            averageRoundSpeed = 100f;
            bucksotRate = 12;
        }
        protected override void Shoot(Transform target)
        {
            for (int i = 0; i < bucksotRate; i++)
            {
                Quaternion direction = transform.rotation;
                double[] randomOffset = Randomizer.Uniform(10, 90, 2);
                if (randomOffset[0] > 50)
                    direction.x = direction.x + (Convert.ToSingle(Global.RandomNormalPool[Convert.ToInt32(randomOffset[0])] - Convert.ToSingle(Global.RandomNormalAverage)) * Dispersion);
                else
                    direction.x = direction.x + (Convert.ToSingle(Global.RandomNormalPool[Convert.ToInt32(randomOffset[0])] - Convert.ToSingle(Global.RandomNormalAverage)) * -Dispersion);
                if (randomOffset[1] > 50)
                    direction.y = direction.y + (Convert.ToSingle(Global.RandomNormalPool[Convert.ToInt32(randomOffset[1])] - Convert.ToSingle(Global.RandomNormalAverage)) * Dispersion);
                else
                    direction.y = direction.y + (Convert.ToSingle(Global.RandomNormalPool[Convert.ToInt32(randomOffset[1])] - Convert.ToSingle(Global.RandomNormalAverage)) * -Dispersion);
                GameObject shell = Instantiate(Global.UnitaryShell, gameObject.transform.position, direction);

                float speed, damage, armorPiersing, mass;
                bool canRicochet = true;
                GameObject explosionPrefab = null;
                speed = 100f;
                damage = 10f;
                armorPiersing = 3;
                mass = 0.3f;

                shell.GetComponent<IShell>().StatUp(speed * (1 + RoundspeedMultiplacator), damage * (1 + DamageMultiplacator), armorPiersing * (1 + APMultiplacator), mass * (1 + ShellmassMultiplacator), canRicochet, explosionPrefab);
            }
        }
    }
}

