# Utility functions for the emotion app.
import os
import numpy as np


# Generate participant sequences.
def generate_shuffled_scenes(scene_path="./res/scene_names.txt"):
    # Read scene files.
    # Get path to the app root.
    app_root = os.path.dirname(os.path.abspath(__file__))
    with open(os.path.join(app_root, scene_path), "r") as f:
        scenes = f.readlines()
    scenes = [scene.strip() for scene in scenes]

    # Shuffle scenes.
    np.random.shuffle(scenes)
    return list(scenes)

