### TipCalculator

#### 一：项目介绍

根据视频内容，重构了一下他的布局

#### 二：发现几个问题

1. 国外的format ` {0:C}`后会自动添加$,而国内则添加¥
2. windows phone控件生成代码和逻辑代码初始化顺序和wpf初始化顺序不同。比较好的实现是用`placeholder`设置提示值

#### 三：项目截图

![tipC](pic/tipC.jpg)