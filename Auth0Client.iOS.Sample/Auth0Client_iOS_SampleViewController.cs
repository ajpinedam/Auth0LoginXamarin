using System;
using System.Drawing;
using System.Threading.Tasks;
using Auth0.SDK;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;

namespace Auth0Client.iOS.Sample
{
	public partial class Auth0Client_iOS_SampleViewController : DialogViewController
	{
		private Auth0.SDK.Auth0Client client = new Auth0.SDK.Auth0Client (
			Auth0LoginCredentials.Name,
			Auth0LoginCredentials.ClientId,
			Auth0LoginCredentials.ClientSecret);

		private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

		public Auth0Client_iOS_SampleViewController (RootElement root) : base(root)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			this.Initialize ();
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		private void LoginWithWidgetButtonClick ()
		{
			// This will show all connections enabled in Auth0, and let the user choose the identity provider
			this.client.LoginAsync (this)					// current controller
						.ContinueWith(
							task => this.ShowResult (task), 
							this.scheduler);
		}

		private void LoginWithConnectionButtonClick ()
		{
			// This uses a specific connection: google-oauth2
			this.client.LoginAsync (this, Auth0Connection.Google)	// current controller and connection name
						.ContinueWith(
							task => this.ShowResult (task), 
							this.scheduler);
		}

		private void LoginWithConnectionButtonFbClick ()
		{
			//using Facebook specific connection:
			this.client.LoginAsync (this, Auth0Connection.Facebook)
				.ContinueWith (
					t => this.ShowResult (t), 
					this.scheduler);
		
		}

		private void LoginWithUsernamePassword ()
		{
			// Show loading animation
			this.loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			this.View.Add (this.loadingOverlay);

			// This uses a specific connection (named sql-azure-database in Auth0 dashboard) which supports username/password authentication
			this.client.LoginAsync ("sql-azure-database", this.userNameElement.Value, this.passwordElement.Value)
						.ContinueWith (
							task => this.ShowResult (task),
							this.scheduler);
		}

		private void ShowResult(Task<Auth0User> taskResult)
		{
			Exception error = taskResult.Exception != null ? taskResult.Exception.Flatten () : null;

			if (error == null && taskResult.IsCanceled) {
				error = new Exception ("Authentication was canceled.");
			}

			if (taskResult.IsCompleted) 
			{
				var membershipInfo = new MembershipInfo ();

				membershipInfo.Name = (string)taskResult.Result.Profile ["name"];
				membershipInfo.Email = (string)taskResult.Result.Profile ["email"];
				membershipInfo.Token = (string)taskResult.Result.Profile ["identities"][0]["access_token"];
				membershipInfo.Provider = (string)taskResult.Result.Profile ["identities"][0]["provider"];
				membershipInfo.UserId = (string)taskResult.Result.Profile["user_id"];

				string _dataToInsert = String.Format("-name: {0} \n-email: {1} \n-user_id: {2} \n-provider: {3} \n-token: {4}", 
					membershipInfo.Name, 
					membershipInfo.Email, 
					membershipInfo.UserId, 
					membershipInfo.Provider, 
					membershipInfo.Token
				 ).Replace("\n", Environment.NewLine);

				this.dataElement.Value = _dataToInsert;
			}

			this.resultElement.Value = error == null ?
				taskResult.Result.Profile.ToString () :
				error.InnerException != null ? error.InnerException.Message : error.Message;

			this.ReloadData ();
			this.loadingOverlay.Hide ();
		}
	}
}
