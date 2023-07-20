using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Database
{
    public static class SupabaseConfig
    {
        //For demo purposes, we are hardcoding the API key and URL in our code.However,
        //please use more secure methods such as storing them in a configuration file to avoid sharing your credentials.

        public const string SUPABASE_URL = "https://txmjfvyjbapxastqdcjy.supabase.co"; // Your Supabase URL
        public const string SUPABASE_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InR4bWpmdnlqYmFweGFzdHFkY2p5Iiwicm9sZSI6ImFub24iLCJpYXQiOjE2ODk3OTM2NjMsImV4cCI6MjAwNTM2OTY2M30.-eaHbmLfehQUyVoDRGKe_c-XuOSYGZOn3EwE8FDtOfc"; // Your Supabase Key
    }
}
