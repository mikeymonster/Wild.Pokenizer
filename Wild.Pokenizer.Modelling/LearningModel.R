library(keras)
# library(imager)

source("Globals.R") # Load global variables


# Create and train the network
# See https://www.kaggle.com/kwisatzhaderach/neural-networks-with-pokemon 
# Data source https://www.kaggle.com/mrgravelord/complete-pokemon-image-dataset
#
# See also https://github.com/jjallaire/deep-learning-with-r-notebooks/blob/master/notebooks/5.3-using-a-pretrained-convnet.Rmd
#
# Another example - http://flovv.github.io/Logo_detection_transfer_learning/


# Use specific environment already set up in Anaconda 
{
  if(!is.na(condaEnvironmentName)) {
    message("Setting conda enironment to ", condaEnvironmentName)
    use_condaenv(condaEnvironmentName)
  } 
}

####

tensorBoard_log_file <- "pokemon_run"
tensorBoard_log_file_path <- file.path("logs", tensorBoard_log_file)

batch_size = as.integer(24)
#is.integer(batch_size)

#num_classes <-  928 # this many classes of Pokemon in the dataset
num_classes <- length(list.files(dataFilePath, include.dirs = TRUE ))

# https://keras.io/preprocessing/image/
data_generator <-  image_data_generator(rescale = 1/255,
                                    horizontal_flip = TRUE,
                                    vertical_flip = FALSE,
                                    brightness_range = c(0.5, 1.5),
                                    rotation_range = as.integer(10),
                                    validation_split = 0.2)

train_generator <- data_generator$flow_from_directory(dataFilePath,
                                                     target_size = as.integer(c(160,160)), # chosen because this is size of the images in dataset
                                                     batch_size = batch_size,
                                                     shuffle = TRUE,
                                                     subset ='training')

val_generator <- data_generator$flow_from_directory(dataFilePath,
                                                   target_size = as.integer(c(160,160)),
                                                   batch_size = batch_size,
                                                   subset = 'validation')

# import the base model and pretrained weights
#custom_input = layer_input(shape=c(160,160,3,NULL))
custom_input = layer_input(shape=c(160,160,3))

base_model = application_inception_v3(
  include_top = FALSE, 
  weights ='imagenet',
  input_tensor = custom_input,
  #input_shape = NULL, 
  pooling = NULL, 
  classes = num_classes)

#base_model  <- load_model_hdf5(file.path("../data", "inception_v3_weights_tf_dim_ordering_tf_kernels_notop.h5"))
#file.path("./data", "inception_v3_weights_tf_dim_ordering_tf_kernels_notop.h5")


#x = base_model.layers[-1].output # snag the last layer of the imported model
x <- base_model$output %>% 
  layer_global_max_pooling_2d(data_format = "channels_last") %>% 
  layer_dense(units = 1024, activation = "relu") %>% 
  layer_dense(units = num_classes, activation = "softmax", name = "predictions") # our new, custom prediction layer
x

model = keras_model(input = base_model$input, output = x)
# model

for (layer in model$layers) {
  layer$training = TRUE
}


# these are utilities to maximize learning, while preventing over-fitting
#reduce_lr = ReduceLROnPlateau(monitor='val_loss', patience=12, cooldown=6, rate=0.6, min_lr=1e-18, verbose=1)
#early_stop = EarlyStopping(monitor='val_loss', patience=24, verbose=1)
reduce_lr <- callback_reduce_lr_on_plateau(monitor = "val_loss", 
                                           patience = 12, 
                                           cooldown = 6, 
                                           #rate = 0.6, 
                                           min_lr = 1e-18, 
                                           verbose = 1)
early_stop <- callback_early_stopping(monitor = 'val_loss', patience = 24,  verbose = 1)
tensorboard <-  callback_tensorboard(tensorBoard_log_file_path)

# compile and train the network

model %>% compile(
  optimizer = optimizer_adam(1e-8),
  loss = "categorical_crossentropy",
  metrics = c("accuracy")
)

#?fit_generator
history <- model %>% fit_generator(train_generator,
                        validation_data = val_generator,
                        steps_per_epoch = as.integer(2000/batch_size),
                        validation_steps = as.integer(800/batch_size),
                        initial_epoch = 1,
                        epochs = 50,
                        callbacks = c(reduce_lr, early_stop, tensorboard),
                        verbose = 2)

plot(history)

# Show tensorboard
tensorboard(tensorBoard_log_file_path)

# here's how to save the model after training. Use ModelCheckpoint callback to save mid-training.
model_filepath <- '../data/InceptionV3_Pokemon_trained.h5'
model %>% save_model_hdf5(model_filepath)
#model <- load_model_hdf5(model_filepath, custom_objects = NULL, compile = TRUE)

