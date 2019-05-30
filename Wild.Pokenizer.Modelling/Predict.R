# install.packages("magick")

# https://cran.r-project.org/web/packages/magick/vignettes/intro.html
library(magick)
library(keras)

source("Globals.R") # Load global variables

# Use specific environment already set up in Anaconda 
{
  if(!is.na(condaEnvironmentName)) {
    message("Setting conda enironment to ", condaEnvironmentName)
    use_condaenv(condaEnvironmentName)
  } 
}


#Functions
predict_pokemon <- function(input_image)
{
  #im = input_image.resize(c(160,160)) # size expected by network
  im <-  image_array_resize(image1, width = 160, height = 160)
  
  ##Done to here
  
  img_array = np.array(im)
  img_array = img_array/255 # rescale pixel intensity as expected by network
  img_array = np.expand_dims(img_array, axis=0) # reshape from (160,160,3) to (1,160,160,3)
  pred = model.predict(img_array)
  return np.argmax(pred, axis=1).tolist()[0]
}


# Reload model
model_filepath <- '../data/InceptionV3_Pokemon.h5'
model <- load_model_hdf5(model_filepath, custom_objects = NULL, compile = TRUE)



#TODO: Turn this into a function to download and save image
require(magick)
url <- 'https://i.imgur.com/5Nycvcx.jpg'
image_path <- "../data/test_images/jigglypuff.jpg"
img_1 <- image_read(url) #, width = 400)
image_write(img_1, path = image_path, format = "jpg")

url <- 'https://bit.ly/2VQ32fd'
image_path <- "../data/test_images/pikachu.jpg"
img_1 <- image_read(url) #, width = 400)
image_write(img_1, path = image_path, format = "jpg")




# Keras image load, reshape and plot
img <- image_load(image_path) #, grayscale = FALSE, target_size = c(160, 160))
img <-  image_array_resize(img, width = 160, height = 160)
image_tensor <-  image_to_array(img)
image_tensor <-  array_reshape(image_tensor, c(1, 160, 160, 3))
image_tensor <-  image_tensor / 255
plot(as.raster(image_tensor[1,,,]))

#Predict
prediction <-  model %>% predict(image_tensor)

classes <-  train_generator$class_indices
ix <- which.max(prediction)
ix
classes[ix]
message("A wild ", names(classes[ix]), " appears!" )

#####################




prediction
summary(prediction[1])
#predict_classes(model, image_tensor)
?predict_classes
max(prediction)

# This might work...
ix_tensor <- k_argmax(prediction, axis = 1)
ix_tensor
ix <- k_get_value(ix_tensor[1])
ix
prediction[3411]


# try
ix <- k_eval(ix_tensor)

# try 


#k_argmax(prediction, axis = -2)
#k_argmax(prediction, axis = -1)
#k_argmax(prediction, axis = 1)
#k_argmax(prediction, axis = 2)

argmax(prediction)

k_argmax(prediction, axis = 1)[[0]]
#return np.argmax(pred, axis=1).tolist()[0]


##Mkght need something like this to get the class labels:

#    {'cats': 0, 'dogs': 1}



?image_load

?image_array_resize
#result <- predict_pokemon(image_tensor)
message("A wild ", result , " appears!" )


