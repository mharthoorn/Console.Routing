namespace ConsoleRouting.Tests
{
    [Global]
    public static class SomeSettings
    {
        public static bool Debug { get; set; }

    }

    [Global]
    public static class AnimalSettings
    {
        public static bool Mouse { get; set; }
        public static bool Cat { get; set; } = false;
        public static bool Canary { get; set; } 
    }


}
