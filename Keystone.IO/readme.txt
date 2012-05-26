Keystone.IO

概要
====

3D 関連データの読み込みおよび書き出しのためのライブラリ
for .NET Framework 4.0 Client Profile
その他のライブラリは要求しません。


対応形式
=======

Metasequoia
-----------

名前空間: Linearstar.Keystone.IO.Metasequoia

* .mqo, .mqm
  メタセコイア オブジェクト
  MqDocument


Elfreina
--------

名前空間: Linearstar.Keystone.IO.Elfreina

* .elem
  エルフレイナ拡張モデルファイル
  ElDocument


MikuMikuDance
-------------

名前空間: Linearstar.Keystone.IO.MikuMikuDance

* .xx
  XX file
  XxDocument
* .osm
  One Skin Model file
  OsmDocument
* .pmd
  Polygon Model Data file
  PmdDocument
* .pmx
  拡張モデルファイル
  PmxDocument
* .vmd
  Vocaloid Motion Data file
  VmdDocument
* .vpd
  Vocaloid Pose Data file
  VpdDocument
* .vac
  Vocaloid Accessory Connection file
  VacDocument


MikuMikuMoving
--------------

名前空間: Linearstar.Keystone.IO.MikuMikuMoving

* .mvd
  Motion Vector Data file
  MvdDocument


使用方法
=======

MqDocument や ElDocument の Parse メソッドにより既存のデータからインスタンスが得られます。Parse せずに新規で全部作ってもおそらく動きます。
編集したインスタンスはテキストデータは GetFormattedText メソッド、バイナリデータは Write メソッドより出力できます。

サンプルとしては以下のようなものがあります。

* MMDVer3 以降の .vmd を MMDVer2 の .vmd に変換する
  https://gist.github.com/2656910