[+]在 Scene 中可以序列化保存，但是在 Prefab 中无法保存
[+]派生类的序列化


序列化无法序列化派生类
将 ScriptableObject 保存在 Prefab 中，只能通过创建 asset 文件，然后引用的方式关联到 Prefab 上
序列化 ScriptableObject 类的名字要和文件的名字一样，否则 Unity 找不到
添加 ScriptableObject 到 ScriptableObject 要使用 AssetsDatabase.AddObjectToAsset
添加 ScriptableObject 到 Monobehavior 不需要

// 另外一种方法是参考 Behavior Tree
// 将编辑后的 json 转换为 string 保存在 Behavior Tree (派生自 MonoBehavior)



https://blogs.unity3d.com/2014/06/24/serialization-in-unity/
https://forum.unity.com/threads/saving-instances-of-scriptable-objects-to-prefabs.56947/
https://zhuanlan.zhihu.com/p/37782807
https://docs.unity3d.com/Manual/script-Serialization.html#ClassSerialized
https://forum.unity.com/threads/editor-tool-better-scriptableobject-inspector-editing.484393/
https://forum.unity.com/threads/best-practice-questions-scriptable-objects-and-prefabs-reference.511021/
https://forum.unity.com/threads/referencing-prefabs-in-a-scriptableobject.717881/
https://answers.unity.com/questions/1249485/scriptable-object-references-to-prefabs.html


Serializable 不支持派生的序列化，只能用 ScriptableObject 替代
https://answers.unity.com/questions/245604/does-unity-serialization-support-inheritence.html?_ga=2.5187123.1642642325.1607938770-656484685.1581916478