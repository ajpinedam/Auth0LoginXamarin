using System;

namespace Auth0Client.iOS.Sample
{
	public class Auth0Connection
	{
		private const string _facebook = "facebook";
		private const string _google = "google-oauth2";
		private const string _twitter="twitter";
		private const string _windowsLive="windowslive";

		public static string Facebook
		{
			get{ return _facebook;}
		}

		public static string Google
		{
			get{ return _google;}
		}

		public static string Twitter
		{
			get{ return _twitter;}
		}

		public static string WindowsLive
		{
			get{ return _windowsLive;}
		}

	}
}

