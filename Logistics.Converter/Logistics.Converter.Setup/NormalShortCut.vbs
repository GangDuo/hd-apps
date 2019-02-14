Option Explicit

Const msiOpenDatabaseModeTransact = 1

Dim msiPath : msiPath = Wscript.Arguments(0)

Dim installer
Set installer = Wscript.CreateObject("WindowsInstaller.Installer")
Dim database
Set database = installer.OpenDatabase(msiPath, msiOpenDatabaseModeTransact)

database.OpenView("INSERT INTO Property(Property, Value) VALUES('DISABLEADVTSHORTCUTS', '1')").Execute
database.OpenView ("UPDATE Shortcut SET Icon_ = ''").Execute

database.Commit