using Android.Content;
using System;

namespace Bulboss.MvvmCross.Plugins.UserInteraction.Droid
{
   public class AlertDialogCancelListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
   {
      readonly Action<IDialogInterface> _cancelAction;

      public AlertDialogCancelListener(Action<IDialogInterface> cancelAction)
      {
         _cancelAction = cancelAction;   
      }

      public void OnCancel(IDialogInterface dialog)
      {
         _cancelAction(dialog);
      }
   }
}