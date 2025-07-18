# 🎛️ **Inept‑Intruders**  
A frantic *asymmetric* co‑op heist where communication is your only lifeline.

> *One player is stuck in a booby‑trapped factory floor.  
> The other sits in a security booth, flipping CCTV feeds and frantically mashing buttons to keep their partner alive.  
> Neither of you can see what the other sees—so talk fast, or die trying.*

[![Unity Version](https://img.shields.io/badge/Unity-2022.3%20LTS-blue?logo=unity)](#prerequisites)
![GitHub license](https://img.shields.io/github/license/YOUR‑ORG/inept‑intruders)

---

## ✨ Core Loop
1. **Scout:** *Cam‑Guy* cycles CCTV feeds, spotting hazards and factory‑bot patrols.  
2. **Manipulate:** Pull levers, power down lasers, rotate walls—each camera view has a unique control panel.  
3. **Sneak:** *Infiltrator* navigates in 1st‑person, following shouted instructions (“Left! Disable crate‑crusher!”).  
4. **Escape:** Retrieve the prototype, regroup at the extraction elevator, and bail before the alarm timer hits 0.

Clear a level? A new wing of the plant unlocks—harder layouts, deadlier traps, fewer comm checkpoints.

---

## 🕹️ Feature Highlights

| System                     | Details                                                                 |
|----------------------------|-------------------------------------------------------------------------|
| **Asymmetric Co‑Op**       | Two distinct roles: *Infiltrator* & *Cam‑Guy*. Each sees only half the puzzle. |
| **Network Multiplayer**    | Built with *Unity Netcode for GameObjects* (🔌 relay or LAN).            |
| **Dynamic Factory Rooms**  | Conveyor belts, rotating corridors, press hammers, electrified floors—toggleable in real time. |
| **Contextual Buttons**     | Every CCTV view exposes bespoke “big red buttons” that affect only that room. |
| **Diegetic UI**            | Cam‑Guy’s in‑game CRT wall & hardware panel—no meta overlays.           |
| **VOIP‑Ready**             | Native Vivox integration (fallback to any third‑party chat).            |
| **Replay‑Friendly**        | Procedural hazard seeds & role swap option keep runs fresh.             |
| **Accessibility**          | Full remap, color‑blind safe palettes, subtitle captions for SFX cues.  |

---

## 📦 Project Structure
```text
/Assets
  /Art
    /Environments        # Modular factory kit, HDRP
    /Props               # Traps, machines, CCTV consoles
  /Scripts
    /Gameplay            # Player, hazard, door logic
    /Networking          # NGO wrappers & RPC helpers
    /CameraBooth         # Feed switching, UI, button bindings
  /Scenes
    /Lobby               # Role select & matchmaking
    /Levels              # Procedural seeds + baked layouts
  /Audio
  /Tests
```

---

## 🚀 Getting Started

### Prerequisites
* **Unity 2022.3 LTS** (*HDRP template*)  
* .NET 6 SDK (for unit tests & custom build CLI)  
* Git LFS (large texture / HDRP asset bundles)  

### 1 · Clone
```bash
git clone --recursive https://github.com/YOUR-ORG/inept-intruders.git
```

### 2 · Configure Networking  
The project ships with *Relay* for quick play.  
For LAN: tick **Use Direct IP** in *NetworkManager* before hitting Play.

### 3 · Host or Join  
* **Host:** run *LobbyScene*, click **Create Room**, share lobby code.  
* **Client:** input lobby code, choose a role, profit.

---

## 🎮 Controls

| Action                      | Infiltrator (KB/M) | Cam‑Guy (KB/M)      | Gamepad (Both) |
|-----------------------------|--------------------|---------------------|---------------|
| Move                        | WASD              | WASD (booth walk)   | Left Stick    |
| Look / Aim                  | Mouse Δ           | Mouse Δ             | Right Stick   |
| Interact / Use              | E                 | E (press buttons)   | X / ▢         |
| Sprint                      | Shift             | —                   | B / O         |
| Switch CCTV Feed            | —                 | Q / E or Scroll     | LB / RB       |
| Toggle Panel (Cam‑Guy)      | —                 | F                   | Y / △         |
| Push‑to‑Talk (VOIP)         | V                 | V                   | LT            |
| Pause / Menu                | Esc               | Esc                 | Start         |

> **Hint:** Cam‑Guy can mark points by double‑tapping the *Toggle Panel* key—an audio ping plays for the Infiltrator.


---

## 👥 Contributing Guidelines
1. **Fork** → branch off `main` (`feat/my‑awesome‑thing`).  
2. Run `npm run test` (PlayMode + EditMode).  
3. Lint passes? Commit with conventional‑commit message.  
4. Open a PR with screenshots / short clip (<30 s).  
5. Two approvals auto‑merge.

We follow the **Unity Coding Standards** (see `CONTRIBUTING.md`).

---

## 🗺️ Roadmap
- [ ] **Matchmaking Service** (Steam Lobbies & P2P fallback)  
- [ ] **Workshop Support** (custom room layouts & trap mods)  
- [ ] **Spectator Mode** with stream‑friendly HUD  
- [ ] **Cross‑play** builds for Switch & Steam Deck  

Vote or suggest features in [Discussions](https://github.com/YOUR-ORG/inept-intruders/discussions).

---

## 📚 Acknowledgements
* *Special thanks* to the keimyung university mentor who green‑lit the concept.  
* **Mirror** & **NGO** dev communities for open‑source networking gems.  
* Everyone who shouted “LEFT LEFT LEFT!” at 3 AM and still remained friends. ❤️

---

## 🛡️ License
Released under the **MIT License**.  
Commercial streaming is allowed, but please credit the team and link back to the repo.

---

<p align="center">
  <img src="docs/inept_intruders_teaser.gif" width="550" alt="Inept Intruders gameplay teaser">
</p>
