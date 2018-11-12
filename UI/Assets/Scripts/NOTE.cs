// NOTE

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
