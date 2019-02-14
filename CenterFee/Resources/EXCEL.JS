(function(args) {
  var ExcelApp;

  try {
    ExcelApp = WScript.CreateObject("Excel.Application");
    ExcelApp.Visible = true;
    WScript.Echo(args[0]);
    ExcelApp.Workbooks.Open(args[0]);
  } catch(e) {
    WScript.Echo(e.message);
  }

})((function() {
  var i,
      argv = [];
  for (i = 0; i < WScript.Arguments.length; i++) {
    argv.push(WScript.Arguments.Item(i));
  }
  return argv;
})());
