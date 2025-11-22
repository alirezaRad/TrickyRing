# Tricky.Ring Game

**TrickyRing** is a simple yet engaging mobile game developed in **Unity** using **C#**.  
The project emphasizes smooth gameplay, responsive UI, and an enjoyable user experience, optimized for **Android devices**.

> ⚡ This game was developed in a short timeframe of **two days over a weekend**, focusing on delivering a polished experience quickly.



---
- [Landscape GIF](https://github.com/alirezaRad/TrickyRing/blob/main/Recordings/Movie_005.gif)
- [Portrait GIF](https://github.com/alirezaRad/TrickyRing/blob/main/Recordings/Movie_006.gif)


## 🎮 Features

### Game Design and Gameplay
- **Responsive UI:** Fully supports multiple aspect ratios, including portrait (9:16, 9:18, 3:4, etc.) and landscape. The layout adapts automatically to different screen sizes.
- **Splash and Loading Screens:** Includes a splash screen with the game’s name and a loading bar. On first launch, the player’s name is requested.
- **Smooth Scene Transitions:** Animated scene transitions create the feel of a single continuous game environment.
- **Player Score Tracking:** Tracks the current score and saves the highest score locally using PlayerPrefs.
- **Game Over & Share Functionality:** Displays the player’s final score and high score, with a button to share a screenshot of the game.

### Leaderboard
- **Multiple Modes:** Daily, weekly, and all-time leaderboards.
- **Dynamic Loading:** Simulates a large number of players (~6000) with randomized scores. Only visible leaderboard items are loaded at a time to ensure smooth scrolling.
- **Current Player Highlight:** The current player is highlighted for easy identification.
- **Optimized Performance:** Sorting and inserting players are efficiently handled over multiple frames to prevent frame drops, even with very large datasets.

### Animations & Visuals
- **DG.Tweening Animations:** All UI and gameplay animations are smooth and polished using DG.Tweening.
- **Particle Effects & Visual Enhancements:** Enhance the overall game experience.
- **UI & Graphics:** Designed in Figma for a clean and intuitive interface.

### Audio
- **High-Quality Sounds:** Licensed sound effects and music enhance the player experience.
- **Preloaded Audio Clips:** Important sounds are preloaded to avoid performance issues.
- **Smooth Music Transitions:** Music tracks are played with fade effects for a seamless experience.

### Data & Saving
- **Local Game Data:** Saved using PlayerPrefs.
- **JSON Export:** Key actions generate a JSON file in the Android data folder containing the player’s profile, scores, and settings.

### Architecture & Development Approach
- Prioritizes **performance and gameplay** over strict architectural patterns.
- Built mainly with **Prefabs** and **ScriptableObjects** for simplicity and speed.
- **Events & Communication:** Handled via ScriptableObjects for readability and efficiency.
- Simplified code design to enhance understanding and maintainability while following basic coding standards.

### Additional Information
- The repository includes a **build of the game** for testing.
- The project focuses on delivering a **fun, smooth, and engaging experience** while keeping the codebase simple and efficient.

---

## ⚠️ Notes
- Settings panel, pause/resume menu, and exit popups were **not implemented** due to time constraints.
- The game prioritizes **smooth performance and user experience** over strict modular architecture.
