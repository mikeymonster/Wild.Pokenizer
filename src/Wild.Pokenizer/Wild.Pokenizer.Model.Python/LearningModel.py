from io import BytesIO
import os
import requests
import math
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from PIL import Image
from sklearn import model_selection
import tensorflow as tf
from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras.models import Model
from tensorflow.keras.layers import Input, Flatten, Dense, Dropout, GlobalAveragePooling2D, GlobalMaxPooling2D
from tensorflow.keras.applications.mobilenet import MobileNet, preprocess_input
from tensorflow.keras.callbacks import EarlyStopping, ReduceLROnPlateau

import config #shared configuration module 

# Check working directory and set if necessary:
# os.getcwd()
# #os.chdir("..\\..\\..\\source\\repos\\Wild.Pokenizer\\src\\Wild.Pokenizer\\Wild.Pokenizer.Model.Python")

print('TensorFlow version: ' + tf.__version__ + ' Keras version:' + tf.keras.__version__)

#Convert to tensorflowlite
def convert_model_to_tflite(model, output_dir, file_name):
    output_path = os.path.normpath(os.path.join(output_dir, file_name))
    converter = tf.lite.TFLiteConverter.from_keras_model(model)
    #converter = tf.lite.TFLiteConverter.from_keras_model(from_saved_modele(MODEL_FILE_PATH)
    tflite_model = converter.convert()
    #tflite_model.save() #Fails with error 'bytes' object has no attribute 'save'
    open(output_path, "wb").write(tflite_model)


#Set up model
#num_classes = len(labels) # this many classes of Pokemon in the dataset
#num_classes = config.NUM_CLASSES
#custom_input = Input(shape=(160,160,3,))

#Adapted from
#https://github.com/PracticalDL/Practical-Deep-Learning-Book/blob/master/code/chapter-3/1-keras-custom-classifier-with-transfer-learning.ipynb

train_datagen = ImageDataGenerator(preprocessing_function=preprocess_input,
                                   #rescale=1./255,
                                   rotation_range=20,
                                   width_shift_range=0.2,
                                   height_shift_range=0.2,
                                   horizontal_flip=True,
                                   vertical_flip=False,
                                   brightness_range=(0.5,1.5),
                                   zoom_range=0.2)
#Parameters in old version:
#data_generator = ImageDataGenerator(rescale=1./255,
#                                    horizontal_flip=True,
#                                    vertical_flip=False,
#                                    brightness_range=(0.5,1.5),
#                                    rotation_range=10,

val_datagen = ImageDataGenerator(preprocessing_function=preprocess_input)

train_generator = train_datagen.flow_from_directory(config.TRAIN_DATA_DIR,
                                                    target_size=(config.IMG_WIDTH,config.IMG_HEIGHT),
                                                    batch_size=config.BATCH_SIZE,
                                                    shuffle=True,
                                                    seed=12345,
                                                    class_mode='categorical')
validation_generator = val_datagen.flow_from_directory(config.VALIDATION_DATA_DIR,
                                                       target_size=(config.IMG_WIDTH, config.IMG_HEIGHT),
                                                       batch_size=config.BATCH_SIZE,
                                                       shuffle=False,
                                                       class_mode='categorical')

#https://expoundai.wordpress.com/2019/04/24/image-data-generators-in-keras/
#train_samples = config.TRAIN_SAMPLES
#validation_samples = config.VALIDATION_SAMPLES
train_samples = train_generator.samples
validation_samples = validation_generator.samples

num_classes = train_generator.num_classes
print("Have " + str(num_classes) + " classes")

#def build_model():
base_model = MobileNet(include_top=False,
                        input_shape=(config.IMG_WIDTH, config.IMG_HEIGHT, 3))
for layer in base_model.layers[:]:
#for layer in base_model.layers[-20:]:
    layer.trainable = True
    #layer.trainable = False

input = Input(shape=(config.IMG_WIDTH, config.IMG_HEIGHT, 3))
custom_model = base_model(input)
#custom_model = GlobalAveragePooling2D()(custom_model)
custom_model = GlobalMaxPooling2D()(custom_model)
custom_model = Dense(4096, activation='relu')(custom_model)
#custom_model = Dense(64, activation='relu')(custom_model)
custom_model = Dense(1024, activation='relu')(custom_model)
#custom_model = Dropout(0.5)(custom_model)
predictions = Dense(num_classes, activation='softmax')(custom_model)
    
#    return Model(inputs=input, outputs=predictions)

#model = build_model()
model = Model(inputs=input, outputs=predictions)

model.compile(loss='categorical_crossentropy',
              optimizer=tf.keras.optimizers.Adam(0.00001), #1e-8),
              metrics=['acc'])

#Callbacks
reduce_lr = ReduceLROnPlateau(monitor='val_loss', patience=10, cooldown=6, rate=0.6, min_lr=1e-18, verbose=1)
early_stop = EarlyStopping(monitor='val_loss', patience=15, verbose=1)

#Set up tensorboard callback
tensor_board = tf.keras.callbacks.TensorBoard(r'.\logs\Pokemon_Learner')


history = model.fit_generator (train_generator,
                    steps_per_epoch=math.ceil(float(train_samples) / config.BATCH_SIZE),
                    epochs=25,
                    validation_data=validation_generator,
                    validation_steps=math.ceil(float(validation_samples) / config.BATCH_SIZE),
                    callbacks=[reduce_lr,early_stop,tensor_board])

#Now retrain with some layers unfrozen
#for layer in base_model.layers[-20:]:
#    layer.trainable = True

model.compile(loss='categorical_crossentropy',
              optimizer=tf.keras.optimizers.Adam(1e-8), #Use a lower learning rate for retraining
              metrics=['acc'])

retraining_history = model.fit_generator (train_generator,
                                          steps_per_epoch=math.ceil(float(train_samples) / config.BATCH_SIZE),
                                          epochs=10,
                                          validation_data=validation_generator,
                                          validation_steps=math.ceil(float(validation_samples) / config.BATCH_SIZE),
                                          callbacks=[reduce_lr,early_stop,tensor_board])

print("Classes: ")
validation_generator.class_indices
#TODO: Write these labels to a file or clipboard. We will need them in our client application
print("Lablel: ")
for lbl in validation_generator.class_indices:
    print(lbl)
classes = [_class for _class in validation_generator.class_indices]


#Snippet for plotting history from fit - ref p305
pd.DataFrame(history.history).plot(figsize=(8, 5))
plt.grid(True)
plt.gca().set_ylim(0, 1)
plt.show()

pd.DataFrame(retraining_history.history).plot(figsize=(8, 5))
plt.grid(True)
plt.gca().set_ylim(0, 1)
plt.show()

#Evaluate the model against the test data
#model.evaluate(X_test, y_test)

#Save the model
#os.rmdir(config.SAVED_MODELS_DIR)
if not os.path.isdir(config.SAVED_MODELS_DIR):
    os.mkdir(config.SAVED_MODELS_DIR)

model.save(os.path.normpath(os.path.join(config.SAVED_MODELS_DIR, config.MODEL_FILE_NAME)))
convert_model_to_tflite(model, config.SAVED_MODELS_DIR, config.TFLITE_MODEL_NAME)

"""
#snippet for showing an image
url = 'https://i.imgur.com/5Nycvcx.jpg'
response = requests.get(url)
img_1 = Image.open(BytesIO(response.content))
#display(img_1)
plt.imshow(img_1)
plt.show()
"""

#https://github.com/PracticalDL/Practical-Deep-Learning-Book/blob/master/code/chapter-3/2-analyzing-the-results.ipynb

