# csharp-xsystem

WIP. forever.

## Screenshot

![screenshot](http://github.com/smly/csharp-xsystem/raw/master/ss.png)

````
$ make
$ mono src/xsystem3.exe tt2/toushin2_ga.ald
$ convert output.{ppm,png}
$ display output.png
````

## Notes for developers

xsystem35 の以下の箇所は新しい環境だとコンパイルできない:

* src/count.h の include guard `__COUNT__` はコンパイラのマクロと被るので `__COUNTER_H__` に.
* フォント関連のコードで ft2build.h をインクルードする必要がある
* WM_CLASS が定義されていないので tiling window manager を使う場合は定義しないと workspace 変更時に落ちる
  * https://gist.github.com/smly/5787574 を適用して xmonad で float 指定する
* ALSA PCM API は 0.9.x から 1.0.x にかけて変更されている (kernel 2.6.4 は ALSA 1.0)
  * xsystem35 は 0.5 series と 0.9 series しかサポートしていないので最近のカーネルだと要修正