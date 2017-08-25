﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DeusUtility.Random;

namespace SpaceCommander.Weapons
{
    public class SmallCannon : RoundWeapon
    {
        public SmallShellType AmmoType;
        public override void StatUp()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            range = 250;
            dispersion = 0.05f;
            shildBlinkTime = 0.1f;
            firerate = 150;
            ammoCampacity = 100;
            ammo = ammoCampacity;
            reloadingTime = 10;
            PreAiming = true;
            averageRoundSpeed = 133.33f;
        }
        protected override void Shoot(Transform target)
        {
            ammo -= 1;
            Quaternion direction = transform.rotation;
            double[] randomOffset = Randomizer.Uniform(10, 90, 2);
            if (randomOffset[0] > 50)
                direction.x = direction.x + (Convert.ToSingle(Global.RandomNormalPool[Convert.ToInt32(randomOffset[0])] - Convert.ToSingle(Global.RandomNormalAverage)) * dispersion);
            else
                direction.x = direction.x + (Convert.ToSingle(Global.RandomNormalPool[Convert.ToInt32(randomOffset[0])] - Convert.ToSingle(Global.RandomNormalAverage)) * -dispersion);
            if (randomOffset[1] > 50)
                direction.y = direction.y + (Convert.ToSingle(Global.RandomNormalPool[Convert.ToInt32(randomOffset[1])] - Convert.ToSingle(Global.RandomNormalAverage)) * dispersion);
            else
                direction.y = direction.y + (Convert.ToSingle(Global.RandomNormalPool[Convert.ToInt32(randomOffset[1])] - Convert.ToSingle(Global.RandomNormalAverage)) * -dispersion);
            GameObject shell = Instantiate(Global.UnitaryShell, gameObject.transform.position, direction);

            float speed, damage, armorPiersing, mass;
            bool canRicochet = false;
            GameObject explosionPrefab = null;
            switch (AmmoType)
            {

                case SmallShellType.SemiShell:
                    {
                        speed = 130f;
                        damage = 25f;
                        armorPiersing = 1.5f;
                        mass = 0.75f;
                        break;
                    }
                case SmallShellType.APShell:
                    {
                        speed = 130f;
                        damage = 15f;
                        armorPiersing = 4;
                        mass = 1.05f;
                        canRicochet = true;
                        break;
                    }
                case SmallShellType.Incendiary:
                    {
                        speed = 130f;
                        damage = 5f;
                        armorPiersing = 1;
                        mass = 1;
                        explosionPrefab = Global.ShellBlast;
                        break;
                    }
                case SmallShellType.BuckShot:
                    {
                        speed = 100f;
                        damage = 30f;
                        armorPiersing = 1;
                        mass = 0.6f;
                        canRicochet = true;
                        break;
                    }
                case SmallShellType.Solid:
                default:
                    {
                        speed = 130f;
                        damage = 20f;
                        armorPiersing = 2;
                        mass = 1;
                        canRicochet = true;
                        break;
                    }
            }

            shell.GetComponent<IShell>().StatUp(speed, damage, armorPiersing, mass, canRicochet, explosionPrefab);
        }
    }
}
