# SetWindowVisible
一个设置可视窗口可视化状态（隐藏、显示、最大化、最小化等）的控制台程序。

# Usage

查看用法，命令行下输入'SetWindowVisible /?'

```
设置窗体程序的可视化状态。

SetWindowVisible /a
SetWindowVisible /h
SetWindowVisible PID [State]

    /a         显示所有进程信息。
    /h         显示已隐藏窗口的信息。
    PID        指定进程的PID值。
    State      窗体的状态值(0-11),此参数缺省时默认为0。

    State值对应的窗体状态如下:

        0    -   Hide
        1    -   ShowNormal
        2    -   ShowMinimized
        3    -   ShowMaximized
        4    -   Maximize
        5    -   ShowNormalNoActivate
        6    -   Show
        7    -   Minimize
        8    -   ShowMinNoActivate
        9    -   ShowNoActivate
        10   -   Restore
        11   -   ShowDefault
        12   -   ForceMinimized

    注：当参数为空时，默认显示可视化窗体程序列表。
```
