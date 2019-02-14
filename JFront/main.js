if (!String.prototype.startsWith) {
  String.prototype.startsWith = function(searchString, position){
    position = position || 0;
    return this.substr(position, searchString.length) === searchString;
  };
}

function getPageTitle(browser) {
  try {
    return browser.Document.getElementsByTagName('title')[0].innerHTML
  } catch(e) {}
  return ''
}

function ie_DocumentComplete(browser, url) {
  //WScript.Echo("DocumentComplete: " + url + ' ' + getPageTitle(browser));
    if (url === getEndPoint('')) {
  } else if (url.startsWith(getEndPoint('/JMODE_ASP/faces/index.jsp?time='))) {
    onLogin(browser);
  } else if (url === getEndPoint('/JMODE_ASP/faces/index.jsp')) {
    switch(getPageTitle(browser)) {
      case 'メインメニュー': return onMainMenu(browser)
    }
  } else if (url === getEndPoint('/JMODE_ASP/faces/contents/index.jsp')) {
    onSerchDist(browser)
  }
}

function onLogin(browser) {
  var CLIENT = WScript.Arguments.Named("organization-code"),
      PERSON = WScript.Arguments.Named("user-code"),
      CLPASS = WScript.Arguments.Named("organization-pass"),
      PSPASS = WScript.Arguments.Named("user-pass"),
      id_client = "form1:username",
      id_person = "form1:person_code",
      id_clpass = "form1:password",
      id_pspass = "form1:person_password";

  if(!$(id_client)) {
    id_client = "form1:client";
    id_person = "form1:person";
    id_clpass = "form1:clpass";
    id_pspass = "form1:pspass";
  }
  $(id_client).value = CLIENT;
  $(id_person).value = PERSON;
  $(id_clpass).value = CLPASS;
  $(id_pspass).value = PSPASS;
  $("form1:login").click();

  function $(id) {
    return browser.Document.getElementById(id);
  }
}

function getBytes(text) {
  var bytes,
      stream = WScript.CreateObject("ADODB.Stream")
  stream.Open()
  stream.Charset = "UTF-8"
  stream.WriteText(text)
  stream.Position = 0
  stream.Type = 1 // binary
  stream.Position = 3 // skip BOM
  bytes = stream.Read()
  stream.Close()
  return bytes
}

function onMainMenu(browser) {
  var headers = 'Content-Type: application/x-www-form-urlencoded\r\n'
  var data = 'form1%3Aredirect=%E5%85%A5%E5%8A%9B&form1%3Aredirectpage=X018_SELECT.jsp&form1%3Aredirectfolder=X018_160_DISTRIBUTE&form1%3Areturnvalue=SUCCESS&form1%3Afunctype=10&form1%3AselectMenu=004&form1%3AselectSubMenu=007&form1%3AselectFunction=X018&form1%3Afop=&form1=form1'
  browser.Navigate(getEndPoint('/JMODE_ASP/faces/contents/index.jsp'),
      '_self', null, getBytes(data), headers)
}

function onSerchDist(browser) {
  var headers = 'Content-Type: application/x-www-form-urlencoded\r\n',
      today = new Date(),
      year = WScript.Arguments.Named("year") || today.getFullYear(),
      month = WScript.Arguments.Named("month") || today.getMonth() + 1,
      day = WScript.Arguments.Named("day") || today.getDate(),
      lineCode = WScript.Arguments.Named("line") || '',
      data = 'form1%3Aexecute=execute&form1%3Aaction=search&form1%3AisAjaxMode=&distribute_cd_from=&distribute_cd_to=&modify_date_from=&modify_date_to=&payment_date_from=&payment_date_to=&wh_sche_date_from=&wh_sche_date_to=&shop_sche_date_from=' + year + '%E5%B9%B4' + month + '%E6%9C%88' + day + '%E6%97%A5&shop_sche_date_to=&sup_cd=&line_cd=' + lineCode + '&catg_gp=&season_cd=&brand_cd=&style_cd=&dest_cd=&status=02&dest%3Adest=&dest%3AdestName=&dest%3Acust=&dest%3Aarea=&dest%3AdestClass=&dest%3AdestType=&dest%3AdestGroup=&form1=form1'

  browser.Navigate(getEndPoint('/JMODE_ASP/faces/contents/X018_160_DISTRIBUTE/X018_SELECT.jsp'),
      '_self', null, getBytes(data), headers)
  //browser.Visible = true;
  WScript.Quit()
}

function combine(s1, s2, separator) {
  var regexp = new RegExp('^' + separator + '+|' + separator + '+$', 'g');
  return s1.replace(regexp, '') + separator + s2.replace(regexp, '');
}

function getEndPoint(relativePath) {
  return combine(WScript.Arguments.Named("base-url"), relativePath, '/');
}

// IE起動
var ie = WScript.CreateObject("InternetExplorer.Application", "ie_");
ie.Visible = true
ie.Navigate(getEndPoint(""));
while(true) {
  WScript.Sleep(100);
}