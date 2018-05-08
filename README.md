# bca - Binary Chroma Animation

BCA is a file format for time/frame based animations for Razer Chroma enabled devices.

A `.bca` file contains data for each frame wrapped in a package containing:

- a frame header
- a device header for each device in this frame
- color data with row/column asignment for the specific device

> For reading and playing these animations there is a C# project which uses [Colore](https://github.com/CoraleStudios/Colore).

# File Structure

### File Header
Offset | Bytes | Datatype | Name | Description
--- | --- | --- | --- | ---
0x00 | 2 | uint16 | fType | ASCII Chars 'BA', 0x4241
0x02 | 4 | uint32 | fSize | Whole file size
0x06 | 4 | uint32 | fReserved | 
0x0A | 4 | uint32 | fBcaOffset | Start of BCA content after file header

### BCA Header
Offset | Bytes | Datatype | Name | Description
--- | --- | --- | --- | ---
0x0E | 4 | uint32 | hSize | BCA header size
0x12 | 2 | uint16 | hVersion | BCA file version = 1
0x14 | 4 | uint32 | hFrameOffset | Offset to first frame
0x18 | 2 | uint16 | hFPS | Frames per second
0x1A | 4 | uint32 | hFrameCount | Number of total frames
0x1E | 2 | uint16 | hReserved | 

### Frame Header
Offset | Bytes | Datatype | Name | Description
--- | --- | --- | --- | ---
0x20 | 2 | uint16 | fhSize | Frame header size
0x22 | 2 | uint16 | fhDeviceCount | Number of devices in this frame
0x24 | 2 | uint16 | fhDataSize | Size of all devices and their data in this frame 

### Device Header
Offset | Bytes | Datatype | Name | Description
--- | --- | --- | --- | ---
0x26 | 1 | uint8 | dhSize | Device header size
0x27 | 1 | enum | dhDataType | 0-Assign color to all, 1-Assing color to row/col
0x28 | 2 | enum | dhDevice | None, Keyboard, Keypad, Mouse, Mousepad, Headset
0x30 | 2 | uint16 | dhDataSize | Size of data contained in this frame for this device 

### Device Data
Offset | Bytes | Datatype | Name | Description
--- | --- | --- | --- | ---
0x32 | 1 | uint8 | dRow | 0 on devices with only 1 row
0x33 | 1 | uint8 | dColumn | Column
0x34 | 4 | uint32 | dABGR | RGBa

# License
Copyright © 2016 - 2018 Chroma Developer Community. Created by [@blackworx](https://github.com/blackworx) & [@wolfspiritm](https://github.com/wolfspiritm).

This project is licensed under the MIT license, please see the file [LICENSE](https://github.com/blackworx/bca/blob/master/LICENSE) for more information.

Razer is a trademark and/or a registered trademark of Razer USA Ltd.
 All other trademarks are property of their respective owners.
