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