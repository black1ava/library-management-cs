using System;
using Gtk;
using static LibraryManagement.AppMenu;

namespace LibraryManagement {
  public class Home: Window {

    private Fixed container;

    public Home(): base("Home") {
      this.SetDefaultSize(800, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);

      this.container = new Fixed();
      this.Add(this.container);

      new AppMenu(this, this.container);

      this.ShowAll();
    }

    private void Exit(object obj, DeleteEventArgs args){
      Application.Quit();
    }
  }
}