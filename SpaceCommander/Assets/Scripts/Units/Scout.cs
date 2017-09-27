﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DeusUtility.Random;

namespace SpaceCommander.Units
{
    public class Scout : SpaceShip
    {
        private bool idleFulag;

        protected override void StatsUp()
        {
            base.StatsUp();
            type = UnitClass.Scout;
            EnemySortDelegate = ScoutSortEnemys;
            AlliesSortDelegate = ReconSortEnemys;

            module = new SpellModule[4];
            module[0] = new MissileTrapLauncher(this);
            module[1] = new Jammer(this);
            module[2] = new RadarBooster(this);
            module[3] = new Warp(this);
        }
        protected override void Explosion()
        {
            GameObject blast = Instantiate(Global.ShipDieBlast, gameObject.transform.position, gameObject.transform.rotation);
            blast.GetComponent<Explosion>().StatUp(BlastType.SmallShip);
        }
        protected override void DecrementLocalCounters()
        {
        }
        //AI logick
        protected override bool AttackManeuver()
        {
            switch (targetStatus)
            {
                case TargetStateType.Captured:
                    {
                        UseModule(new SpellFunction[] { SpellFunction.Attack });
                        return Rush();
                    }
                case TargetStateType.InPrimaryRange:
                    {
                        UseModule(new SpellFunction[] { SpellFunction.Self, SpellFunction.Buff });
                        return Evasion(CurrentTarget.transform.right);
                    }
                case TargetStateType.InSecondaryRange:
                    {
                        UseModule(new SpellFunction[] { SpellFunction.Attack, SpellFunction.Buff });
                        return Rush();
                    }
                case TargetStateType.BehindABarrier:
                    {
                        return ToPrimaryDistance();
                    }
                default:
                    return false;
            }
        }
        protected override bool IdleManeuverFunction()
        {
            //Debug.Log("new loop");
            idleFulag = !idleFulag;
            if (idleFulag)
                return PatroolTriangle();
            else return PatroolPoint();
        }
        private bool PatroolTriangle()
        {
            //waitingBackCount = 40f;
            float dirflag;
            if (Randomizer.Uniform(-10, 10, 1)[0] > 0)
                dirflag = -1;
            else dirflag = 1;
            Vector3 point;
            float n = Convert.ToSingle(Randomizer.Uniform(10, 25, 1)[0]);
            float j = 1;
            for (float i = 1; i < 8; i++)
            {
                point = (transform.forward * n * i) + (transform.right * j * dirflag * n) + new Vector3(0, 0.5f, 0) + this.transform.position;
                dirflag = dirflag * -1;
                Driver.MoveToQueue(point);
                j++;
            }
            Driver.MoveToQueue(this.transform.position);
            return true;
        }

        public override void SendTo(Vector3 destination)
        {
            UseModule(new SpellFunction[] { SpellFunction.Attack, SpellFunction.Buff });
            aiStatus = UnitStateType.UnderControl;
            if (Driver.MoveTo(destination) && Team == Global.playerArmy)
                Driver.BuildPathArrows();
        }
        public override void SendToQueue(Vector3 destination)
        {
            UseModule(new SpellFunction[] { SpellFunction.Attack, SpellFunction.Buff });
            aiStatus = UnitStateType.UnderControl;
            if (Driver.MoveToQueue(destination) && Team == Global.playerArmy)
                Driver.BuildPathArrows();
        }
    }
}
