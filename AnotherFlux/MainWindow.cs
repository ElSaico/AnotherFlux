using System;
using Gtk;

public partial class MainWindow : Window
{
    public MainWindow() : base("Another Flux")
    {
        DeleteEvent += delegate { Application.Quit(); };

        MenuBar mainMenu = new MenuBar();

        Menu fileMenu = new Menu();
        AccelGroup fileAgr = new AccelGroup();
        AddAccelGroup(fileAgr);
        MenuItem file = new MenuItem("File");
        file.Submenu = fileMenu;

        MenuItem open = new MenuItem("Open");
        open.AddAccelerator("activate", fileAgr,
            new AccelKey(Gdk.Key.o, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        open.Activated += OnOpen;
        fileMenu.Append(open);

        MenuItem save = new MenuItem("Save");
        save.AddAccelerator("activate", fileAgr,
            new AccelKey(Gdk.Key.s, Gdk.ModifierType.ControlMask | Gdk.ModifierType.ShiftMask, AccelFlags.Visible));
        save.Activated += OnSave;
        fileMenu.Append(save);

        MenuItem saveAs = new MenuItem("Save as...");
        saveAs.Activated += OnSaveAs;
        fileMenu.Append(saveAs);

        MenuItem autoArchive = new MenuItem("Auto-Archive");
        autoArchive.Activated += OnAutoArchive;
        fileMenu.Append(autoArchive);

        MenuItem markModified = new MenuItem("Mark All Modified");
        markModified.Activated += OnMarkModified;
        fileMenu.Append(markModified);

        fileMenu.Append(new SeparatorMenuItem());

        MenuItem compression = new MenuItem("Compression...");
        compression.Activated += OnCompression;
        fileMenu.Append(compression);

        MenuItem export = new MenuItem("Export...");
        export.Activated += OnExport;
        fileMenu.Append(export);

        MenuItem import = new MenuItem("Import...");
        import.Activated += OnImport;
        fileMenu.Append(import);

        // Patches >
        // > Expand ROM
        // > All Overworlds have a NLZ
        // > Dactyl NLZ is not origin based
        // > Startup Location
        // > Beta Events

        fileMenu.Append(new SeparatorMenuItem());

        MenuItem exit = new MenuItem("Exit");
        exit.AddAccelerator("activate", fileAgr,
            new AccelKey(Gdk.Key.F4, Gdk.ModifierType.MetaMask, AccelFlags.Visible));
        exit.Activated += OnExit;
        fileMenu.Append(exit);

        mainMenu.Append(file);

        Menu windowMenu = new Menu();
        AccelGroup windowAgr = new AccelGroup();
        AddAccelGroup(windowAgr);
        MenuItem window = new MenuItem("Window");
        window.Submenu = windowMenu;

        mainMenu.Append(window);

        Menu pluginsMenu = new Menu();
        AccelGroup pluginsAgr = new AccelGroup();
        AddAccelGroup(pluginsAgr);
        MenuItem plugins = new MenuItem("Plugins");
        plugins.Submenu = pluginsMenu;

        mainMenu.Append(plugins);

        Menu helpMenu = new Menu();
        AccelGroup helpAgr = new AccelGroup();
        AddAccelGroup(helpAgr);
        MenuItem help = new MenuItem("Help");
        help.Submenu = helpMenu;

        MenuItem manual = new MenuItem("Manual");
        manual.AddAccelerator("activate", fileAgr,
            new AccelKey(Gdk.Key.F1, Gdk.ModifierType.None, AccelFlags.Visible));
        manual.Activated += OnManual;
        helpMenu.Append(manual);

        MenuItem ack = new MenuItem("Acknowledgements");
        ack.Activated += OnAck;
        helpMenu.Append(ack);

        MenuItem about = new MenuItem("About...");
        about.Activated += OnAbout;
        helpMenu.Append(about);

        mainMenu.Append(help);

        VBox vbox = new VBox(false, 2);
        vbox.PackStart(mainMenu, false, false, 0);
        vbox.PackStart(new Label(), false, false, 0);

        Add(vbox);

        ShowAll();
    }

    private void OnOpen(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnSave(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnSaveAs(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnAutoArchive(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnMarkModified(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnCompression(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnExport(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnImport(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnExit(object sender, EventArgs e)
    {
        Application.Quit();
    }

    private void OnManual(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnAck(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnAbout(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
