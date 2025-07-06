# Lux Synthesis patcher

## Description

Carries over all changes from [Lux](https://www.nexusmods.com/skyrimspecialedition/mods/43158). Also patches some modded locations to use Lux's lighting templates & image spaces, and adjusts some modded imagespaces & lights.

<details>
  <summary>Detailed list of included changes</summary>

  - Image Spaces: HDR, cinematic, tint
  - Lights: record flags, flags, object bounds, radius, color, near clip, fade value
  - Worldspaces: interior lighting
  - Cells: lighting, lighting template, water height, water noise texture, sky and weather from region, image space
  - Placed objects: record flags, primitive, light data, bound half extents, unknown, lighting template, image space, location reference, placement
</details>

<details>
  <summary>Supported mods</summary>
  Patcher was made for version in parantheses, but should mostly work okay for any version.

  - Based on ELE's official patches, with updates here & there as said patches are 2 years old
    - [Beyond Skyrim - Bruma SE](https://www.nexusmods.com/skyrimspecialedition/mods/10917) (1.4.2)
    - [Cutting Room Floor - SSE](https://www.nexusmods.com/skyrimspecialedition/mods/276) (3.1.9)
    - [Darkend](https://www.nexusmods.com/skyrimspecialedition/mods/10423) (1.4)
    - [Falskaar](https://www.nexusmods.com/skyrimspecialedition/mods/2057) (2.2)
    - [Helgen Reborn](https://www.nexusmods.com/skyrimspecialedition/mods/5673) (V106.SSE)
      - Added light bulb colors
    - [Lanterns of Skyrim](https://www.nexusmods.com/skyrimspecialedition/mods/2429) (any version)
    - [Legacy of the Dragonborn SSE](https://www.nexusmods.com/skyrimspecialedition/mods/11802) (5.5.2, 4.1.1 support included)
      - Added light bulb colors
      - v5 version uses brighter lighting templates for the museum interior, since the lighting almost purely depends on those now
    - [Medieval Lanterns of Skyrim](https://www.nexusmods.com/skyrimspecialedition/mods/27622) (any version)
    - [Ravengate](https://www.nexusmods.com/skyrimspecialedition/mods/12617) (0.06BTASSE)
      - Added light bulb colors
</details>

## Installation

### Synthesis (Match or 0.32.1+ required)

If you have Synthesis, there are 3 options:
- In Synthesis, click on Git repository, and choose Lux Patcher from the list of patchers
  - Not recommended
