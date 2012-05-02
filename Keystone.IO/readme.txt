Keystone.IO

概要
====

3D 関連データの読み込みおよび書き出しのためのライブラリ
for .NET Framework 4.0 Client Profile
その他のライブラリは要求しません。

以下の形式に対応:

* .mqo
  Metasequoia: メタセコイア オブジェクト
  Linearstar.Keystone.IO.Metasequoia.MqDocument
* .elem
  Eflreina: エルフレイナ拡張モデルファイル
  Linearstar.Keystone.IO.Elfreina.ElDocument
* .mvd
  MikuMikuMoving: Motion Vector Data file
  Linearstar.Keystone.IO.MikuMikuMoving.MvdDocument


使用方法
=======

MqDocument や ElDocument の Parse メソッドにより既存のデータからインスタンスが得られます。Parse せずに新規で全部作ってもおそらく動きます。
編集したインスタンスはテキストデータは GetFormattedText メソッド、バイナリデータは Write メソッドより出力できます。