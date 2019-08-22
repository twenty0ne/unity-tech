// NOTE

// UI Assets
// https://assetstore.unity.com/packages/2d/gui/fantasy-wooden-gui-free-103811

// 原则：
// 由于 Prefab 的不便查看，尽量少修改 Prefab，多用代码实现
// 一个页面在功能基本不变的情况下，需要保证美术策划能方便修改，而程序基本不用参与
// 已实现功能应该能够方便重用
// 尽量不使用 Unity 2018 prefab 嵌套的功能，使其更加通用

// TODO
// 堆栈管理
// 内存管理
// 最大限度资源复用
// 资源清理
// UI 间消息传递
// 优化：减少 batch，内存，加载速度（长列表）
// Navigation
// 网络更新，数据刷新
// 粒子
// 动画
// 不要使用 SetActive, 会引起重绘，最好办法是移除屏幕
// Console
// 最好不要使用 Alpha = 0 的作为输入的遮挡层，尝试用 Canvas block 来处理
// 热度图：用于显示 Over Drawing
// Draw call 合并提示：相同图片和Material
// Config, Data 获取频繁的可尝试做成通用组件，方便策划配置，而程序更关注复杂UI逻辑
// UIEventCompant 在 Awake 上面注册 UI Event 监听，这样就能将一部分代码移动到修改 prefab？
// ScrollView/List 动态加载，惰性加载（指先打开页面，然后一个个的异步加载）
// 多语言
// 自动换行，自动缩放文本
// 当页面被清理之后，需要有恢复页面的功能，比如切换到指定的 tab
// dyanmic batch - 比如背包 Widget 格子的面板，应该自动 batch
// UI panel 下面元素分层， Static, Dynamic, Static+Dynamic（偶尔变化） 根据修改的强度划分
// 适配 - 匹配大多数手机的分辨率

// OpenDialog 如何指定使用哪个 Canvas, 是否添加一个 int canvasOrder 参数
// UIRoot 默认3个 Canvas： -1，0， 1
// 如果再添加新的，只需要设置一个新的 order
// 这样的好处是自由

// DIALOG
// Dialog 关闭是会从堆栈中清楚的，是否从缓存中清除？

// UI 逻辑复用
// 最好的办法就是将与 UI 界面元素无关的代码提炼到单独的 static 类，如 GameHelperUI

// 类似 MenuLogin 这种，之使用一次，可以直接清理掉
// 所有页面的控制除了自动的倒计时

// TODO:
// Q:如何解决 美术/策划 修改 prefab 之后，原来的引用不存在
// 解决程序修改名字之后
// A:最简便的是添加 引用 检测工具，检测到 prefab 里面有 null，报错
// 1.不要在代码里面使用 transform.Find 这种，否则修改之后很容易找不到，尽量采用拖动的方式
// 编辑保存 Prefab 的时候，进行提示

// Q:如何解决多个 Dialog 重叠的时候，半透明背景重叠
// A:

// Q:UI 传递消息
// A:第一种是初始化的时候传递，另外一种情况是通过 UIEvent

// 来自 <2018腾讯移动游戏技术评审标准与实践>
// 不同 Canvas 之间不能合并 batch

// Q:如何区分哪些 dialog 是要缓存的

// Q:是否可以做成类似 bolt 插件一样拖拽的形式
https://assetstore.unity.com/packages/tools/visual-scripting/bolt-87491

// TODO
[+] DOTWeen
[+] Console 添加 ':' 执行命令，'/' 搜索 log
[+] Console 最后采用纯代码生成，这样就能脱离 UI 框架，也能在其他项目中使用
[+] 确保资源释放，无内存泄漏
[+] 使用 LRU 原则
[+] 数据绑定
https://github.com/Real-Serious-Games/Unity-Weld
https://github.com/sigtrapgames/SmartData
[+] DrawCall 优化  
[+] OverDraw 优化  
[+] 自动

// 测试例子
[+] 同时打开多个 Dialog，应该按照顺序依次弹出
[+] 最顶层的系统 Dialog 应该是覆盖在所有界面之上
[+] Menu A 弹出 Dialog B, 通过 Dialog B 打开 Menu C，Menu C 返回的时候 Dialog B 应该是关闭的
[+] Navigation 循环跳转，A -> Nav -> B -> Nav -> C -> Nav -> A, 在点击返回
[+] 滚动栏，重复利用已分配 Item
[+] A(卡片01信息) 界面打开 Nav，跳到删除弹窗，弹窗删除01，回来应该关闭 A 界面，回到 A 上一个界面