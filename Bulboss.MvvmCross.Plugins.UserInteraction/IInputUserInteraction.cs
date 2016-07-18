using System;
using System.Threading.Tasks;

namespace Bulboss.MvvmCross.Plugins.UserInteraction
{
    public interface IInputUserInteraction
    {
        void Input(string message, Action<string> okClicked, string placeholder = null, string title = null,
            string okButton = "OK", string cancelButton = "Cancel", string initialText = null);

        void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null,
            string okButton = "OK", string cancelButton = "Cancel", string initialText = null);

        Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null,
            string okButton = "OK", string cancelButton = "Cancel", string initialText = null);
    }
}