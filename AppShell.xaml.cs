namespace TelluriumPOS
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(TerminalPage), typeof(TerminalPage));
            Routing.RegisterRoute(nameof(Settings), typeof(Settings));
            Routing.RegisterRoute(nameof(DebuggerPage), typeof(DebuggerPage));
            Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));
        }
    }
}
