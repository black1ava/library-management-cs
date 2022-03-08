#! bin/bash

mcs -pkg:gtk-sharp-2.0 main.cs account/login.cs length/length.cs home.cs menu/appMenu.cs librarian/new.cs librarian/form.cs database/connection.cs -r:System.Data.dll -r:System.Data.OracleClient.dll && mono main.exe