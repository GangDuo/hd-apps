(function(args) {
  var isSuccess = true,
      ExcelApp,
      book,
      sheet;

  try {
    ExcelApp = WScript.CreateObject("Excel.Application");
    ExcelApp.DisplayAlerts = false;
    ExcelApp.Visible = false;
    book = ExcelApp.Workbooks.Open(args[0]);
    sheet = book.Worksheets(args[1]);
    sheet.Rows("1:1").Delete();
    book.Save();
    book.Close();
    ExcelApp.Quit();
    ExcelApp = null;
  } catch(e) {
    WScript.Echo(e.message);
    isSuccess = false;
  }
  WScript.StdOut.WriteLine(isSuccess);
})((function() {
  var i,
      argv = [];
  for (i = 0; i < WScript.Arguments.length; i++) {
    argv.push(WScript.Arguments.Item(i));
  }
  return argv;
})());
