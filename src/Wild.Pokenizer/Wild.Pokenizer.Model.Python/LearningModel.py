from io import BytesIO
import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from PIL import Image
from sklearn import model_selection
import requests
import zipfile

import tensorflow as tf
#import tensorflow.keras as keras

print('Working directory: ' + os.getcwd())
print('TensorFlow version: ' + tf.__version__)
print('Keras version:' + tf.keras.__version__)

"""
Data is in ../../../data/complete-pokemon-image-dataset.zip
"""

#archivePath = '../../../data/complete-pokemon-image-dataset.zip'

#archive = zipfile.ZipFile(archivePath, 'r')

#labels = [x.filename.strip("/") for x in archive.filelist if x.file_size == 0]
#len(labels)
#labels

#len([x for x in archive.filelist if x.file_size > 0])
#24646

#for f in [x for x in archive.filelist if x.file_size > 0]:
#  print(f.filename)

#Plan: group all the file names by label (the part before the /) then make test/train split, then extract

#x = archive.filelist
#x[2].filename
##If file size is 0 then this is a directory
#x[1].file_size

##Extract a single file
#targetPath = '../../../data/train/' + x[1].filename
#archive._extract_member(x[2], targetPath, pwd=None)


# Need to get the data into a dataframe
#https://www.geeksforgeeks.org/python-pandas-dataframe/
#df = pd.DataFrame({'dataobj':archive.filelist})
##Optionally: use a list comprehension to only include files (size > 0)
##df = pd.DataFrame({'dataobj':[x for x in archive.filelist if x.file_size > 0]})
##df.columns = ['dataobj']
#before = len(df)
#df[['filename', 'size']] = df.apply(lambda x : pd.Series([x['dataobj'].filename, x['dataobj'].file_size]),axis=1)
##df['filename']
##df['size']
#df = df[df['size'] > 0]
#df['dir'] = df['filename'].str.split('/').str[0]
#df['file'] = df['filename'].str.split('/').str[1]
#df
#df = df[['dir', 'file', 'size', 'dataobj']]
##df
#after = len(df)
#print('dropped ' + str(before - len(df)) + ' rows with file size 0')


#"""
## new data frame with split value columns 
#new = data["Name"].str.split(" ", n = 1, expand = True) 
  
## making separate first name column from new data frame 
#data["First Name"]= new[0] 
  
## making separate last name column from new data frame 
#data["Last Name"]= new[1] 
  
## Dropping old Name columns 
#data.drop(columns =["Name"], inplace = True) 
#"""

#df[['dir', 'file']]
#df['file']

#"""
#https://stackoverflow.com/questions/36081217/efficiently-access-property-of-objects-in-pandas-column
#def get_week(x):
#    return x.weekofyear

#df.x.apply(get_week)
#"""
#df["dataobj"][0].filename
#df["dataobj"][0].file_size

def createTestData(archive, outputPath='./data/', test_size = 0.2, random_state = 40):
    """
    Opens the archive and returns a data frame containing the zipInfo objects with dir/filename details
    """
    #archive = zipfile.ZipFile(archivePath, 'r')
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

archivePath='../../../data/complete-pokemon-image-dataset.zip'
archive = zipfile.ZipFile(archivePath, 'r')
all_data, labels = createTestData(archive, outputPath='../../../data/')

#all_data['file']
#sub = all_data.head(5)
#outputDir = '../../../data/'
#for index, row in sub.iterrows():
#    print(index, row['dir'], row['file'])
#    #filePath = os.path.normpath(os.path.join(outputDir, "test", row['dir'], row['file']))
#    filePath = os.path.normpath(os.path.join(outputDir, ("test" if index %2 == 0 else "train")))
#    #xx = ("test" if index %2 == 0 else "train")
#    print('extacting to ' + filePath)
    
    #targetPath = '../../../data/train/' + x[1].filename
#archive._extract_member(x[2], targetPath, pwd=None)


def splitData(zipArchive, data, outputDir='./data/', test_size = 0.2, random_state = 40):
    """
    Split the data int train/test and extract to folders
    data should contain zipInfo objects from an open archive
    """

    for index, row in data.iterrows():
        print(index, row['dir'], row['file'])
        #filePath = os.path.normpath(os.path.join(outputDir, "test", row['dir'], row['file']))
        filePath = os.path.normpath(os.path.join(outputDir, ("train" if index %2 == 0 else "test")))
        print('extacting to ' + filePath)
        zipArchive._extract_member(row['dataobj'], filePath, pwd=None)
    return

splitData(archive, 
          #all_data.head(50), 
          all_data, 
          outputDir='../../../data/')


#Next - extract data to train/test

archive.close()


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









#Set random seed so results will be reproducible
np.random.seed(348258)

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
