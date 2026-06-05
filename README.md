# JellyBeanAttack

## Development Environment
* Unity Version: 6000.3.4f1
* Render Pipeline: Built-in Render Pipeline / URP
* Target Platform: PC

## How to Open and Run
### Run the Pre-built Executable
1. Go to the **[Releases]** page of this GitHub repository.  
2. Download the latest release zip file (e.g., `JellyBeanAttack_vX.X.X.zip`).  
3. Extract the downloaded zip file to any folder on your local machine.  
4. Double-click and run the **`JellyBeanAttack.exe`** file to play the game instantly.  

## Completed Features & Specifications
### Core Gameplay & Logic
Grid System: A structured 6x6 grid dynamically generated at runtime using 4 random colors (Red, Blue, Green, Yellow).  
Match Detection (BFS): Implemented Breadth-First Search (BFS) for precise 4-directional matching (Up, Down, Left, Right). Diagonals are correctly ignored.  
Score System: Successfully grants 10 points per exploded jellybean when selecting a valid group of 3 or more.  
Zero-Gravity Refill: Adhering strictly to the constraint of "no gravity/falling behavior", matched cells instantly swap to new randomly generated colors without altering the rest of the board.  

### Board Validity (Soft-lock Prevention)
Implemented a validation algorithm that scans the board to ensure at least one valid match (3+ connected colors) exists.  
The validation triggers during two key phases:  
Initial board generation.  
Immediately after a grid refill.  
If no valid match is detected, the board automatically regenerates until a playable state is achieved, preventing soft-locks.  

### UI and Visual Feedback
Dynamic Polish: Added clean visual particle effects/animations when jellybeans vanish upon a valid match.  
Invalid Click Feedback: Plays a distinct animation/feedback when clicking a group of fewer than 3 jellybeans.  
Game Over Sequence: When the 60s timer hits 0, grid interactions are blocked, and a custom UI overlay counts up the final score leading into a Restart button.  

## Known Issues / Limitations
* **Color Accessibility:**
  The current prototype relies solely on solid colors (Red, Blue, Green, Yellow) to distinguish the jellybeans. This may cause readability and gameplay difficulties for users with color vision deficiencies (color blindness).  
*(Note: A plan to resolve this by implementing distinct shapes/symbols is documented under the "Future Improvements" section.)*

## Future Improvements (With More Time)
If given more development time, I would focus on the following features to elevate the user experience and overall polish:  

* **Accessibility / Color Blindness Support:** 
  Modify the asset shapes or add unique inner symbols for each color (e.g., changing the silhouettes) to ensure the board is fully readable for players with color vision deficiencies.

* **Dynamic Text Effects (Juiciness):** 
  Implement animated pop-up text (e.g., "+30", "Perfect!") when floating above matched groups to give players more satisfying and punchy visual feedback.

* **Interactive Tutorial:** 
  Add a brief, guided onboarding sequence or overlay to instantly teach new players the 4-directional matching rule and the 3-bean minimum requirement.

* **Game Start Sequence:** 
  Introduce a dramatic "Ready... Go!" intro animation/countdown before the timer starts ticking, enhancing the game's arcade-style pacing.

* **High Score System:** 
  Implement local persistent storage (using `PlayerPrefs`) to save and display the player's personal best score, increasing replayability.

* **Atmospheric Background Art:** 
  Replace the flat color background with a stylized, dynamic background theme that matches the playful "Jellybean" aesthetic.  

## Tools & References Used
* **AI Assistance (Gemini):** 
  Utilized Gemini to research and optimize core matching algorithms, brainstorm juice/visual feedback implementations, and refine standard standalone build settings.  

* **Documentation & Resources:** 
  Referenced official Unity Documentation for URP setup and standard Wikipedia entries for BFS (Breadth-First Search) implementation structures.

## Gameplay Video
Check out how the game looks and feels!

![GamePlayDemo1](https://github.com/user-attachments/assets/f59db15a-4492-4868-b324-7ac4339bf4de)
![GamePlayDemo2](https://github.com/user-attachments/assets/43fa6522-0e56-4c62-9d03-0e4ab4ce7c77)

---
Enjoy the game!
