library(keras)
# library(imager)

source("Globals.R") # Load global variables

# Use specific environment already set up in Anaconda 
{
  if(!is.na(condaEnvironmentName)) {
    message("Setting conda enironment to ", condaEnvironmentName)
    use_condaenv(condaEnvironmentName)
  } 
}

tensorBoard_log_file <- "pokemon_run_new"
tensorBoard_log_file_path <- file.path("logs", tensorBoard_log_file)


# Adapted from http://flovv.github.io/Logo_detection_transfer_learning/
################### Section 1 #########################
img_width <- 160
img_height <- 160
batch_size <- 32 #8

train_directory <- training_data_path
test_directory <- validation_data_path

train_samples <- length(list.files(training_data_path, include.dirs = TRUE, recursive = TRUE))
validation_samples <- length(list.files(validation_data_path, include.dirs = TRUE, recursive = TRUE))
num_classes <- length(list.files(dataFilePath, include.dirs = TRUE ))

# Try adding some preprocessing
data_generator <-  image_data_generator(rescale = 1/255,
                                        horizontal_flip = TRUE,
                                        vertical_flip = FALSE,
                                        brightness_range = c(0.5, 1.5),
                                        rotation_range = as.integer(10)
                                        #validation_split = 0.2
                                        )


train_generator <- flow_images_from_directory(train_directory, 
                                              generator = data_generator, #image_data_generator(),
                                              target_size = c(img_width, img_height), 
                                              color_mode = "rgb",
                                              class_mode = "categorical", 
                                              batch_size = batch_size, 
                                              shuffle = TRUE,
                                              seed = 123)

validation_generator <- flow_images_from_directory(test_directory, 
                                                   generator = data_generator, #image_data_generator(),
                                                   target_size = c(img_width, img_height), 
                                                   color_mode = "rgb", 
                                                   classes = NULL,
                                                   class_mode = "categorical", 
                                                   batch_size = batch_size, 
                                                   shuffle = TRUE,
                                                   seed = 123)

################### Section 2 #########################
#base_model <- application_inception_v3(weights = 'imagenet', include_top = FALSE)
base_model <- application_vgg16(weights = 'imagenet', include_top = FALSE)
### use vgg16 -  as inception won't converge --- 

################### Section 3 #########################
## add your custom layers
predictions <- base_model$output %>% 
  layer_global_average_pooling_2d(trainable = T) %>% 
  #layer_dense(64, trainable = T) %>%
  layer_dense(1024, trainable = T) %>%
  layer_activation("relu", trainable = T) %>%
  layer_dropout(0.4, trainable = T) %>%
  layer_dense(num_classes, trainable = T) %>%    ## important to adapt to fit the 27 classes in the dataset!
  layer_activation("softmax", trainable = T)
# this is the model we will train
model <- keras_model(inputs = base_model$input, outputs = predictions)

################### Section 4 #########################
for (layer in base_model$layers)
  layer$trainable <- FALSE

################### Section 5 #########################
model %>% compile(
  loss = "categorical_crossentropy",
  optimizer = optimizer_rmsprop(lr = 0.003, decay = 1e-6),  ## play with the learning rate
  metrics = "accuracy"
)

hist <- model %>% fit_generator(
  train_generator,
  steps_per_epoch = as.integer(train_samples/batch_size), 
  epochs = 5, #20, 
  validation_data = validation_generator,
  validation_steps = as.integer(validation_samples/batch_size),
  verbose=2
)

### saveable data frame obejct.
histDF <- data.frame(acc = unlist(hist$history$acc), val_acc=unlist(hist$history$val_acc), val_loss = unlist(hist$history$val_loss),loss = unlist(hist$history$loss))

df_history <- as.data.frame(history)
ggplot(df_history) +
  geom_point(mapping = aes(x = epoch, y = value, color = metric, )) +
  geom_line(mapping = aes(x = epoch, y = value, color = metric, ))

