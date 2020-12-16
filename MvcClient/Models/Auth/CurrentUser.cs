using System;

namespace MvcClient.Models.Auth
{
    public static class CurrentUser
    {
        public static bool Authorized { get; private set; } = false;

        public static string Username { get; private set; }
        public static string Token { get; private set; }

        public static void Login(string username, string token)
        {
            Username = username;
            Token = token;
            Authorized = true;
        }

        public static void Logout()
        {
            Username = null;
            Token = null;
            Authorized = false;
        }
    }
}