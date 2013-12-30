using System;

namespace Auth0Client.iOS.Sample
{
	/// <summary>
	/// Class that will be used as the contract to send the Auth information to the Server Api
	/// </summary>
	public class MembershipInfo
	{

		public string Name {
			get;
			set;
		}

		public string Email {
			get;
			set;
		}

		public string Token {
			get;
			set;
		}

		public string Provider {
			get;
			set;
		}

		public string UserId {
			get;
			set;
		}

		public MembershipInfo ()
		{
		}
	}
}

