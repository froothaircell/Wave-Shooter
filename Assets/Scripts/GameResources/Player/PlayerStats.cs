using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulletFury;
using BulletFury.Data;
using GameResources.Character;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Player
{
    public enum DodgeSlotState
    {
        Empty = 0,
        Refilling = 1,
        Full = 2
    }
    
    public class PlayerStats : RCharacterStats
    {
        public int MaxShield = 100;
        private int _shield;
        private const int _totalDodges = 2; // should be alterable later
        private float _shieldRechargeTime = 4f; // Should be an equation
        private float _dodgeRechargeTime = 2f; // based off of the level
        private float _shieldTimer;
        private DodgeSlotState[] _dodgeSlotStates;

        public Action<int> OnDamageTaken;

        public override void OnInit()
        {
            SetStats();
            _dodgeSlotStates = new DodgeSlotState[_totalDodges];
            for (int i = 0; i < _dodgeSlotStates.Length; i++)
                _dodgeSlotStates[i] = DodgeSlotState.Full;
        }

        public override void OnUpdate()
        {
            // CheckDodgeSlots();
        }

        public override void OnDeInit()
        {
        }

        public override void SetStats()
        {
            _shield = MaxShield;
            base.SetStats();
        }

        public void TakeDamage(BulletContainer container, BulletCollider collider)
        {
            TakeDamage((int) container.Damage);
        }

        public override void TakeDamage(int dmg)
        {
            if (_shield > dmg)
            {
                _shield -= dmg;
                return;
            }

            var healthDed = dmg;
            if (_shield > 0)
            {
                healthDed -= _shield;
                _shield = 0;
                Debug.Log("Player Shield Destroyed!");
                // Shield destroyed effect
            }
            base.TakeDamage(healthDed);

            OnDamageTaken.Invoke(dmg);
        }

        public bool ConsumeDodge()
        {
            for (int i = 0; i < _dodgeSlotStates.Length; i++)
            {
                if (_dodgeSlotStates[i] == DodgeSlotState.Full)
                {
                    _dodgeSlotStates[i] = DodgeSlotState.Empty;
                    Task.Run(async () => { await RefillDodge(i); });
                    return true;
                }
            }

            return false;
        }

        /*private async Task CheckDodgeSlots()
        {
            List<Task> taskList = new List<Task>();
            for (int i = 0; i < _dodgeSlotStates.Length; i++)
            {
                if (_dodgeSlotStates[i] == DodgeSlotState.Full || _dodgeSlotStates[i] == DodgeSlotState.Refilling)
                {
                    continue;
                }
                taskList.Add(RefillDodge(i));
            }

            await Task.WhenAll(taskList);
        }*/

        private async Task RefillDodge(int dodgeSlot)
        {
            _dodgeSlotStates[dodgeSlot] = DodgeSlotState.Refilling;
            await Task.Delay((int) _dodgeRechargeTime * 1000);

            _dodgeSlotStates[dodgeSlot] = DodgeSlotState.Full;
        }

        protected override void OnDeath()
        {
            REvent_PlayerDeath.Dispatch(transform);
        }
    }
}