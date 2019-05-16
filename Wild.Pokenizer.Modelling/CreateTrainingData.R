# Set up training and testdata

source("Globals.R") # Load global variables

# training_data_path <- "../data/train"
# test_data_path <- "../data/test"
# validation_data_path <- "../data/validation"

# training_set_size <- 0.7
# test_set_size <- 0.2
# validation_set_size <- 0.0

# TODO: 
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
if (dir.exists(test_data_path)){
  unlink(test_data_path, recursive = TRUE)
}  

if (dir.exists(validation_data_path)){
  unlink(validation_data_path, recursive = TRUE)
}  

# Create target directories

dir.create(training_data_path)
dir.create(test_data_path)
dir.create(validation_data_path)

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
  source_dir <- paste0(dataFilePath ,"/", dir)
  train_dir <- paste0(training_data_path ,"/", dir)
  test_dir <- paste0(test_data_path ,"/", dir)
  val_dir <- paste0(validation_data_path ,"/", dir)

  message("Creating directories for ", dir, " and copying files from ", source_dir)
  dir.create(train_dir)
  dir.create(test_dir)
  dir.create(val_dir)

  # Get a list of files
  files <- list.files(source_dir)
  number_of_files_in_training_set <- training_set_size * length(files)
  number_of_files_in_test_set <- test_set_size * length(files)
  number_of_files_in_validation_set <- validation_set_size * length(files)
  
  # get random samples and copy files

  if(number_of_files_in_training_set > 0)
  {
    print("creating training data")
    # lapply(files, function(x) print(x))
    #summary(files)
    
    # Get test set, and remove all of the ones taken from the files list
    sample_indices <- sample(1:length(files), number_of_files_in_training_set)
    sample_set <- files[sample_indices]
    files <- files[-sample_indices]
    
    #summary(sample_set)
    #sample_set
    #sample_indices
    #files
    #class(files)
    
    # file.copy(file.path(original_dataset_dir, fnames), file.path(validation_cats_dir))
    
    for(f in sample_set)
    {
      message("f = ", f)
      filePath <- paste0(source_dir, '/', f) 
      print(filePath)
      ?file.copy()
      file.copy(filePath, train_dir, overwrite=TRUE)
    }
  }

  if(number_of_files_in_test_set > 0)
  {
    print("creating test data")
  }
  
  if(number_of_files_in_validation_set > 0)
  {
    print("creating validation data")
  }
}



