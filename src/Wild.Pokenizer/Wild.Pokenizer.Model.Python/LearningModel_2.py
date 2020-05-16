from io import BytesIO
import os
import requests
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from PIL import Image
from sklearn import model_selection
import tensorflow as tf
#from tensorflow.keras.preprocessing.image import ImageDataGenerator
#from tensorflow.keras.models import Model
#from tensorflow.keras.layers import Input, Flatten, Dense, Dropout, GlobalAveragePooling2D, GlobalMaxPooling2D
#from tensorflow.keras.applications.mobilenet import MobileNet, preprocess_input
#from tensorflow.keras.callbacks import EarlyStopping, ReduceLROnPlateau

from tensorflow.keras.optimizers import Adam
from tensorflow.keras.models import Model,load_model
from tensorflow.keras.applications.inception_v3 import InceptionV3
from tensorflow.keras.layers import Dense,Input,GlobalMaxPooling2D
from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras.callbacks import EarlyStopping,ReduceLROnPlateau


batch_size = 24
num_classes = 928 # this many classes of Pokemon in the dataset

data_dir= '../../../data/pokemon-image-dataset/all'

data_generator = ImageDataGenerator(rescale=1./255,
                                    horizontal_flip=True,
                                    vertical_flip=False,
                                    brightness_range=(0.5,1.5),
                                    rotation_range=10,
                                    validation_split=0.2) # use the `subset` argument in `flow_from_directory` to access

train_generator = data_generator.flow_from_directory(data_dir,
                                                    target_size=(160,160), # chosen because this is size of the images in dataset
                                                    batch_size=batch_size,
                                                    subset='training')

validation_generator = data_generator.flow_from_directory(data_dir,
                                                          target_size=(160,160),
                                                          batch_size=batch_size,
                                                          subset='validation')

# import the base model and pretrained weights
custom_input = Input(shape=(160,160,3,))
base_model = InceptionV3(include_top=False, 
                         weights='imagenet', 
                         input_tensor=custom_input, 
                         input_shape=None, 
                         pooling=None, 
                         classes=num_classes)

x = base_model.layers[-1].output # snag the last layer of the imported model

x = GlobalMaxPooling2D()(x)
x = Dense(1024,activation='relu')(x) # an optional extra layer
x = Dense(num_classes,activation='softmax',name='predictions')(x) # our new, custom prediction layer

model = Model(inputs=base_model.input,outputs=x)

# new model begins from the beginning of the imported model,
# and the predictions come out of `x` (our new prediction layer)

# let's train all the layers
for layer in model.layers:
    layer.training = True

# these are utilities to maximize learning, while preventing over-fitting
reduce_lr = ReduceLROnPlateau(monitor='val_loss', patience=12, cooldown=6, rate=0.6, min_lr=1e-18, verbose=1)
early_stop = EarlyStopping(monitor='val_loss', patience=24, verbose=1)

# compile and train the network
model.compile(optimizer=Adam(1e-8),
              loss='categorical_crossentropy',
              metrics=['accuracy'])

history = model.fit_generator(train_generator,
                    validation_data=validation_generator,
                    steps_per_epoch=2000//batch_size,
                    validation_steps=800//batch_size,
                    epochs=10, # increase this if actually training
                    shuffle=True,
                    callbacks=[reduce_lr,early_stop],
                    verbose=0)

history.history['loss'][-1]
history.history['accuracy'][-1]

#Snippet for plotting history from fit - ref p305
pd.DataFrame(history.history).plot(figsize=(8, 5))
plt.grid(True)
plt.gca().set_ylim(0, 1)
plt.show()

