# CPAX — Hedge Maze

A real-time interactive 3D navigation environment for studying decision-making behavior. This repository contains the **hedge maze** map of the CPAX project, built in Unity.

Participants navigate a branching, feed-forward maze from an entrance to an exit. At each node the path splits into two forward options, and because the structure is feed-forward, choices cannot be reversed — making every node a discrete decision point. The system logs navigation behavior (traversal times, per-node decision times, route choices, completion time) to CSV for later analysis.

## About the Project

CPAX (Cognitive Pathway Xtreme) is a research platform developed at the Electronic Visualization Laboratory (EVL), University of Illinois Chicago, to capture decision-making behavior in controlled, visually familiar environments. It is an interdisciplinary collaboration spanning neuroscience, computer science, and design.

The hedge maze is one of two environments in CPAX (the other being a subway maze). Both share the same feed-forward decision paradigm but differ in visual theme, layout, and edge weights, and are analyzed independently.

## Features

- Branching, feed-forward graph structure with defined decision points
- Landmark assets (statues, fountains, garden gates, vegetation) that give each region a distinct visual identity to support wayfinding
- Automatic behavioral logging to CSV via a custom C# module:
  - Per-edge traversal time
  - Per-node decision time
  - Route choices and sequence order
  - Total completion time
  - Partial data from incomplete sessions
- Time-based scoring (out of 100) revealed at the exit
- Repeated runs supported, each recorded independently

## Built With

- **Unity** [ghi phiên bản chính xác của hedge maze — ví dụ 6.4 / 6000.4.9f1]
- **ProBuilder** — maze layout construction
- **C#** — game logic and behavioral logging
- **Autodesk Maya / Blender** — asset modification
- **Adobe Creative Suite** — texturing and visual adjustments

## Getting Started

### Prerequisites

- Unity [phiên bản] or compatible
- [các dependency khác nếu có]

### Running the Project

1. Clone the repository:
   ```bash
   git clone [URL repo của bạn]
   ```
2. Open the project in Unity Hub with Unity version [phiên bản].
3. Open the scene [tên scene, ví dụ Scenes/HedgeMaze.unity].
4. Press Play in the editor, or build a standalone application for Windows/macOS.

### Data Output

Navigation data is written to a per-run CSV file [ghi rõ đường dẫn output, ví dụ `path_results_<timestamp>.csv`]. Each file contains path-level, decision-level, and playthrough-level records.

## Repository Structure

```
[điền cấu trúc thư mục chính, ví dụ:]
Assets/
  Scenes/        # Unity scenes
  Scripts/       # C# logic and logging
  ...
```

## Credits

- Ambient sound: "Nat_20.wav" by TheWAVLab, sourced from [Freesound](https://freesound.org/people/TheWAVLab/sounds/244180/) under CC BY 4.0
- Environment assets sourced from the Unity Asset Store and other asset libraries

## License

[Chọn license — ví dụ MIT, hoặc ghi "All rights reserved" nếu chưa quyết. Lưu ý: kiểm tra license của các asset bên thứ ba trước khi công khai code.]

## Acknowledgments

Developed at the Electronic Visualization Laboratory (EVL), University of Illinois Chicago, under the guidance of Professor Daria Tsoupikova, with the CPAX research team.
