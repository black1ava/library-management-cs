using System;
using Gtk;
using Pango;
using static LibraryManagement.AppMenu;

namespace LibraryManagement {
  public class AuthorShow: Window {

    private Fixed container;
    private Label header;
    private TreeView treeView;
    private TreeStore treeStore;
    private Button updateButton;
    private Button deleteButton;
    private Button refreshButton;

    public AuthorShow(): base("Librarians") {
      this.SetDefaultSize(1000, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);
      
      this.container = new Fixed();
      this.Add(this.container);

      new AppMenu(this, this.container);

      this.header = new Label("All authors");
      FontDescription fontDescription = new FontDescription();
      fontDescription.Weight = Weight.Bold;
      fontDescription.Size = 1024 * 20;
      this.header.ModifyFont(fontDescription);
      this.container.Put(this.header, 20, 30);

      this.treeStore = new TreeStore(
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string),
        typeof(string)
      );

      this.treeView = new TreeView();
      this.treeView.Model = this.treeStore;
      this.treeView.HeadersVisible = true;
      this.treeView.EnableGridLines = TreeViewGridLines.Both;

      this.treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
      this.treeView.AppendColumn("Gender", new CellRendererText(), "text", 1);
      this.treeView.AppendColumn("Name", new CellRendererText(), "text", 2);
      this.treeView.AppendColumn("Date of birth", new CellRendererText(), "text", 3);
      this.treeView.AppendColumn("Place of birth", new CellRendererText(), "text", 4);
      this.treeView.AppendColumn("Address", new CellRendererText(), "text", 5);
      this.treeView.AppendColumn("Phone", new CellRendererText(), "text", 6);
      this.treeView.AppendColumn("Email", new CellRendererText(), "text", 7);

      ScrolledWindow sw = new ScrolledWindow();
      sw.Add(this.treeView);
      sw.WidthRequest = 800;
      sw.HeightRequest = 800;
      this.container.Put(sw, 0, 100);

      this.updateButton = new Button("Update");
      this.updateButton.WidthRequest = 200;
      this.updateButton.Sensitive = false;
      this.container.Put(this.updateButton, 850, 100);

      this.deleteButton = new Button("Delete");
      this.deleteButton.WidthRequest = 200;
      this.deleteButton.Sensitive = false;
      this.container.Put(this.deleteButton, 850, 150);

      this.refreshButton = new Button("Refresh");
      this.refreshButton.WidthRequest = 200;
      this.refreshButton.Clicked += new EventHandler(this.OnRefreshButtonClicked);
      this.container.Put(this.refreshButton, 850, 200);

      this.ShowAll();
    }

    private void OnRefreshButtonClicked(object obj, EventArgs args){
      this.Destroy();
      new AuthorShow();
    }

    private void Exit(object obj, DeleteEventArgs args){
      Application.Quit();
    }
  }
}