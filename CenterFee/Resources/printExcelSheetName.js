(function (args) {
  var ExcelApp = null,
      book = null,
      i;

  try {
    ExcelApp = WScript.CreateObject("Excel.Application");
    ExcelApp.Visible = false;
    ExcelApp.DisplayAlerts = false;
    book = ExcelApp.Workbooks.Open(args[0]);
    for (i = 1; i <= book.Worksheets.Count; i++) {
      WScript.StdOut.WriteLine(book.Worksheets(i).Name);
    }
  } catch (e) {
    WScript.Echo(e.message);
  } finally {
    if(null !== book) {
      book.Close();
    }
    if(null !== ExcelApp) {
      ExcelApp.Quit();
    }
  }

})((function () {
  var i,
      argv = [];
  for (i = 0; i < WScript.Arguments.length; i++) {
    argv.push(WScript.Arguments.Item(i));
  }
  return argv;
})());
