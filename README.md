# CenterFee

## 概要

買掛金支払予定一覧表から取引先毎に差引明細書を作成し、金額を転記する。

> 【買掛金支払予定一覧表】
>
> エクセルで作成した、取引先毎に買掛金額が記載された一覧
>
> 【差引明細書】
>
> 買掛金額と各種手数料、最終的な支払い金額を記載したエクセル

## ビルド

設定ファイルを作成します。

```xml:global.config
<?xml version="1.0" encoding="utf-8"?>
<appSettings>
    <add key="folder:destination" value="保存先フォルダ絶対パス" />
</appSettings>
```

folder:destinationが存在しなければ、実行ファイルと同じフォルダ。

# JFront

## 概要

店舗着日から投入表を検索できます。

## 使用方法

1. JFront.exeを実行する。
2. 設定ボタンをクリックする。
3. サインインURL、アカウントを入力し、OKボタンをクリックする。

# Logistics.Converter

## 概要

WMSから出力されたCSVをエクセル定型フォーマットへ変換する。

## ビルド

設定ファイルを作成します。

```xml:global.config
<?xml version="1.0" encoding="utf-8"?>
<appSettings>
    <add key="db:server" value="" />
    <add key="db:user" value="" />
    <add key="db:password" value="" />
    <add key="db:port" value="" />
    <add key="db:database" value= ""/>
</appSettings>
```

