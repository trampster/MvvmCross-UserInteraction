using System;
using System.Threading.Tasks;

namespace Bulboss.MvvmCross.Plugins.UserInteraction
{
    public interface IAlertUserInteraction
    {
        void Alert(string message, Action done = null, string title = "", string okButton = "OK");
        Task AlertAsync(string message, string title = "", string okButton = "OK");
    }
}