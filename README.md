# bca
BCA (.bca, Binary Chroma Animation) is a file format for time/frame based animations for Razer Chroma enabled devices.

A .bca file contains data for each frame wrapped in a package containing 
+ a frame header
+ a device header for each device in this frame
+ color data with row/column asignment for the specific device

See Wiki for more information and a detailed file structure.

For reading and playing these animations there is a C# project which uses [Colore](https://github.com/CoraleStudios/Colore).


# License
Copyright © 2016 by [@blackworx](https://github.com/blackworx) & [@wolfspiritm](https://github.com/wolfspiritm)

This project is licensed under the MIT license, please see the file [LICENSE](https://github.com/blackworx/bca/blob/master/LICENSE) for more information.

Razer is a trademark and/or a registered trademark of Razer USA Ltd.
 All other trademarks are property of their respective owners.
