using System;
using Gtk;
using Pango;
using static LibraryManagement.AppMenu;
using static LibraryManagement.BookTypeForm;

namespace LibraryManagement {
  public class NewBookType: Window {

    private Fixed container;
    private Label header;

    public NewBookType(): base("New book type") {
      this.SetDefaultSize(800, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);
      
      this.container = new Fixed();
      this.Add(this.container);

      new AppMenu(this, this.container);

      this.header = new Label("New book type");
      FontDescription fontDescription = new FontDescription();
      fontDescription.Weight = Weight.Bold;
      fontDescription.Size = 1024 * 20;
      this.header.ModifyFont(fontDescription);
      this.container.Put(this.header, 20, 30);

      new BookTypeForm(this, this.container);

      this.ShowAll();
    }

    private void Exit(object obj, DeleteEventArgs args){
      Application.Quit();
    }
  }
}