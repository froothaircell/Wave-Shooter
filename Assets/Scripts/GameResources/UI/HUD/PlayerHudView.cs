using UnityEngine;
using CoreResources.MVCCore;
using GameResources.Player;

namespace GameResources.UI.HUD
{
    public class PlayerHudView : MenuView<PlayerHudView>
    {
        public GameObject healthBar;
        public RectTransform healthBarRect;

        public override void InitializeMenuView()
        {
            healthBarRect.localScale = new Vector3(1f, 1f, 1f);
        }

        public override void DeInitializeMenuView()
        {
            healthBarRect.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
