# ASCII HEART
Projects a Rotating 3D heart onto the Terminal screen and plots the "pixels" using ASCII art characters - ".,-~:;=!*#$@". These characters correspond to different values of illumination at the point, ranging from dimmest to brightest. There is no raytracing or heavy graphical computations required to execute it. At its core, it’s a framebuffer and a Z-buffer into which I render pixels. Since it’s just rendering relatively low-resolution ASCII art, I massively cheat.


## How the program works?
It checks for the condintion of a 3D equation for a lot of X,Y and Zs and it adds all the points satisfying the equation into a collection. On each frame, the points are multiplied with Rotation matrices to get the updated values of them after rotating. It then plots the updated values of these points on the 2D terminal screen using perspective rendering. We only need to print those pixels which correspond to the nearest point of the object to the screen. This is done by keeping track of the depths in a Z-Buffer. 

Printing the character corresponding to a point is a whole different story. We need to calculate the unit normal vector of the equation at that point and take it's dot product with unit Light vector. The value of this dot product will tell us which is the most suitable character for the point's illumination.

## Related Links
[Lex Fridman's Video](https://youtu.be/DEqXNfs_HhY)

[Andy Sloane's Blog Post](https://www.a1k0n.net/2011/07/20/donut-math.html)
