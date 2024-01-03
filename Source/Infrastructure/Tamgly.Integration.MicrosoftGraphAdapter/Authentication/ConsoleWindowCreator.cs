using System.Runtime.InteropServices;

namespace Tamgly.Integration.MicrosoftGraphAdapter.Authentication;

public static class ConsoleWindowCreator
{
    private enum GetAncestorFlags
    {
        GetParent = 1,
        GetRoot = 2,
        /// <summary>
        /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
        /// </summary>
        GetRootOwner = 3
    }

    /// <summary>
    /// Retrieves the handle to the ancestor of the specified window.
    /// </summary>
    /// <param name="hwnd">A handle to the window whose ancestor is to be retrieved.
    /// If this parameter is the desktop window, the function returns NULL. </param>
    /// <param name="flags">The ancestor to be retrieved.</param>
    /// <returns>The return value is the handle to the ancestor window.</returns>
    [DllImport("user32.dll", ExactSpelling = true)]
    private static extern nint GetAncestor(nint hwnd, GetAncestorFlags flags);

    [DllImport("kernel32.dll")]
    private static extern nint GetConsoleWindow();

    public static nint GetConsoleOrTerminalWindow()
    {
        nint consoleHandle = GetConsoleWindow();
        nint handle = GetAncestor(consoleHandle, GetAncestorFlags.GetRootOwner);

        return handle;
    }
}