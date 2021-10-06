# MustlePassthrough
Questのパススルー機能を使い、筋トレをカウントする。
筋トレ種目はSquat, PushUp, Superman, Plank, Cycling, Crunch, NeatChest, SideVent, SidePlankの8種類

# DEMO

https://user-images.githubusercontent.com/62923803/135836727-ab7a44e6-3ed3-422d-9c4d-5ade237177c9.mp4

# Features
- VRで腕立て伏せなどの筋トレを行うのは危険だが、パススルー機能によって可能になったため実装した。
- MVRPパターン, zenject, photonなどを学習しながら作成した。
 
# Requirement
使用ライブラリ
- [UniRx](https://github.com/neuecc/UniRx)
- [UniTask](https://github.com/Cysharp/UniTask)

使用アセット
- [photon](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922)
- [photon voice](https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518)
- [extenject](https://assetstore.unity.com/packages/tools/utilities/extenject-dependency-injection-ioc-157735?locale=ja-JP)
- [jellyfish_Aurelia(有料)](https://assetstore.unity.com/packages/3d/characters/animals/fish/jellyfish-aurelia-103073)
- [MobilePostProcess(有料)](https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/fast-mobile-post-processing-color-correction-lut-blur-bloom-urp--129638)
 
# Note
MVRPパターンとはUniRxを用いたMVPパターンのことです。
下記のようにModel, Presenter, Viewを作成しました。
その際、Zenject(extenject)を用いて依存性を管理し、ModelとPresenterはMonoBehaviourの継承をなくしました。
- [Model](https://github.com/TomohiroHibino600/MustlePassthrough/blob/main/Assets/Scripts/Model/Model.cs)
- [Presenter(Modelへの入力)](https://github.com/TomohiroHibino600/MustlePassthrough/blob/main/Assets/Scripts/Presenter/PresenterInput.cs)
- [Presenter(ModelからViewへの出力)](https://github.com/TomohiroHibino600/MustlePassthrough/blob/main/Assets/Scripts/Presenter/PresenterOutput.cs)
- [View](https://github.com/TomohiroHibino600/MustlePassthrough/tree/main/Assets/Scripts/View)

将来的には筋トレデータを売買し、腹筋を割るのに効率のいい筋トレ速度とか各パーツを置くべき位置・筋トレ頻度が分かるようにもしてみたいと思っています。
 
# Author
* 作成者 Tomohiro Hibino
* twitter https://twitter.com/33bWy
 
# License
"MustlePassthrough" is under [MIT license](https://en.wikipedia.org/wiki/MIT_License).

