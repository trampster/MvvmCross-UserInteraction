using System;
using Android.App;
using Android.Content;
using Android.Widget;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Support.V7.Widget;
using Android.Views;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;

namespace Bulboss.MvvmCross.Plugins.UserInteraction.Droid
{
	public class UserInteraction : 
        IAlertUserInteraction, 
        IConfirmUserInteraction, 
        IInputUserInteraction, 
        ISelectorUserInteraction
	{
		protected Activity CurrentActivity {
			get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
		}

		public void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel")
		{
			Confirm(message, confirmed => {
				if (confirmed)
					okClicked();
			},
			title, okButton, cancelButton);
		}

		public void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel")
		{
			//Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction();
			Application.SynchronizationContext.Post(ignored => {
				if (CurrentActivity == null) return;
				new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
					.SetTitle(title)
					.SetPositiveButton(okButton, (sender, args) => answer?.Invoke(true))
					.SetNegativeButton(cancelButton, (sender, args) => answer?.Invoke(false))
					.SetOnCancelListener(new AlertDialogCancelListener(dialog => answer?.Invoke(false)))
					.Show();
         }, null);
		}

		public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
		{
			var tcs = new TaskCompletionSource<bool>();
			Confirm(message, tcs.SetResult, title, okButton, cancelButton);
			return tcs.Task;
		}

	    public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No",
	        string neutral = "Maybe")
	    {
	        Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null) return;
                new AlertDialog.Builder(CurrentActivity)
                    .SetMessage(message)
                        .SetTitle(title)
                        .SetPositiveButton(positive, delegate {
                            if (answer != null)
                                answer(ConfirmThreeButtonsResponse.Positive);
                        })
                        .SetNegativeButton(negative, delegate {
                            if (answer != null)
                                answer(ConfirmThreeButtonsResponse.Negative);
                        })
                        .SetNeutralButton(neutral, delegate {
                            if (answer != null)
                                answer(ConfirmThreeButtonsResponse.Neutral);
                        })
                        .Show();
            }, null);
	    }

        public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No",
            string neutral = "Maybe")
	    {
	        var tcs = new TaskCompletionSource<ConfirmThreeButtonsResponse>();
	        ConfirmThreeButtons(message, tcs.SetResult, title, positive, negative, neutral);
	        return tcs.Task;
	    }

		public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
		{
			Application.SynchronizationContext.Post(ignored => {
				if (CurrentActivity == null) return;
				new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(okButton, delegate {
							if (done != null)
								done();
						})
						.Show();
			}, null);
		}

		public Task AlertAsync(string message, string title = "", string okButton = "OK")
		{
			var tcs = new TaskCompletionSource<object>();
			Alert(message, () => tcs.SetResult(null), title, okButton);
			return tcs.Task;
		}

		public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			Input(message, (ok, text) => {
				if (ok)
					okClicked(text);
			},
				placeholder, title, okButton, cancelButton, initialText);
		}

		public void Input(string message, Action<bool, string> answer, string hint = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			Application.SynchronizationContext.Post(ignored => {
				if (CurrentActivity == null) return;
				var linearLayout = new LinearLayout(CurrentActivity);

				LayoutInflater inflater = (LayoutInflater)CurrentActivity.GetSystemService(Context.LayoutInflaterService);
				var view = inflater.Inflate(Resource.Layout.input_dialog, null);

				var input = view.FindViewById<AppCompatEditText>(Resource.Id.input);
				input.Hint = hint;
				input.Text = initialText;

				new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
						.SetTitle(title)
						.SetView(view)
						.SetPositiveButton(okButton, delegate {
							if (answer != null)
								answer(true, input.Text);
						})
						.SetNegativeButton(cancelButton, delegate {	
							if (answer != null)
								answer(false, input.Text);
						})
						.Show();
			}, null);
		}

		public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			var tcs = new TaskCompletionSource<InputResponse>();
			Input(message, (ok, text) => tcs.SetResult(new InputResponse {Ok = ok, Text = text}),	placeholder, title, okButton, cancelButton, initialText);
			return tcs.Task;
		}

		public int DpToPixel(Context context, float dp)
		{
			return (int)(dp * ((float)context.Resources.DisplayMetrics.DensityDpi / 160f));
		}

		public void Selector(List<SelectorItem> items, Action<SelectorItem> selector, string title = null)
		{
			Application.SynchronizationContext.Post(ignored =>
			{
				if (CurrentActivity == null)
					return;

				new AlertDialog.Builder(CurrentActivity)
							   .SetTitle(title)
							   .SetItems(items.Select(x => x.Text).ToArray(), (s, e) => { if (selector != null) selector(items.ElementAt(e.Which)); })
							   .Show();
			}, null);
		}
	}
}

