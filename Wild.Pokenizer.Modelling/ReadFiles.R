library(imager)

source("Globals.R") # Load global variables

#files
dirs <- list.files(dataFilePath, include.dirs= TRUE )
dirs
files <- list.files(dataFilePath, recursive=TRUE )
files
# lapply(files, function(x) print(x))

summary(files)

# Show a random file
f <- files[sample(1:length(files),1)]
filePath <- paste0(dataFilePath, '/', f) 
im <- load.image(filePath)
plot(im)

