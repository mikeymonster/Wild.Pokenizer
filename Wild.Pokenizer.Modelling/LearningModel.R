# install.packages("imager")

library(keras)
library(imager)

source("Globals.R") # Load global variables


# Create and train the network
# See https://www.kaggle.com/kwisatzhaderach/neural-networks-with-pokemon 
# Data source https://www.kaggle.com/mrgravelord/complete-pokemon-image-dataset


# Use specific environment already set up in Anaconda 
{
  if(!is.na(condaEnvironmentName)) {
    message("Setting conda enironment to ", condaEnvoronmentName)
    use_condaenv(condaEnvoronmentName)
  } 
}




