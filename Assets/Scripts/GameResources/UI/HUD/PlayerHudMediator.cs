using UnityEngine;
using CoreResources.MVCCore;
using GameResources.Player;

namespace GameResources.UI.HUD
{
    public class PlayerHudMediator : MenuMediator<PlayerHudMediator, PlayerHudView>
    {
        private PlayerStats playerStats;

        public override void InitializeMediator()
        {
            base.InitializeMediator();
            playerStats = AppHandler.CharacterManager.PlayerShip.GetComponent<PlayerStats>();

            playerStats.OnDamageTaken += AnimateHealthBar;
        }

        public override void DeInitializeMediator()
        {
            base.DeInitializeMediator();

            playerStats.OnDamageTaken -= AnimateHealthBar;
        }

        public void AnimateHealthBar(int damage)
        {
            var currentHealth = (float) playerStats.GetHealthNormalized();
            if (playerStats.GetHealth() - damage <= 0)
            {
                menuView.healthBarRect.localScale = new Vector3(0f, 1f, 1f);
                return;
            }
            menuView.healthBarRect.localScale = new Vector3(currentHealth, 1f, 1f);
        }
    }
}