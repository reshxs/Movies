using System;

namespace MvcClient.Models.Auth
{
    public static class CurrentUser
    {
        public static bool Authorized { get; private set; } = false;
        public static string Id { get; set; }
        public static string Username { get; private set; }
        public static string Token { get; private set; }

        public static void Login(string id, string username, string token)
        {
            id = id;
            Username = username;
            Token = token;
            Authorized = true;
        }

        public static void Logout()
        {
            Id = null;
            Username = null;
            Token = null;
            Authorized = false;
        }
    }
}