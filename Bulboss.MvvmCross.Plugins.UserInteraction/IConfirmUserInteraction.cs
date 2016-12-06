using System;
using System.Threading.Tasks;

namespace Bulboss.MvvmCross.Plugins.UserInteraction
{
    public interface IConfirmUserInteraction
    {
        void Confirm(string message, Action okClicked, string title = null, string okButton = "OK",
            string cancelButton = "Cancel");

        void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK",
            string cancelButton = "Cancel");

        Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK",
            string cancelButton = "Cancel");

        void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null,
            string positive = "Yes", string negative = "No", string neutral = "Maybe");

        Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null,
            string positive = "Yes", string negative = "No", string neutral = "Maybe");
    }
}