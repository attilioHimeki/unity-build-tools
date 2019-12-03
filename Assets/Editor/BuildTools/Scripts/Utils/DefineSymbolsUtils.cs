using System;

public static class DefineSymbolsUtils
{
    private static readonly char[] SPLIT_DIVIDER = new char[] { ';' };
    private const string JOIN_SEPARATOR = ";";

    public static string[] splitDefineSymbolString(string defineSymbols)
    {
        string[] split = defineSymbols.Split(SPLIT_DIVIDER, StringSplitOptions.RemoveEmptyEntries);

        return split;
    }

    public static string mergeDefineSymbols(string[] defineSymbols)
    {
        string merge = string.Join(JOIN_SEPARATOR, defineSymbols);

        return merge;
    }


}