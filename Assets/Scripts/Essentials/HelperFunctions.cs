namespace DefaultNamespace
{
    public static class HelperFunctions
    {
        public static string IntToSymbol(int val, char symbol)
        {
            return val <= 0 ? "" : new string(symbol, val);
        }

        //public static void Stop()
        //{
        //    #if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //    #else
        //        Application.Quit();
        //    #endif
        //}
    }
}