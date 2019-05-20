# Set up training and testdata

source("Globals.R") # Load global variables

# Create directories for training/test/validation data
# If any directories already exist, delete them 
# Read all data dirs and create corresponding dirs in training/test/validation
# Copy samples of files to the directories

# See https://github.com/jjallaire/deep-learning-with-r-notebooks/blob/master/notebooks/5.2-using-convnets-with-small-datasets.Rmd
# dir.create(file.path(mainDir, subDir), showWarnings = FALSE)


# Remove existing data, if any
if (dir.exists(training_data_path)){
  unlink(training_data_path, recursive = TRUE)
}
if (dir.exists(validation_data_path)){
  unlink(validation_data_path, recursive = TRUE)
}  
if (dir.exists(test_data_path)){
  unlink(test_data_path, recursive = TRUE)
}

# Create target directories

dir.create(training_data_path)
dir.create(validation_data_path)
dir.create(test_data_path)

#output_dir <- file.path(main_dir, sub_dir)

#if (!dir.exists(output_dir)){
#  dir.create(output_dir)
#} else {
#  print("Dir already exists!")
#}

# Get source directories

sourceDirs <- list.files(dataFilePath, include.dirs = TRUE )

for(dir in sourceDirs)
{
  source_dir <- file.path(dataFilePath, dir)
  val_dir <- file.path(validation_data_path, dir)
  train_dir <- file.path(training_data_path, dir)
  test_dir <- file.path(test_data_path, dir)
  
  message("Creating directories for ", dir, " and copying files from ", source_dir)
  dir.create(train_dir)
  dir.create(val_dir)
  dir.create(test_dir)

  # Get a list of files
  files <- list.files(source_dir)
  number_of_files_in_training_set <- training_set_size * length(files)
  number_of_files_in_validation_set <- validation_set_size * length(files)
  number_of_files_in_test_set <- test_set_size * length(files)
  
  # get random samples and copy files

    print("creating training data")
    # lapply(files, function(x) print(x))
    #summary(files)
    
    # Get test set, and remove all of the ones taken from the files list
    train_sample_indices <- sample(1:length(files), number_of_files_in_training_set)
    train_sample_set <- files[train_sample_indices]

    if(number_of_files_in_test_set > 0)
    {
      print("creating test data")
      files <- files[-train_sample_indices]
      validation_sample_indices <- sample(1:length(files), number_of_files_in_validation_set)
      validation_sample_set <- files[validation_sample_indices]
      test_sample_set <- files[-validation_sample_indices]
    }
    else
    {
      validation_sample_set <- files[-train_sample_indices]
    }
    
    #summary(train_sample_set)
    #train_sample_set
    #sample_indices
    #files
    #class(files)
    
    file.copy(file.path(source_dir, train_sample_set), file.path(train_dir))
    file.copy(file.path(source_dir, validation_sample_set), file.path(val_dir))
    if(number_of_files_in_test_set > 0)
    {
      file.copy(file.path(source_dir, test_sample_set), file.path(test_dir))
    }
}


# Basic validation of files created for the last set

df <- as.data.frame(train_sample_set)
df2 <- as.data.frame(validation_sample_set)
names(df2) <- c("fname")
names(df) <- c("fname")
df <- rbind(df2, df)

if(number_of_files_in_test_set > 0)
{
  df3 <- as.data.frame(test_sample_set)
  names(df3) <- c("fname")
  df <- rbind(df3, df)
}

df$filepart <- sapply(strsplit(as.character(df$fname),'.jpg'), "[", 1)
df$label <- sapply(strsplit(as.character(df$filepart),'_'), "[", 1)
df$number <- as.numeric(sapply(strsplit(as.character(df$filepart),'_'), "[", 2))

df <- subset(df, select = -c(filepart))

newdata <- df[order(df$number),]
newdata

# write.table(newdata, "c:/temp/pikas1.csv", sep=",")

