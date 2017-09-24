﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DeusUtility.Random;

namespace SpaceCommander.Weapons
{
    public class MediumCannon : MagWeapon
    {
        public MediumShellType AmmoType;
        public override void StatUp()
        {
            type = WeaponType.Cannon;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            range = 355;
            dispersion = 0.05f;
            shildBlinkTime = 0.15f;
            firerate = 120;
            ammoCampacity = 60;
            ammo = AmmoCampacity;
            reloadingTime = 15;
            PreAiming = true;
            averageRoundSpeed = 133.33f;
        }
        protected override void Shoot(Transform target)
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

            shell.transform.localScale = shell.transform.localScale * 2;

            float speed, damage, armorPiersing, mass;
            bool canRicochet = false;
            GameObject explosionPrefab = null;
            switch (AmmoType)
            {
                case MediumShellType.Camorous:
                    {
                        speed = 110f;
                        damage = 45f;
                        armorPiersing = 3f;
                        mass = 5f;
                        explosionPrefab = Global.ShellBlast;
                        break;
                    }
                case MediumShellType.Subcaliber:
                    {
                        speed = 110f;
                        damage = 40f;
                        armorPiersing = 7;
                        mass = 4;
                        break;
                    }
                case MediumShellType.CamorousAP:
                default:
                    {
                        speed = 110f;
                        damage = 35f;
                        armorPiersing = 5;
                        mass = 5.5f;
                        canRicochet = true;
                        explosionPrefab = Global.ShellBlast;
                        break;
                    }
            }

            shell.GetComponent<IShell>().StatUp(speed * (1 + RoundspeedMultiplacator), damage * (1 + DamageMultiplacator), armorPiersing * (1 + APMultiplacator), mass * (1 + ShellmassMultiplacator), canRicochet, explosionPrefab);
        }
    }
}
