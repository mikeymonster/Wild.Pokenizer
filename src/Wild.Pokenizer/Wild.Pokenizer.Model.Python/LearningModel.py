from io import BytesIO
import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from PIL import Image
import requests

import tensorflow as tf
#import tensorflow.keras as keras

print('Working directory: ' + os.getcwd())
print('TensorFlow version: ' + tf.__version__)
print('Keras version:' + keras.__version__)

"""
Data is in ../../../datacomplete-pokemon-image-dataset.zip
"""