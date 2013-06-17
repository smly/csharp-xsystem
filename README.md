# csharp-xsystem

えいえんにわーくいんぷろぐれす


## Note

xsystem35 は古くていろいろ動かないので、触るところメモ:

* src/count.h の include guard `__COUNT__` はコンパイラのマクロと被るので `__COUNTER_H__` に.
* ft2build.h をインクルードする必要がある
* WM_CLASS が定義されていないので tiling window manager を使う場合は不便
  * patch: https://gist.github.com/smly/5787574
* ALSA PCM API は 0.9.x から 1.0.x にかけて変更されている (kernel 2.6.4 は ALSA 1.0)
  * xsystem35 は 0.5 series と 0.9 series しかサポートしていないので最近のカーネルだと要修正