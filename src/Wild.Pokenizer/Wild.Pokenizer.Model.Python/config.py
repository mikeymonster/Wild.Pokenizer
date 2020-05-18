"""
Contains common constants and configuration
"""

DATA_ARCHIVE_PATH='D:/datascience/complete-pokemon-image-dataset.zip'
DATA_DIR = '../../../data/pokemon-image-dataset'
#TRAIN_DATA_DIR = '../../../data/pokemon-image-dataset/two_pokemon/train'
#VALIDATION_DATA_DIR = '../../../data/pokemon-image-dataset/two_pokemon/val'
TRAIN_DATA_DIR = '../../../data/pokemon-image-dataset/train'
VALIDATION_DATA_DIR = '../../../data/pokemon-image-dataset/val'
TEST_DATA_DIR = '../../../data/pokemon-image-dataset/test'

SAVED_MODELS_DIR = './SavedModels'
MODEL_FILE_NAME = 'Pokemon_trained.h5'
TFLITE_MODEL_NAME = 'model.tflite'

TRAIN_SAMPLES= 28 #2000
VALIDATION_SAMPLES= 5 #800
IMG_WIDTH, IMG_HEIGHT = 160, 160
NUM_CLASSES = 2 #928
BATCH_SIZE = 24

RANDOM_SEED = 348258
