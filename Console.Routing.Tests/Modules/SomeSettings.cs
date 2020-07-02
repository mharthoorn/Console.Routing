namespace ConsoleRouting.Tests
{
    [Global]
    public static class SomeSettings
    {
        public static Flag Debug { get; set; }

    }

    public static class AnimalSettings
    {
        public static Flag Mouse { get; set; }
        public static bool Cat { get; set; } = false;
        public static Flag Canary { get; set; } 
    }


}
