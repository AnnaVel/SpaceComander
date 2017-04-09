﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeProject
{
    public class Bait : SpaceShip
    {
        protected override void StatsUp()
        {
            type = UnitClass.LR_Corvette;
            radarRange = 300; //set in child
            radarPover = 10;
            speed = 10; //set in child
            stealthness = 1f; //set in child
            radiolink = 2f;
            movementAiEnabled = true;
            combatAIEnabled = true;
            selfDefenceModuleEnabled = true;
            roleModuleEnabled = false;
        }
        protected override void DecrementCounters()
        {
        }
        protected override bool RoleFunction()
        {
            return false;
        }
        protected override bool SelfDefenceFunction()
        {
            SelfDefenceFunctionBase();
            return true;
        }
    }
}
