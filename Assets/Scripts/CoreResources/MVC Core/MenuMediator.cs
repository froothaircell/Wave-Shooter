using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreResources.MVCCore
{
    public abstract class MenuMediator : MonoBehaviour
    {
        public abstract void InitializeMediator();
        public abstract void DeInitializeMediator();
    }

    public class MenuMediator<TMenuMediatior, TMenuView> : MenuMediator 
        where TMenuMediatior : MenuMediator<TMenuMediatior, TMenuView>
        where TMenuView : MenuView<TMenuView>
    {
        public TMenuView menuView;

        public override void InitializeMediator()
        {
            menuView?.InitializeMenuView();
        }

        public override void DeInitializeMediator()
        {
            menuView?.DeInitializeMenuView();
        }
    }
}
