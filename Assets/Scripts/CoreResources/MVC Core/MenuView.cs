using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreResources.MVCCore
{
    public class MenuView<TMenuView> : MonoBehaviour where TMenuView : MenuView<TMenuView>
    {
        public virtual void InitializeMenuView() { }
        public virtual void DeInitializeMenuView() { }
    }
}
