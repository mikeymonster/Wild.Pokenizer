# Set up variables

# Set condaEnvironmentName to NA for default
# condaEnvironmentName <- NA
condaEnvironmentName <- "tf-gpu"

# Data source files
# dataFilePath <-  "../data/cut-down-pokemon"
dataFilePath <-  "../data/complete-pokemon-image-dataset"

training_data_path <- "../data/train"
validation_data_path <- "../data/validation"
test_data_path <- "../data/test"

# Sizes of trining and test sets etc
training_set_size <- 0.7
validation_set_size <- 0.2
test_set_size <- max(0, 1 - training_set_size - validation_set_size)

