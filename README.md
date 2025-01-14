# BepInCharacterSwapper
 A model swapper for DesktopMate built for BepInEx.

 ## Installation
 - Download the [latest v6 release](https://builds.bepinex.dev/projects/bepinex_be/733/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.733%2B995f049.zip) of BepInEx. (`BepInEx-Unity.Il2CPP-win-x64-6.0.0`)
 - Extract the .zip into `steamapps\common\Desktop Mate`
 - Download the latest version of the mod .dll from the releases section.
 - Place the .dll in `steamapps\common\Desktop Mate\BepInEx\plugins`.
 - (Optional) Create a `vrm` folder in the `steamapps\common\Desktop Mate\` folder. If you do not make one, the mod will make one for you.

## Usage
- Place all of your `.vrm` files into the aforementioned `vrm` folder.
- Upon launching Desktop Mate, the mod will automatically load your `.vrm` files.
- Right click on the default character and navigate to the `Change Character` dialogue. You should see button(s) with the name of your models.
- Click on the button to load the model as normal.

![preview](https://cdn.discordapp.com/attachments/343113240143986709/1328204316137226250/image.png?ex=6785da29&is=678488a9&hm=294b679aa54a290a4b05567dcd203c926d648a2c27459bba03813cd2bc4696f3&)

## Known Issues
- If you delete or add a `.vrm` while Desktop Mate is running, the mod will (currently) not detect this change. If you add or remove any models you will have to restart Desktop Mate.
- Due to a bug in Desktop Mate, you may see spam in the console. Navigate to `Bepinex/config/BepInEx.cfg` and set `UnityLogListening = false` to hide these messages.

## Planned Changes
- Per-model settings
- Detect added/removed models at runtime
- Allow customization of buttons

## External Libraries
This mod uses a slightly modified version of the VRMLoader and CharacterLoader from [YusufOzmen01/desktopmate-custom-avatar-loader](https://github.com/YusufOzmen01/desktopmate-custom-avatar-loader). Licensing can be found below:

Copyright 2025 Yusuf Çınar "SergioMarquina" Özmen

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
