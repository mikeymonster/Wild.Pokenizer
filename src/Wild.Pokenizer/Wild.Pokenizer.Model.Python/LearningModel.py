from io import BytesIO
import os
import requests
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from PIL import Image
from sklearn import model_selection
import tensorflow as tf
import zipfile


def createTestData(archive, outputPath='./data/', test_size = 0.2, random_state = 40):
    """
    Opens the archive and returns a data frame containing the zipInfo objects with dir/filename details
    """
    labels = [x.filename.strip("/") for x in archive.filelist if x.file_size == 0]

    # Need to get the data into a dataframe
    #https://www.geeksforgeeks.org/python-pandas-dataframe/
    df = pd.DataFrame({'dataobj':archive.filelist})

    before = len(df)
    #Pull file name and size out of the zipInfo object
    df[['filename', 'size']] = df.apply(lambda x : pd.Series([x['dataobj'].filename, x['dataobj'].file_size]),axis=1)
    df['dir'] = df['filename'].str.split('/').str[0]
    df['file'] = df['filename'].str.split('/').str[1]
    #Drop rows with size 0 (these are directories, not files)
    df = df[df['size'] > 0]
    print('dropped ' + str(before - after) + ' rows with file size 0')
    #reorder columns
    df = df[['dir', 'file', 'size', 'dataobj']]

    #Probably want to return a test and a train split
    return df, labels


def splitData(archive, data, outputDir='./data/', test_size = 0.2, random_state = 40):
    """
    Split the data int train/test and extract to folders
    data should contain zipInfo objects from an open archive
    """

    for index, row in data.iterrows():
        print(index, row['dir'], row['file'])
        #filePath = os.path.normpath(os.path.join(outputDir, "test", row['dir'], row['file']))
        filePath = os.path.normpath(os.path.join(outputDir, ("train" if index %2 == 0 else "test")))
        print('extacting to ' + filePath)
        archive._extract_member(row['dataobj'], filePath, pwd=None)
    return

print('Working directory: ' + os.getcwd())
print('TensorFlow version: ' + tf.__version__ + ' Keras version:' + tf.keras.__version__)

#Set random seed so results will be reproducible
np.random.seed(348258)

#dataPath='../../../data/complete-pokemon-image-dataset.zip'
dataPath='D:/datascience/complete-pokemon-image-dataset.zip'

zipped_data = zipfile.ZipFile(dataPath, 'r')

all_data, labels = createTestData(zipped_data, 
                                  outputPath='../../../data/')

splitData(zipped_data, 
          #all_data.head(50), 
          all_data, 
          outputDir='../../../data/')

zipped_data.close()


"""
#Snippet for splitting data - ref p106
#https://scikit-learn.org/stable/modules/generated/sklearn.model_selection.GroupShuffleSplit.html
test_size = 0.2
random_state = 40
classNameCol = "dir"
splitter = model_selection.GroupShuffleSplit(
    nsplits = 1,
    test_size = test_size,
    random_state=random_state)
splits = splitter.split(df, groups=df[className])
"""

#Set up model
batch_size = 24
num_classes = len(labels) # this many classes of Pokemon in the dataset
#custom_input = Input(shape=(160,160,3,))


"""
#history = model.fit()
#Snippet for plotting history from fit - ref p305
pd.DataFrame(history.history).plot(figsize=(8, 5))
plt.grid(True)
plt.gca().set_ylim(0, 1)
plt.show()
"""

#Evaluate the model agains the test data
#model.evaluate(X_test, y_test)


"""
#snippet for showing an image
url = 'https://i.imgur.com/5Nycvcx.jpg'
response = requests.get(url)
img_1 = Image.open(BytesIO(response.content))
#display(img_1)
plt.imshow(img_1)
plt.show()
"""


"""
x[1]
i = archive.open(x[1]) #as thefile:
plt.imshow(i.read())
plt.show()
"""
