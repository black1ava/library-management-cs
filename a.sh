#! bin/bash

mcs -pkg:gtk-sharp-2.0 main.cs account/login.cs length/length.cs home.cs menu/appMenu.cs librarian/new.cs librarian/form.cs database/connection.cs librarian/show.cs librarian/edit.cs account/account.cs author/show.cs author/new.cs author/form.cs -r:System.Data.dll -r:System.Data.OracleClient.dll && mono main.exe