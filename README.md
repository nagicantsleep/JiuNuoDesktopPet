# JiuNuo Desktop Pet

A WPF desktop pet that lives on Windows windows and the taskbar. Nuonuo treats
deleted desktop items as food, while Feibi Jiubi treats application windows as
toys.

> [!NOTE]
> The product contains two characters: Nuonuo and Feibi Jiubi. Releases are
> distributed as a self-contained Windows x64 portable folder.

## License and assets

Project code is licensed under [PolyForm Noncommercial 1.0.0](LICENSE).
Commercial use is not included. See [NOTICE](NOTICE) and
[ASSET_RIGHTS.md](ASSET_RIGHTS.md) for asset ownership, source, and
redistribution details.

## Features

- Choose Nuonuo or Feibi Jiubi, or show both characters together.
- Walk, climb, fall, sleep, drag, toss, and collide with desktop terrain.
- React to files entering the Recycle Bin without reading or deleting file
  contents.
- Let Feibi kick, punch, charge into, store, shatter, and tear eligible
  application windows. Stored windows are restored only when requested.
- Support quiet, fasting, infinite-progress, speech-bubble, and startup modes.
- Resize pets from 60% to 200%, with multi-monitor and per-monitor DPI support.
- Keep runtime data local; the application does not upload logs or screenshots.

## Requirements

| Use case | Requirements |
| --- | --- |
| Packaged release | Windows 10/11 x64 |
| Run from source | Windows 10/11 x64 and .NET 8 SDK |
| Build a portable release | .NET 8 SDK, Python/Pillow, and Visual Studio 2022 C++ or LLVM-MinGW |

The application is Windows-only because transparent windows, tray menus,
desktop-item positioning, and window terrain use Win32 and WPF.

## Quick start

### Run from source

```powershell
git clone https://github.com/nicedoctors/JiuNuoDesktopPet.git
cd JiuNuoDesktopPet
.\run.ps1
```

Or use the .NET CLI:

```powershell
dotnet run --project .\src\SoftMochiPet\SoftMochiPet.csproj -c Release
```

### Build a portable release

```powershell
.\package.ps1
```

The package is written to `dist\`. Extract the ZIP completely before launching
the root `啾糯桌宠.exe`; keep its `程序文件` directory beside it.

## Controls

| Action | Result |
| --- | --- |
| Hold and move Nuonuo with the left mouse button | Drag the pet and pause autonomous movement |
| Release slowly | Gently place the pet on nearby terrain |
| Release quickly | Throw the pet using the release velocity |
| Right-click the pet | Open the tray menu |
| Double-click the tray icon | Bring the pet home |
| Use the size slider | Resize from 60% to 200% |
| Enable Quiet mode | Stop autonomous walking, feeding, and mischief |
| Enable Mute | Stop or restore character voice playback |
| Choose Exit pet | Save state and close the application |

## Development and tests

```powershell
.\build.ps1
dotnet run --project .\tests\SoftMochiPet.LogicTests\SoftMochiPet.LogicTests.csproj -c Release
.\publish.ps1
```

See [CONTRIBUTING.md](CONTRIBUTING.md), [SELF_TEST.md](SELF_TEST.md), and the
documents in `docs/` for development and manual validation procedures.

## Data and privacy

Settings and life state are stored below `%APPDATA%\SoftMochiPet`. Diagnostic
logs are stored below `%LOCALAPPDATA%\SoftMochiPet`. The application keeps
runtime data local, does not read file contents, and does not actively delete
user files. See [docs/PRIVACY.md](docs/PRIVACY.md).
