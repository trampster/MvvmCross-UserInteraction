using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Bulboss.MvvmCross.Plugins.UserInteraction.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IAlertUserInteraction, UserInteraction>();
            Mvx.RegisterType<ISelectorUserInteraction, UserInteraction>();
            Mvx.RegisterType<IConfirmUserInteraction, UserInteraction>();
            Mvx.RegisterType<IInputUserInteraction, UserInteraction>();
        }
    }
}