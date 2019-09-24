# Priests and Devils v2 Unity Project

Can you move all the three priests and three devils to the other side of river without leaving more devils than priests on any side at anytime?

## SSAction

In this version, the game uses `SSAction` to control movements of characters and boats and a `Judge` class to inform the controller of the end of the game additionally.

## Objects

- Boat
- Priests
- Devils

## Actions

|  Action   | Description  |
|  ----  | ----  |
| Move Boat  | Move the boat from one side to the other side if any passengers aboard |
| Priest To Boat  | Get one priest on the same side of the boat aboard |
| Priest To Bank  | Get one priest on the same side of the boat off |
| Devil To Boat  | Get one devil on the same side of the boat aboard |
| Devil To Bank  | Get one devil on the same side of the boat off |
| Restart  | Reset the game to initial state|

Note that the boat is capable of only two passengers.

You can watch demo video here: [https://www.bilibili.com/video/av68864097](https://www.bilibili.com/video/av68864097)