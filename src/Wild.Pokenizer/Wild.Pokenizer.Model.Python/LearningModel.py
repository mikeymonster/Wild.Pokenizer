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

def splitData(data, groupCol, test_size = 0.2, random_state = 40):
    """
    Split the data into train/test
    groupCol is the column to stratify/group on
    """
    X_train, X_test = model_selection.train_test_split(data, 
                                                       stratify=data[groupCol], 
                                                       test_size=test_size, 
                                                       random_state=random_state)
    return X_train, X_test

def extractDataToDisk(archive, data, outputDir, outputSubDir, zipInfoCol='dataobj', fileNameCol='file'):
    """
    Save the data to folders
    archive is the zip archive that data will be extracted from - it should be open
    data should contain zipInfo objects from the open archive
    zipInfoCol is the name of the column containing a ZipIno object
    """

    for index, row in data.iterrows():
        filePath = os.path.normpath(os.path.join(outputDir, outputSubDir)) #, row['file']))
        print('Extracting ' + row[fileNameCol] + ' to ' + filePath)
        archive._extract_member(row[zipInfoCol], filePath, pwd=None)
    return


#Processing starts here...
print('Working directory: ' + os.getcwd())
print('TensorFlow version: ' + tf.__version__ + ' Keras version:' + tf.keras.__version__)

#Set random seed so results will be reproducible
np.random.seed(348258)

#dataArchivePath='../../../data/complete-pokemon-image-dataset.zip'
dataArchivePath='D:/datascience/complete-pokemon-image-dataset.zip'
dataPath = '../../../data/pokemon-image-dataset'

zipFile = zipfile.ZipFile(dataArchivePath, 'r')

all_data, labels = createTestData(zipFile, 
                                  outputPath='../../../data/')

train_set, test_set = splitData(all_data, groupCol='dir')

extractDataToDisk(zipFile, train_set, dataPath, 'train')
extractDataToDisk(zipFile, test_set, dataPath, 'test')

zipFile.close()

"""
#Playing around with splitting...
#https://stackoverflow.com/questions/53196174/split-into-train-and-test-by-group-sklearn-cross-val-score
data = all_data.head(80)
data = data[['dir', 'file']]

data
data['file']
test_size = 0.2
random_state = 40
classNameCol = "dir"
#GroupShuffleSplit won't include every group (class label) so isn't the one we want
#https://scikit-learn.org/stable/modules/generated/sklearn.model_selection.GroupShuffleSplit.html
splitter = model_selection.GroupShuffleSplit(n_splits = 1, test_size = test_size, random_state=random_state)
idx1, idx2 = next(splitter.split(data, groups=data.dir))
idx1
data
df1, df2 = data.iloc[idx1], data.iloc[idx2]
df1
df2

#toy data set for testing splits
df=pd.DataFrame({'0':[17.0, 18.0,16.0, 12.0,  8.0, 21.0], 
                 '1':[18.0, 16.0, 15.0,  8.0, 21.0, 19.0], 
                 '2':[16.0, 15.0, 15.0, 21.0, 19.0, 20.0], 
                 '3':[15.0, 15.0, 16.0, 19.0, 20.0,  9.0], 
                 'ids':[13.0, 13.0, 13.0, 14.0, 14.0, 13.0]}) 
df
gss = model_selection.GroupShuffleSplit(n_splits=1, test_size=0.5)
# Get the indexers for the split.
idx1, idx2 = next(gss.split(df, groups=df.ids))
idx1, idx2 = next(gss.split(df, groups=df['ids']))
# Get the split DataFrames.
df1, df2 = df.iloc[idx1], df.iloc[idx2]
df1
df2
data
X_train, X_test, y_train, y_test = train_test_split(df['data'], df['labels'],stratify=df['ids'])
X_train, X_test = model_selection.train_test_split(df, stratify=df['ids'])
X_train, X_test = model_selection.train_test_split(all_data, stratify=all_data['dir'], test_size=0.5)
X_train
X_test
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
