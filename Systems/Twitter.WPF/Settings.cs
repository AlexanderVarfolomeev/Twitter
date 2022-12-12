﻿namespace Twitter.WPF;

public class Settings
{
    public static string ApiRoot = "https://localhost:5000/api";

    public static string IdentityRoot = "https://localhost:5001";
    public static string ClientId = "wpf";
    public static string ClientSecret = "secret";

    public static string AccessToken = "";
    public static string DefaultAvatar ="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";
}