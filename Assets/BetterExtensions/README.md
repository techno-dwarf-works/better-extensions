# Better Extensions
[![openupm](https://img.shields.io/npm/v/com.uurha.betterextensions?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.uurha.betterextensions/)

This package provides useful extension.

## Extensions:
1. AudioClipExtensions
    1. `AudioClip.FromByteArray/Async`
    2. `AudioClip.ToByteArray/Async`
    3. `AudioClip.Trim/Async`
    4. `AudioClip.Amplify/Async`
2. SerializeExtensions
    1. `Serialize/Async`
    2. `Deserialize/Async`
    3. `Compress/Async`
    4. `Decompress/Async`
3. StringExtensions
4. UIExtension
    1. `CanvasGroup.SetActive`
    2. Custom Components
        1. `LinkOpener`
5. TaskExtensions
    1. `TaskExtensions.WaitUntil`
    2. `TaskExtensions.WaitWhile`
    3. `TaskExtensions.CreateTask`
6. Vector3Math
7. ExtensionMethods
    1. Adds awaiter to following types:
        1. UnityWebRequestAsyncOperation
            1. `await webRequest.SendRequest();`
        2. AsyncOperation
        3. AssetBundleRequest

## Wrappers:
1. DownloadHandlerFile

## Install
Project Settings -> Package Manager -> Scoped registries
</br>

![image](https://user-images.githubusercontent.com/22265817/197618796-e4f99403-e119-4f35-8320-b233696496d9.png)

```json
"scopedRegistries": [
    {
      "name": "Arcueid Plugins",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.uurha"
      ]
    }
  ]
```

Window -> PackageManager -> Packages: My Registries -> Arcueid Plugins -> Better Extensions