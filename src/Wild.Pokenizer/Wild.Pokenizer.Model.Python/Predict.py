import os
from io import BytesIO
import matplotlib.pyplot as plt
from PIL import Image
import numpy as np
import requests
import tensorflow as tf
import config #shared configuration module 

#Code adapted from https://towardsdatascience.com/transfer-learning-using-mobilenet-and-keras-c75daf7ff299

#Assumes the model is already loaded
#Assumes the class labels are in a list called classes

def display_image(img):
    plt.imshow(img)
    plt.axis('off')
    plt.draw()
    plt.pause(1.)
    #input("Press enter to continue...")
    plt.close()      

def load_image(img_path, show=False, width=256, height=256):

    img = tf.keras.preprocessing.image.load_img(img_path, target_size=(width, height))
    img_tensor = tf.keras.preprocessing.image.img_to_array(img)                    # (height, width, channels)
    img_tensor = np.expand_dims(img_tensor, axis=0)         # (1, height, width, channels), add a dimension because the model expects this shape: (batch_size, height, width, channels)
    img_tensor /= 255.                                      # imshow expects values in the range [0, 1]

    if show:
        display_image(img_tensor[0])

    return img_tensor

#prediction function from the old project
def predict_this(this_img):
    im = this_img.resize((160,160)) # size expected by network
    img_array = np.array(im)
    img_array = img_array/255 # rescale pixel intensity as expected by network
    img_array = np.expand_dims(img_array, axis=0) # reshape from (160,160,3) to (1,160,160,3)
    pred = model.predict(img_array)
    return np.argmax(pred, axis=1).tolist()[0]

#Identify and display from a url of an image
def identify(url):
    response = requests.get(url)
    _img = Image.open(BytesIO(response.content))
    print("A {} appears!".format(classes[predict_this(_img)]))
    display_image(_img)

#img_path = 'C:/Users/Ferhat/Python Code/Workshop/Tensoorflow transfer learning/blue_tit.jpg'
img_path = os.path.join(config.TEST_DATA_DIR, 'Pikachu\Pikachu_12.jpg')
img_path
new_image = load_image(img_path, width=config.IMG_WIDTH, height=config.IMG_HEIGHT, show=True)

pred = model.predict(new_image)
pred
predicted_index = np.argmax(pred, axis=1).tolist()[0]
print("A {} appears!".format(classes[predicted_index]))
display_image(new_image[0])

identify("https://bit.ly/2VQ32fd")

