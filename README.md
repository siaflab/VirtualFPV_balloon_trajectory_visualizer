VirtualFPV - Space Moere Project balloon trajectory realtime visualiser / simulator / log viewer
===

## はじめに
　920tracker - Space Moere Module Real-Time Tracking Systemは、Space Moere気球
軌跡データを3D空間に描画するソフトウェアです。テレコーディングパフォーマンス時の
ビジュアライズ、追跡、展示などの用途に使用することを想定しています。

## 動作環境
　Mac OS X、Windows 64bitで動作します。高速なGPU搭載が推奨環境です。
　リアルタイムデータを受信するには、別途 gps920bot プログラムが必要になります。

## 機能
・軌跡リアルタイム表示
　OSCで受信した位置情報を3D空間上にプロットします

・シミュレーション軌跡表示
　実行ファイルと同じフォルダにシミュレータ(CUSF Landing Predictor http://predict.habhub.org/ )
　のCSVデータをflight_path.csvのファイル名で置いておくと、リアルタイム軌跡とは別に緑の線で軌跡を
　描画します。
　Dataメニュー → Simulation

・デモ機能
　初回の打ち上げ実験の軌跡をデモ展示用に無限ループで表示します。
　Dataメニュー → Run Demo

・表示切替とクレジット表示
　Dataメニュー → Setting
　・気球のラベル表示　(ショートカットB)
　・ランドマークのラベル表示　(ショートカットL)
　・マップの3Dポリゴン表示　(ショートカットD)　※追跡班用、OFFにするとバッテリー消費が少なくなるかも
　・衛星写真地図／標準地図切り替え　(ショートカットM)　※追跡班用、標準地図の方が追跡しやすいかも
　・サウンドON/OFF　(ショートカットS)

## OSC Interface
　・Port Number
　　　32000
　・realtime trajectory visualize command
　　　/data "1,1,-42,03:30:00,43.126652,141.430371,6M,\x00\r\n"
　・reset command
　　　/reset
　・simulation trajectory command
　　　/sim "1503342000,43.1221,141.426,62"
　・clear simulation command
　　　/simclear

## LICENSE
TBD



