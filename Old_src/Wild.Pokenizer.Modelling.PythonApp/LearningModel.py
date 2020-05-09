import os
import requests
import numpy as np
import pandas as pd
from PIL import Image
from io import BytesIO
import matplotlib.pyplot as plt

from keras.optimizers import Adam
from keras.models import Model,load_model
from keras.applications.inception_v3 import InceptionV3
from keras.layers import Dense,Input,GlobalMaxPooling2D
from keras.preprocessing.image import ImageDataGenerator
from keras.callbacks import EarlyStopping,ReduceLROnPlateau


print(os.getcwd())

# Can change with __file__ if running as script
os.chdir(os.path.dirname("C:/Users/mike/source/repos/Wild.Pokenizer\Wild.Pokenizer.Modelling.PythonApp"))



batch_size = 24
num_classes = 928 # this many classes of Pokemon in the dataset

data_generator = ImageDataGenerator(rescale=1./255,
                                    horizontal_flip=True,
                                    vertical_flip=False,
                                    brightness_range=(0.5,1.5),
                                    rotation_range=10,
                                    validation_split=0.2) # use the `subset` argument in `flow_from_directory` to access

train_generator = data_generator.flow_from_directory('../input/complete-pokemon-image-dataset/pokemon',
                                                    target_size=(160,160), # chosen because this is size of the images in dataset
                                                    batch_size=batch_size,
                                                    subset='training')

val_generator = data_generator.flow_from_directory('../input/complete-pokemon-image-dataset/pokemon',
                                                    target_size=(160,160),
                                                    batch_size=batch_size,
                                                    subset='validation')

# import the base model and pretrained weights
custom_input = Input(shape=(160,160,3,))
base_model = InceptionV3(include_top=False, weights='imagenet', input_tensor=custom_input, input_shape=None, pooling=None, classes=num_classes)

x = base_model.layers[-1].output # snag the last layer of the imported model

x = GlobalMaxPooling2D()(x)
x = Dense(1024,activation='relu')(x) # an optional extra layer
x = Dense(num_classes,activation='softmax',name='predictions')(x) # our new, custom prediction layer

model = Model(input=base_model.input,output=x)
# new model begins from the beginning of the imported model,
# and the predictions come out of `x` (our new prediction layer)

# let's train all the layers
for layer in model.layers:
    layer.training = True

# these are utilities to maximize learning, while preventing over-fitting
reduce_lr = ReduceLROnPlateau(monitor='val_loss', patience=12, cooldown=6, rate=0.6, min_lr=1e-18, verbose=1)
early_stop = EarlyStopping(monitor='val_loss', patience=24, verbose=1)
# compile and train the network
model.compile(optimizer=Adam(1e-8),loss='categorical_crossentropy',metrics=['accuracy'])
model.fit_generator(train_generator,
                    validation_data=val_generator,
                    steps_per_epoch=2000//batch_size,
                    validation_steps=800//batch_size,
                    epochs=1, # increase this if actually training
                    shuffle=True,
                    callbacks=[reduce_lr,early_stop],
                    verbose=0)

model.save('InceptionV3_Pokemon_py.h5')

def predict_this(this_img):
    im = this_img.resize((160,160)) # size expected by network
    img_array = np.array(im)
    img_array = img_array/255 # rescale pixel intensity as expected by network
    img_array = np.expand_dims(img_array, axis=0) # reshape from (160,160,3) to (1,160,160,3)
    pred = model.predict(img_array)
    return np.argmax(pred, axis=1).tolist()[0]

classes = [_class for _class in os.listdir('../input/complete-pokemon-image-dataset/pokemon/')]
classes.sort() # they were originally converted to number when loaded by folder, alphabetically


url = 'https://i.imgur.com/5Nycvcx.jpg'
response = requests.get(url)
img_1 = Image.open(BytesIO(response.content))

print("A wild {} appears!".format(classes[predict_this(img_1)]))
display(img_1)


# the same thing, as a reusable function
def identify(url):
    response = requests.get(url)
    _img = Image.open(BytesIO(response.content))
    print("A wild {} appears!".format(classes[predict_this(_img)]))
    display(_img)

identify("https://bit.ly/2VQ32fd")




