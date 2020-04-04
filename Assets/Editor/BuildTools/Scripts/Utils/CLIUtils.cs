public class CLIUtils
{
    public static string GetCommandLineArg(string arg)
    {
        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == arg)
            {
                return args[i + 1];
            }
        }

        return string.Empty;
    }
}