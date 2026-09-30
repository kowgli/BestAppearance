# BestAppearance

BestAppearance is a small, one-click Windows utility that restores the visual-effects configuration to **Adjust for best appearance**.

It was created for environments where software such as Citrix repeatedly changes Windows display-performance settings to favor performance. Those changes can disable useful visual features, including displaying full window contents while dragging and ClearType font smoothing. Instead of reopening Performance Options and restoring the settings manually, run BestAppearance once.

## What it restores

The utility enables Windows visual effects such as:

- Showing window contents while dragging
- Window, menu, combo-box, tooltip, and taskbar animations
- Menu, selection, and tooltip fade effects
- Smooth scrolling
- Mouse-pointer shadows
- Desktop composition-related UI effects
- Font smoothing and ClearType

It also sets the Performance Options selection to **Adjust for best appearance** and broadcasts the settings change so Windows and running applications can respond immediately.

## Usage

1. Build the project in Visual Studio or with MSBuild.
2. Run `BestAppearance.exe` whenever the settings have been changed.

The program applies the settings to the current Windows user and then exits. It has no user interface and normally does not require administrator privileges.

## Build

From the repository root:

```powershell
MSBuild .\BestAppearance\BestAppearance.csproj /t:Rebuild /p:Configuration=Release
```

The executable will be created at:

```text
BestAppearance\bin\Release\BestAppearance.exe
```

## Why .NET Framework?

This project intentionally targets **.NET Framework 4.8.1** rather than modern .NET. The goal is to produce a small, self-contained utility that can run on supported Windows installations without requiring the user to install a separate modern .NET runtime or deploy additional application dependencies. This is particularly useful on managed corporate computers where installing software may require administrator approval.

## Requirements

- Windows
- .NET Framework 4.8.1, included with current supported Windows versions or available through Windows Update

## License

See [LICENSE.txt](LICENSE.txt).

