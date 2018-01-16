using System;
using System.Windows.Forms;

namespace Game_of_Life {
    static class EntryPoint {

        private static void ParseArgsToPrefixAndArgInt(string[] args, out string argPrefix, out int argHandle) {
            string curArg;
            char[] SpacesOrColons = { ' ', ':' };

            switch (args.Length) {
                case 0: // Nothing on command line, so just start the screensaver.
                    argPrefix = "/s";
                    argHandle = 0;
                    break;
                case 1:
                    curArg = args[0];
                    argPrefix = curArg.Substring(0, 2);
                    curArg = curArg.Replace(argPrefix, ""); // Drop the slash /? part.
                    curArg = curArg.Trim(SpacesOrColons); // Remove colons and spaces.
                    argHandle = curArg == "" ? 0 : int.Parse(curArg); // if empty return zero. else get handle.
                    break;
                case 2:
                    argPrefix = args[0].Substring(0, 2);
                    argHandle = int.Parse(args[1].ToString());
                    break;
                default:
                    argHandle = 0;
                    argPrefix = "";
                    break;
            }
        }

        [STAThread]
        static void Main(string[] args) {
            if (args.Length > 2) {
                MessageBox.Show("Too many arguments on the command line.");
                return;
            }

            ParseArgsToPrefixAndArgInt(args, out string argPrefix, out int argHandle);

            try {
                switch (argPrefix) {
                    case "/a": //admin required;
                        break;

                    case "/c": // Show configuration screen.
                        Application.Run(new ConfigForm());
                        break;

                    case "/p":
                        goto case "/s"; // No handle found, do a fullscreen screensaver.
                    case "/s": // Start screensaver.
                        ScreenSaver screenSaver = new ScreenSaver();
                        screenSaver.RunTillShutdown();
                        screenSaver = null;
                        break;
                    default:
                        break;
                }
            } catch {
            }
        }
    }
}
