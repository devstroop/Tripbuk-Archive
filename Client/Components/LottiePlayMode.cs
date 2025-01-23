namespace TripBUK.Client.Components;

public enum LottiePlayMode
{
    Normal,
    Bounce,
}
public static class LottiePlayModeExtensions
{
    public static string ToLottiePlayModeString(LottiePlayMode mode)
    {
        return mode switch
        {
            LottiePlayMode.Normal => "normal",
            LottiePlayMode.Bounce => "bounce",
            _ => "normal",
        };
    }
}