﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceCommander.Units
{
    public class Bomber : SpaceShip
    {
        public TorpedoType StrategicLoad;
        protected override void StatsUp()
        {
            type = UnitClass.Bomber;
            radarRange = 600; //set in child
            radarPover = 0.5f;
            speedThrust = 7.5f; //set in child
            speedRotation = 33;
            speedShift = 6;
            stealthness = 0.4f; //set in child
            radiolink = 2.5f;
            StrategicLoad = TorpedoType.Nuke;
            EnemySortDelegate = BomberSortEnemys;
            AlliesSortDelegate = SupportCorvetteSortEnemys;

            module = new SpellModule[1];
            module[0] = new MissileTrapLauncher(this);
        }

        protected override void Explosion()
        {
            GameObject blast = Instantiate(Global.ShipDieBlast, gameObject.transform.position, gameObject.transform.rotation);
            blast.GetComponent<Explosion>().StatUp(BlastType.MediumShip);
        }
        protected override void DecrementLocalCounters()
        {

        }
        protected override bool AttackManeuver()
        {
            switch (targetStatus)
            {
                case TargetStateType.Captured:
                    {
                        return ToSecondaryDistance();
                    }
                case TargetStateType.InPrimaryRange:
                    {
                        return IncreaseDistance();
                    }
                case TargetStateType.InSecondaryRange:
                    {
                        return Evasion(CurrentTarget.transform.right);
                    }
                case TargetStateType.BehindABarrier:
                    {
                        return ToSecondaryDistance();
                    }
                default:
                    return false;
            }
        }
    }
}
