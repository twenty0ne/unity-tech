// NOTE

// TODO
// 堆栈管理
// 内存管理
// 最大限度资源复用
// 资源清理
// UI 间消息传递
// 优化：减少 batch，内存，加载速度（长列表）
// Navigation

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

// Q:如何解决多个 Dialog 重叠的时候，半透明背景重叠
// A: