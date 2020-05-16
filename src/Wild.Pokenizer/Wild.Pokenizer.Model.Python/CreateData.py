"""
Load data from a zip file, then split it into train/val/test sets and save to the data directory
"""

import os
import numpy as np
import pandas as pd
from sklearn import model_selection
import zipfile
import config #shared configuration module 

#Define functions
def loadSourceData(archive):
    """
    Opens the archive and returns a data frame containing the zipInfo objects with dir/filename details
    """
    
    labels = [x.filename.strip("/") for x in archive.filelist if x.file_size == 0]
    
    # Need to get the data into a dataframe
    #https://www.geeksforgeeks.org/python-pandas-dataframe/
    df = pd.DataFrame({'dataobj':archive.filelist})

    len_before = len(df)

    #Pull file name and size out of the zipInfo object
    df[['filename', 'size']] = df.apply(lambda x : pd.Series([x['dataobj'].filename, x['dataobj'].file_size]),axis=1)
    df['dir'] = df['filename'].str.split('/').str[0]
    df['file'] = df['filename'].str.split('/').str[1]
    #Drop rows with size 0 (these are directories, not files)
    df = df[df['size'] > 0]
    print('dropped ' + str(len_before - len(df)) + ' rows with file size 0')
    #reorder columns
    df = df[['dir', 'file', 'size', 'dataobj']]

    #Probably want to return a test and a train split
    return df, labels

def splitData(data, groupCol, train_ratio = 0.75, validation_ratio = 0.15, random_state = 40):
    """
    Split the data into train/test
    groupCol is the column to stratify/group on
    """
    
    test_ratio = 1 - train_ratio - validation_ratio

    X_train, X_test = model_selection.train_test_split(data, 
                                                       stratify=data[groupCol], 
                                                       test_size=1 - train_ratio, 
                                                       random_state=random_state)
    X_val, X_test =  model_selection.train_test_split(X_test, 
                                                       stratify=X_test[groupCol], 
                                                       test_size=test_ratio/(test_ratio + validation_ratio), 
                                                       random_state=random_state) 
    #TODO: Split 3-ways https://datascience.stackexchange.com/questions/15135/train-test-validation-set-splitting-in-sklearn
    """
# train is now 75% of the entire data set
# the _junk suffix means that we drop that variable completely
X_train, X_test, y_train, y_test = train_test_split(dataX, dataY, test_size=1 - train_ratio)

# test is now 10% of the initial data set
# validation is now 15% of the initial data set
X_val, X_test, y_val, y_test = train_test_split(x_test, y_test, test_size=test_ratio/(test_ratio + validation_ratio)) 


"""
    return X_train, X_val, X_test

def extractDataToDisk(archive, data, outputDir, outputSubDir, zipInfoCol='dataobj', fileNameCol='file'):
    """
    Save the data to folders
    archive is the zip archive that data will be extracted from - it should be open
    data should contain zipInfo objects from the open archive
    zipInfoCol is the name of the column containing a ZipIno object
    """
    if data is None:
        print('No input data provided')
        return

    for index, row in data.iterrows():
        filePath = os.path.normpath(os.path.join(outputDir, outputSubDir)) #, row['file']))
        print('Extracting ' + row[fileNameCol] + ' to ' + filePath)
        archive._extract_member(row[zipInfoCol], filePath, pwd=None)
    return


#Processing starts here...
#print('Working directory: ' + os.getcwd())

#Set random seed so results will be reproducible
np.random.seed(config.RANDOM_SEED)

archiveFile = zipfile.ZipFile(config.DATA_ARCHIVE_PATH, 'r')

all_data, labels = loadSourceData(archiveFile)

train_set, validation_set, test_set = splitData(all_data, groupCol='dir')

extractDataToDisk(archiveFile, train_set, config.DATA_DIR, 'train')
extractDataToDisk(archiveFile, validation_set, config.DATA_DIR, 'val')
extractDataToDisk(archiveFile, test_set, config.DATA_DIR, 'test')

archiveFile.close()
