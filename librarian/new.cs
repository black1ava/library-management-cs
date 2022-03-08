using System;
using Gtk;
using Pango;
using static LibraryManagement.LibrarianForm;
using static LibraryManagement.AppMenu;

namespace LibraryManagement {
  public class NewLibrarian: Window {

    private Fixed container;
    private Label header;
    private Button createButton;
    private LibrarianForm form;

    public NewLibrarian(): base("New librarian") {
      this.SetDefaultSize(800, 800);
      this.DeleteEvent += new DeleteEventHandler(this.Exit);

      this.container = new Fixed();
      this.Add(this.container);

      new AppMenu(this, this.container);

      this.header = new Label("New Librarian");
      FontDescription fontDescription = new FontDescription();
      fontDescription.Weight = Weight.Bold;
      fontDescription.Size = 20 * 1024;
      this.header.ModifyFont(fontDescription);
      this.container.Put(this.header, 20, 30);

      this.form = new LibrarianForm(this, this.container);
      
      this.createButton = new Button("Create");
      this.createButton.WidthRequest = Length.ButtonLength;

      this.form.setUpButton(this.createButton);

      this.ShowAll();
    }

    private void Exit(object obj, DeleteEventArgs args){
      Application.Quit();
    }
  }
}