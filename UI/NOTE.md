参考 flutter 结构

UIWidget
所有的物品都是从 UIWidget 派生

配色：
http://www.ruanyifeng.com/blog/2019/03/coloring-scheme.html
文字：1a2a3a
背景：aaaaaa

分辨率：
1080 x 1920
720 x 1280

https://docs.unity3d.com/Manual/HOWTO-UIMultiResolution.html

编辑 UI 时，最好设置为 Pivot 和 Local 模式
https://connect.unity.com/doc/Manual/UIAutoLayout
> First minimum sizes are allocated.
> If there is sufficient available space, preferred sizes are allocated.
> If there is additional available space, flexible size is allocated.
任何带有矩形变换的游戏对象都可以作为布局元素

自动布局系统首先计算宽度，然后计算高度。因此，计算的高度可取决于宽度，但计算的宽度决不能取决于高度

记住：MVVM 或者 MVC 这种框架只适合复杂的 UI，如果是简单的直接放在采用 VC 合并在一个页面逻辑里面
使用 MVVM 绑定数据
MVC 

优化建议：
https://www.youtube.com/watch?v=_wxitgdx-UI
https://create.unity3d.com/Unity-UI-optimization-tips
