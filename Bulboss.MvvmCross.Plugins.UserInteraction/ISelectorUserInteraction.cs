using System;
using System.Collections.Generic;

namespace Bulboss.MvvmCross.Plugins.UserInteraction
{
    public interface ISelectorUserInteraction
    {
        void Selector(List<SelectorItem> items, Action<SelectorItem> selector, string title = null);
    }
}