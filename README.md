# TinyTweener for Unity
I wrote this class because i needed something like iTween or DOTween
but without installing any external library for my project.
TinyTweener it's a quick and fast general utility to move objects and
change values with absolute precise transitions. It also offer many options
of use for different conditions.
# Example of usage.

Attach it to a GameObject and call the function fron any script.
```
MoveTo(this.transform, _target.transform.position, _target.transform.rotation, 10f);
```

Don't forget to use the namespace:
```
using LS;
```
