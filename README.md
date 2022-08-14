# Better Extensions

## Install
Window -> PackageManager -> Add package from GIT url...
```
https://github.com/uurha/BetterExtensions.git#upm
```

This package provides useful extension. Such as:

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
